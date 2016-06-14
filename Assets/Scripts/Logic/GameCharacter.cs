using UnityEngine;
using System.Collections;

public abstract class GameCharacter : GridObject{

    public enum CHARACTER_TYPE
    {
        RED = 0,
        GREEN = 1,
        BLUE = 2
    }

    protected int maxHeart;
    protected int heart;
    protected int sword;
    protected int shield;
    protected bool alive;
    protected CHARACTER_TYPE characterType;

    public GameCharacter(int _x, int _y, GridObjectController _controller) : base (_x, _y, _controller)
    {
        InitialStatus(1);
    }

    public GameCharacter(int _x, int _y, GridObjectController _controller, int level) : base(_x, _y, _controller)
    {
        InitialStatus(level);
    }

    protected virtual void InitialStatus(int level)
    {
        switch(level)
        {
            case 1:
                maxHeart = heart = Random.Range(6, 8);
                sword = Random.Range(2, 4);
                shield = Random.Range(0, 1);
                break;
            case 2:
                maxHeart = heart = Random.Range(8, 12);
                sword = Random.Range(4, 6);
                shield = Random.Range(0, 2);
                break;
            case 3:
                maxHeart = heart = Random.Range(10, 15);
                sword = Random.Range(7, 10);
                shield = Random.Range(1, 3);
                break;
            case 4:
            default:
                maxHeart = heart = Random.Range(15, 20);
                sword = Random.Range(8, 12);
                shield = Random.Range(2, 4);
                break;
        }
        characterType = (CHARACTER_TYPE)Random.Range(0, 3);

        alive = true;
    }

    public int GetHeart()
    {
        return heart;
    }

    public int GetSword()
    {
        return sword;
    }

    public int GetShield()
    {
        return shield;
    }

    public CHARACTER_TYPE GetCharacterType()
    {
        return characterType;
    }

    public abstract int GetMultiplier(CHARACTER_TYPE _type);

    public void Damage(int damage)
    {
        heart -= damage;
        if(heart < 0)
        {
            heart = 0;
            alive = false;
        }
    }

    public bool IsDead()
    {
        return !alive;
    }

    public void Dead()
    {
        // Bring back to pool
        alive = false;
        controller.Dead();
    }
}
