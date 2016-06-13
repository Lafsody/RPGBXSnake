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
        HeroController controller = CreateController<HeroController>(x, y);

        Hero hero = new Hero(x, y, controller);
        snake.AddHero(hero);
    }

    public T CreateController<T>(int x, int y) where T : GridObjectController
    {
        GameObject gameObject = Instantiate(SpriteHolder.Instance.heroPrefabs, GetRealPositionFromGridId(x, y), Quaternion.identity) as GameObject;
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.sprite = SpriteHolder.Instance.GetRandomHeroSprite();

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
        elapsedTime += Time.deltaTime;
        if (gameState == GAMESTATE.MOVE)
        {
            if (elapsedTime >= translateTime)
            {
                elapsedTime = 0;
                CheckMove();

                if(snake.IsEmpty())
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

    private void CheckMove()
    {
        int nextX = snake.GetNextX();
        int nextY = snake.GetNextY();

        //Debug.Log(nextX + ", " + nextY);

        if(gridSystem.IsBorder(nextX, nextY))
        {
            snake.PopFront();
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
            }
            else if(gridObject is Enemy)
            {
                gameState = GAMESTATE.COMBAT;
            }
        }
        else
        {
            snake.MoveTo(GetRealPositionFromGridId(nextX, nextY));
        }
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
}
