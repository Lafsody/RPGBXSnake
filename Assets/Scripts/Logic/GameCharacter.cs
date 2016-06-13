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
        InitialStatus();
    }

    protected void InitialStatus()
    {
        maxHeart = heart = Random.Range(1, 20);
        sword = Random.Range(6, 10);
        shield = Random.Range(1, 5);
        characterType = (CHARACTER_TYPE) Random.Range(0, 2);

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
        controller.Dead();
    }
}
