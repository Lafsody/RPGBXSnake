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

    private GridSystem gridSystem;
    private Snake snake;

    private GAMESTATE gameState;

    private float elapsedTime;
    private float elapsedBossTime;
    public float translateTime;
    public float decreaseTranslateTime;
    public float decreasrBossTime;
    public float combatTime;
    public float bossTime;

    private float startTranslateTime;
    private float startBossTime;

    private float spawnElapseTime;
    public float spawnTime;

    private Enemy combatEnemy;

    private int score;
    private int level;

    private bool isBossAppear;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        gridSystem = new GridSystem(width, height);

        gameState = GAMESTATE.PAUSE;

        startTranslateTime = translateTime;
        startBossTime = bossTime;

        ResetTime();

        score = 0;
        level = 1;
        isBossAppear = false;

        combatEnemy = null;
        
        snake = new Snake();
    }

    void Start()
    {
        int spriteScale = 200;

        GraphicManager.Instance.BG.transform.localScale = new Vector3(width * gridSize * spriteScale, height * gridSize * spriteScale, 5);
        GraphicManager.Instance.BG2.transform.localScale = new Vector3(width * gridSize * spriteScale + spriteScale * gridSize, height * gridSize * spriteScale + spriteScale * gridSize, 6);

        InitiateSnake();
        Spawn();
        Pause();
    }

    private void ResetGame()
    {
        isBossAppear = false;
        ResetScore();
        ResetLevel();
        ResetTime();

        snake.Reset();

        gridSystem.ClearGrids();

        InitiateSnake();
        Spawn();
    }

    private void ResetTime()
    {
        elapsedTime = 0;
        elapsedBossTime = 0;
        spawnElapseTime = 0;


        translateTime = startTranslateTime;
        bossTime = startBossTime;
    }

    private void InitiateSnake()
    {
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
        Enemy newEnemy = new Enemy(x, y, controller, level);

        controller.SetAura(newEnemy.GetCharacterType());
        controller.SetHPLength(newEnemy.GetHeart());
        return newEnemy;
    }

    private Boss CreateBoss(int x, int y)
    {
        BossController controller = CreateController<BossController>(x, y);
        Boss newBoss = new Boss(x, y, controller, level);

        controller.SetAura(newBoss.GetCharacterType());
        controller.SetHPLength(newBoss.GetHeart());
        return newBoss;
    }

    public T CreateController<T>(int x, int y) where T : GridObjectController
    {
        GameObject prefab = null;
        if (typeof(T) == typeof(HeroController))
        {
            prefab = SpriteHolder.Instance.heroPrefab;
        }
        else if (typeof(T) == typeof(BossController))
        {
            prefab = SpriteHolder.Instance.bossPrefab;
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
        else if (typeof(T) == typeof(BossController))
        {
            sprite.sprite = SpriteHolder.Instance.GetRandomBossSprite();
        }
        else if (typeof(T) == typeof(EnemyController))
        {
            sprite.sprite = SpriteHolder.Instance.GetRandomEnemySprite(level);
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
        {
            UpdateSpawnTime(deltatime);
            if(!isBossAppear)
            {
                UpdateBossTime(deltatime);
            }
        }
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
                    GameEnd();
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
                    GameEnd();
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

    private void UpdateBossTime(float deltaTime)
    {
        elapsedBossTime += deltaTime;
        if (elapsedBossTime >= bossTime)
        {
            elapsedBossTime = 0;
            BossAppear();
        }
    }

    private void CheckMove()
    {
        int nextX = snake.GetNextX();
        int nextY = snake.GetNextY();

        //Debug.Log(nextX + ", " + nextY);

        if (gridSystem.IsBorder(nextX, nextY))
        {
            HitWall();
            if (snake.IsEmpty())
                return;
        }

        nextX = snake.GetNextX();
        nextY = snake.GetNextY();
        if (gridSystem.HasObjectOnGrid(nextX, nextY))
        {
            GridObject gridObject = gridSystem.GetObjectOnGrid(nextX, nextY);
            if (gridObject is Hero)
            {
                if (!(gridObject as Hero).IsInList())
                {
                    JoinParty(gridObject as Hero);
                }
                else
                {
                    HitSnake();
                    return;
                }
            }
            else if (gridObject is Enemy)
            {
                gameState = GAMESTATE.COMBAT;
                combatEnemy = gridObject as Enemy;
                if(combatEnemy is Boss)
                {
                    AudioManager.Instance.StopBGM();
                    AudioManager.Instance.PlayBossFightBGM();
                }

                return;
            }
        }

        MoveSnakeTo(GetRealPositionFromGridId(nextX, nextY));
    }

    private void HitWall()
    {
        AudioManager.Instance.PlayWallHitSound();
        PopFrontSnake();
        if (snake.IsEmpty())
            return;
        ForceChangeSnakeDirection();
    }

    private void HitSnake()
    {
        AudioManager.Instance.PlayWallHitSound();
        PopFrontSnake();
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

    private void JoinParty(Hero hero)
    {
        AudioManager.Instance.PlayJoinPartySound();
        snake.AddHero(hero);
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
            if(combatEnemy is Boss)
            {
                AudioManager.Instance.StopBGM();
                AudioManager.Instance.PlayNormalBGM();
                AudioManager.Instance.PlayWinBossSound();
                UpScore(100 * level);
                UpLevel();
                isBossAppear = false;
                elapsedBossTime = 0;
                UpSpeed();
                UpScore(combatEnemy.GetLevel() * 100);
            }
            else
            {
                UpScore(combatEnemy.GetLevel() * 20);
            }
            combatEnemy = null;
            gameState = GAMESTATE.MOVE;
            return;
        }

        Attack(combatEnemy, hero);
        if (hero.IsDead())
        {
            PopFrontSnake();
            if (snake.IsEmpty())
            {

                if (combatEnemy is Boss)
                {
                    AudioManager.Instance.StopBGM();
                    AudioManager.Instance.PlayNormalBGM();
                }
                GameEnd();
            }
        }
    }

    private void Attack(GameCharacter attacker, GameCharacter target)
    {
        AudioManager.Instance.PlayHitSound();
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

        if(level > 4)
        {
            bossTime -= decreasrBossTime;
            float minimumBossTime = 15f;
            if(bossTime < minimumBossTime)
            {
                bossTime = minimumBossTime;
            }
        }
    }

    private void Spawn()
    {
        // Spawn Hero
        GridSystem.Point point = gridSystem.GetFreeSpace();
        if (point == null)
        {
            return;
        }

        Hero newHero = CreateHero(point.x, point.y);
        gridSystem.AddObject(point.x, point.y, newHero);

        // Spawn Enemy
        GridSystem.Point point1 = gridSystem.GetFreeSpace();
        if (point1 == null)
        {
            return;
        }

        Enemy newEnemy = CreateEnemy(point1.x, point1.y);
        gridSystem.AddObject(point1.x, point1.y, newEnemy);
    }

    public void BossAppear()
    {
        AudioManager.Instance.PlayBossAppearSound();
        isBossAppear = true;
        // Spawn Boss
        GridSystem.Point point1 = gridSystem.GetFreeSpace();
        if (point1 == null)
        {
            return;
        }

        Boss newBoss = CreateBoss(point1.x, point1.y);
        gridSystem.AddObject(point1.x, point1.y, newBoss);
    }

    public float GetTranslateTime()
    {
        return translateTime;
    }

    // ------------------- Game State -----------------

    public void AnyKeyDown()
    {
        if(gameState == GAMESTATE.PAUSE)
        {
            Continue();
        }
        else if (gameState == GAMESTATE.GAMEEND)
        {
            Retry();
        }
    }

    public void Pause()
    {
        gameState = GAMESTATE.PAUSE;
        GraphicManager.Instance.Pause();
    }

    public void Continue()
    {
        gameState = GAMESTATE.MOVE;
        GraphicManager.Instance.UnPause();
    }

    public void GameEnd()
    {
        gameState = GAMESTATE.GAMEEND;
        GraphicManager.Instance.GameEnd();
    }

    public void Retry()
    {
        gameState = GAMESTATE.PAUSE;
        GraphicManager.Instance.Retry();

        // TODO Reset Game
        ResetGame();
    }

    // ------------------- Score ---------------------
    private void ResetScore()
    {
        score = 0;
        GraphicManager.Instance.SetScore(score);
    }

    private void UpScore(int value)
    {
        score += value;
        GraphicManager.Instance.SetScore(score);
    }

    // ------------------ Level ----------------------
    private void ResetLevel()
    {
        level = 1;
        GraphicManager.Instance.SetLevel(level);
    }

    private void UpLevel()
    {
        level += 1;
        GraphicManager.Instance.SetLevel(level);
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
