using UnityEngine;
using System.Collections;

public class GameCharacter : GridObject{

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
