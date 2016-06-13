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
    protected CHARACTER_TYPE characterType;

    public GameCharacter(int _x, int _y) : base (_x, _y)
    {
        InitialStatus();
    }

    protected void InitialStatus()
    {
        maxHeart = heart = Random.Range(1, 20);
        sword = Random.Range(1, 10);
        shield = Random.Range(1, 10);
        characterType = (CHARACTER_TYPE) Random.Range(0, 2);
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
}
