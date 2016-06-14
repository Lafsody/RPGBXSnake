using UnityEngine;
using System.Collections;

public class Enemy : GameCharacter{
    protected int level;

    public Enemy(int _x, int _y, GridObjectController _controller) : base(_x, _y, _controller)
    {
        level = 1;
    }

    public Enemy(int _x, int _y, GridObjectController _controller, int _level) : base(_x, _y, _controller,_level)
    {
        level = _level;
    }

    public int GetLevel()
    {
        return level;
    }

    public override int GetMultiplier(CHARACTER_TYPE _type)
    {
        return 1;
    }

    protected override void InitialStatus(int level)
    {
        switch (level)
        {
            case 1:
                maxHeart = heart = Random.Range(4, 6);
                sword = Random.Range(1, 3);
                shield = Random.Range(0, 1);
                break;
            case 2:
                maxHeart = heart = Random.Range(6, 8);
                sword = Random.Range(2, 4);
                shield = Random.Range(0, 1);
                break;
            case 3:
                maxHeart = heart = Random.Range(8, 10);
                sword = Random.Range(4, 6);
                shield = Random.Range(1, 2);
                break;
            case 4:
            default:
                maxHeart = heart = Random.Range(12, 15);
                sword = Random.Range(6, 8);
                shield = Random.Range(2, 4);
                break;
        }
        characterType = (CHARACTER_TYPE)Random.Range(0, 3);

        alive = true;
    }
}
