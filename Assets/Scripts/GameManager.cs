using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

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

    public Text scoreText;
    public GameObject BG;
    public GameObject BG2;

    private GridSystem gridSystem;
    private Snake snake;

    private GAMESTATE gameState;

    private float elapsedTime;
    public float translateTime;
    public float decreaseTranslateTime;
    public float combatTime;

    private float spawnElapseTime;
    public float spawnTime;

    private Enemy combatEnemy;

    private int score;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        gridSystem = new GridSystem(width, height);

        gameState = GAMESTATE.MOVE;

        elapsedTime = 0;

        spawnElapseTime = 0;

        score = 0;

        combatEnemy = null;


        int spriteScale = 200;
        BG.transform.localScale = new Vector3(width * gridSize * spriteScale, height * gridSize * spriteScale, 5);
        BG2.transform.localScale = new Vector3(width * gridSize * spriteScale + spriteScale * gridSize, height * gridSize * spriteScale + spriteScale * gridSize, 6);

    }

    void Start()
    {
        InitiateSnake();
        Spawn();
    }

    private void InitiateSnake()
    {
        snake = new Snake();

        int x = width / 2;
        int y = height / 2;

        Hero hero = CreateHero(x, y);
        snake.AddHero(hero);
        gridSystem.AddObject(x, y, hero);
    }

    private Hero CreateHero(int x, int y)
    {
        HeroController controller = CreateController<HeroController>(x, y);
        Hero newHero = new Hero(x, y, controller);
        
        controller.SetAura(newHero.GetCharacterType());
        controller.SetHPLength(newHero.GetHeart());
        return newHero;
    }

    private Enemy CreateEnemy(int x, int y)
    {
        EnemyController controller = CreateController<EnemyController>(x, y);
        Enemy newEnemy = new Enemy(x, y, controller);

        controller.SetAura(newEnemy.GetCharacterType());
        controller.SetHPLength(newEnemy.GetHeart());
        return newEnemy;
    }

    public T CreateController<T>(int x, int y) where T : GridObjectController
    {
        GameObject prefab = null;
        if (typeof(T) == typeof(HeroController))
        {
            prefab = SpriteHolder.Instance.heroPrefab;
        }
        else if (typeof(T) == typeof(EnemyController))
        {
            prefab = SpriteHolder.Instance.enemyPrefab;
        }

        GameObject newGameObject = Instantiate(prefab, GetRealPositionFromGridId(x, y), Quaternion.identity) as GameObject;
        SpriteRenderer sprite = newGameObject.transform.Find("Character").GetComponent<SpriteRenderer>();

        if (typeof(T) == typeof(HeroController))
        {
            sprite.sprite = SpriteHolder.Instance.GetRandomHeroSprite();
        }
        else if (typeof(T) == typeof(EnemyController))
        {
            sprite.sprite = SpriteHolder.Instance.GetRandomEnemySprite();
        }

        return newGameObject.GetComponent<T>();
    }

    public Vector3 GetRealPositionFromGridId(int x, int y)
    {
        float posX = (x - 1.0f * width / 2) * gridSize + gridSize / 2f;
        float posY = (y - 1.0f * height / 2) * gridSize + gridSize / 2f;
        return new Vector3(posX, posY, 0);
    }

    void Update()
    {
        float deltatime = Time.deltaTime;
        UpdateGameTime(deltatime);
        if (gameState == GAMESTATE.MOVE)
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
        if (spawnElapseTime >= spawnTime)
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

        if (gridSystem.IsBorder(nextX, nextY))
        {
            PopFrontSnake();
            if (snake.IsEmpty())
                return;
            ForceChangeSnakeDirection();
        }

        nextX = snake.GetNextX();
        nextY = snake.GetNextY();
        if (gridSystem.HasObjectOnGrid(nextX, nextY))
        {
            GridObject gridObject = gridSystem.GetObjectOnGrid(nextX, nextY);
            if (gridObject is Hero)
            {
                if (!(gridObject as Hero).IsInList())
                    snake.AddHero(gridObject as Hero);
                else
                {
                    PopFrontSnake();
                    return;
                }
            }
            else if (gridObject is Enemy)
            {
                gameState = GAMESTATE.COMBAT;
                combatEnemy = gridObject as Enemy;
                return;
            }
        }

        MoveSnakeTo(GetRealPositionFromGridId(nextX, nextY));
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
        if (combatEnemy == null)
        {
            Debug.Log("No enemy to combat");
            return;
        }

        Hero hero = snake.GetFirst();

        Attack(hero, combatEnemy);
        if (combatEnemy.IsDead())
        {
            combatEnemy = null;
            UpScore(hero.GetHeart());
            UpSpeed();
            gameState = GAMESTATE.MOVE;
            return;
        }

        Attack(combatEnemy, hero);
        if (hero.IsDead())
        {
            PopFrontSnake();
            if (snake.IsEmpty())
            {
                gameState = GAMESTATE.GAMEEND;
            }
        }
    }

    private void Attack(GameCharacter attacker, GameCharacter target)
    {
        int damage = attacker.GetSword() * attacker.GetMultiplier(target.GetCharacterType()) - target.GetShield();
        if (damage < 0)
            damage = 0;
        target.Damage(damage);
        target.GetController<GameCharacterController>().SetHPLength(target.GetHeart());

        if (target.IsDead())
        {
            if(target is Enemy)
                gridSystem.RemoveObject(target.GetX(), target.GetY());
            target.Dead();
        }
    }

    private void UpSpeed()
    {
        translateTime -= decreaseTranslateTime;

        float minimumTranslateTime = 0.1f;
        if(translateTime <= minimumTranslateTime)
        {
            translateTime = minimumTranslateTime;
        }
    }

    private void Spawn()
    {
        // Spawn Hero
        GridSystem.Point point = gridSystem.GetFreeSpace();
        if (point == null)
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

    public float GetTranslateTime()
    {
        return translateTime;
    }

    // ------------------- Score ---------------------
    private void ResetText()
    {
        score = 0;
        scoreText.text = "Score : " + score;
    }

    private void UpScore(int value)
    {
        score += value;
        scoreText.text = "Score : " + score;
    }


    // ------------------- Snake -----------------------

    private void ClearSnakeGrid()
    {
        List<Hero> heroes = snake.GetHeroes();
        foreach (Hero hero in heroes)
        {
            int x = hero.GetX();
            int y = hero.GetY();
            gridSystem.RemoveObject(x, y);
        }
    }

    private void AddSnakeGrid()
    {
        List<Hero> heroes = snake.GetHeroes();
        foreach (Hero hero in heroes)
        {
            int x = hero.GetX();
            int y = hero.GetY();
            gridSystem.AddObject(x, y, hero);
        }
    }

    public void PopFrontSnake()
    {
        ClearSnakeGrid();
        snake.PopFront();
        AddSnakeGrid();
    }

    public void MoveSnakeTo(Vector3 position)
    {
        ClearSnakeGrid();
        snake.MoveTo(position);
        AddSnakeGrid();
    }

    public void TurnLeft()
    {
        if (gameState != GAMESTATE.MOVE)
            return;

        snake.TurnLeft();

        if (IsNextIsHeroInLine() || IsNextIsBorder())
            snake.TurnRight();
    }

    public void TurnRight()
    {
        if (gameState != GAMESTATE.MOVE)
            return;

        snake.TurnRight();

        if (IsNextIsHeroInLine() || IsNextIsBorder())
            snake.TurnLeft();
    }

    public bool IsNextIsHeroInLine()
    {
        int nextX = snake.GetNextX();
        int nextY = snake.GetNextY();
        if (gridSystem.HasObjectOnGrid(nextX, nextY))
        {
            GridObject gridObject = gridSystem.GetObjectOnGrid(nextX, nextY);
            if (gridObject is Hero)
            {
                if ((gridObject as Hero).IsInList())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsNextIsBorder()
    {
        int nextX = snake.GetNextX();
        int nextY = snake.GetNextY();
        return gridSystem.IsBorder(nextX, nextY);
    }

    public void SwitchHeroUp()
    {
        ClearSnakeGrid();
        snake.FrontRotateHero();
        AddSnakeGrid();
    }

    public void SwitchHeroDown()
    {
        ClearSnakeGrid();
        snake.BackRotateHero();
        AddSnakeGrid();
    }
}
