using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager Instance { get{ return _instance; } }

    public enum GAMESTATE
    {
        MOVE = 0,
        COMBAT = 1,
        PAUSE = 2,
        GAMEEND = 3
    }

    public int width;
    public int height;
    public float gridSize;

    private GridSystem gridSystem;
    private Snake snake;

    private GAMESTATE gameState;

    private float elapsedTime;
    private float translateTime;
    private float combatTime;

    private float spawnElapseTime;
    private float spawnTime;

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        gridSystem = new GridSystem(width, height);

        gameState = GAMESTATE.MOVE;

        elapsedTime = 0;
        translateTime = 0.5f;
        combatTime = 0.5f;

        spawnElapseTime = 0;
        spawnTime = 3;
    }

    void Start()
    {
        InitiateSnake();
    }

    private void InitiateSnake()
    {
        snake = new Snake();

        int x = width / 2;
        int y = height / 2;

        Hero hero = CreateHero(x, y);
        snake.AddHero(hero);
    }

    private Hero CreateHero(int x, int y)
    {
        HeroController controller = CreateController<HeroController>(x, y);
        return new Hero(x, y, controller);
    }

    private Enemy CreateEnemy(int x, int y)
    {
        EnemyController controller = CreateController<EnemyController>(x, y);
        return new Enemy(x, y, controller);
    }

    public T CreateController<T>(int x, int y) where T : GridObjectController
    {
        GameObject prefab = null;
        if(typeof(T) == typeof(HeroController))
        {
            prefab = SpriteHolder.Instance.heroPrefab;
        }
        else if(typeof(T) == typeof(EnemyController))
        {
            prefab = SpriteHolder.Instance.enemyPrefab;
        }

        GameObject gameObject = Instantiate(prefab, GetRealPositionFromGridId(x, y), Quaternion.identity) as GameObject;
        SpriteRenderer sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        
        if (typeof(T) == typeof(HeroController))
        {
            sprite.sprite = SpriteHolder.Instance.GetRandomHeroSprite();
        }
        else if (typeof(T) == typeof(EnemyController))
        {
            sprite.sprite = SpriteHolder.Instance.GetRandomEnemySprite();
        }

        return gameObject.GetComponent<T>();
    }

    public Vector3 GetRealPositionFromGridId(int x, int y)
    {
        float posX = (x - 1.0f * width / 2) * gridSize;
        float posY = (y - 1.0f * height / 2) * gridSize;
        return new Vector3(posX, posY, 0);
    }

    void Update()
    {
        float deltatime = Time.deltaTime;
        UpdateGameTime(deltatime);
        UpdateSpawnTime(deltatime);
    }

    private void UpdateGameTime(float deltaTime)
    {
        elapsedTime += deltaTime;
        if (gameState == GAMESTATE.MOVE)
        {
            if (elapsedTime >= translateTime)
            {
                elapsedTime = 0;
                CheckMove();

                if (snake.IsEmpty())
                {
                    gameState = GAMESTATE.GAMEEND;
                }
            }
        }
        else if (gameState == GAMESTATE.COMBAT)
        {
            if (elapsedTime >= 0.5f)
            {
                elapsedTime = 0;
                Combat();

                if (snake.IsEmpty())
                {
                    gameState = GAMESTATE.GAMEEND;
                }
            }
        }
    }

    private void UpdateSpawnTime(float deltaTime)
    {
        spawnElapseTime += deltaTime;
        if(spawnElapseTime >= spawnTime)
        {
            spawnElapseTime = 0;
            Spawn();
        }
    }

    private void CheckMove()
    {
        int nextX = snake.GetNextX();
        int nextY = snake.GetNextY();

        //Debug.Log(nextX + ", " + nextY);

        if(gridSystem.IsBorder(nextX, nextY))
        {
            //snake.PopFront();
            if (snake.IsEmpty())
                return;
            ForceChangeSnakeDirection();
        }
        else if(gridSystem.HasObjectOnGrid(nextX, nextY))
        {
            GridObject gridObject = gridSystem.GetObjectOnGrid(nextX, nextY);
            if(gridObject is Hero)
            {
                snake.AddHero(gridObject as Hero);
                gridSystem.RemoveObject(nextX, nextY);
            }
            else if(gridObject is Enemy)
            {
                gameState = GAMESTATE.COMBAT;
                return;
            }
        }
        nextX = snake.GetNextX();
        nextY = snake.GetNextY();
        snake.MoveTo(GetRealPositionFromGridId(nextX, nextY));
    }

    private void ForceChangeSnakeDirection()
    {
        int r = Random.Range(0, 1);
        if (r == 0) // Turn Left
        {
            snake.TurnLeft();
            int nextX = snake.GetNextX();
            int nextY = snake.GetNextY();
            if (gridSystem.IsBorder(nextX, nextY))
            {
                snake.TurnRight();
                snake.TurnRight();
            }
        }
        else
        {
            snake.TurnRight();
            int nextX = snake.GetNextX();
            int nextY = snake.GetNextY();
            if (gridSystem.IsBorder(nextX, nextY))
            {
                snake.TurnLeft();
                snake.TurnLeft();
            }
        }
    }

    private void Combat()
    {

    }

    private void Spawn()
    {
        // Spawn Hero
        GridSystem.Point point = gridSystem.GetFreeSpace();
        if(point == null)
        {
            gameState = GAMESTATE.GAMEEND;
            return;
        }

        Hero newHero = CreateHero(point.x, point.y);
        gridSystem.AddObject(point.x, point.y, newHero);

        // Spawn Enemy
        GridSystem.Point point1 = gridSystem.GetFreeSpace();
        if (point1 == null)
        {
            gameState = GAMESTATE.GAMEEND;
            return;
        }

        Enemy newEnemy = CreateEnemy(point1.x, point1.y);
        gridSystem.AddObject(point1.x, point1.y, newEnemy);
    }
}
