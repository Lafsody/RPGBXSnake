using UnityEngine;
using System.Collections;

public class GameCharacter {

    public enum CHARACTER_TYPE
    {
        RED = 0,
        GREEN = 1,
        BLUE = 2
    }

    protected int x;
    protected int y;

    protected int maxHeart;
    protected int heart;
    protected int sword;
    protected int shield;
    protected CHARACTER_TYPE characterType;

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
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
