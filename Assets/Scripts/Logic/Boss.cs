using UnityEngine;
using System.Collections;

public class Boss : Enemy {

    public Boss(int _x, int _y, GridObjectController _controller) : base(_x, _y, _controller)
    {
    }

    public Boss(int _x, int _y, GridObjectController _controller, int _level) : base(_x, _y, _controller, _level)
    {
    }

    protected override void InitialStatus(int level)
    {
        switch (level)
        {
            case 1:
                maxHeart = heart = Random.Range(20, 30);
                sword = Random.Range(4, 6);
                shield = Random.Range(1, 2);
                break;
            case 2:
                maxHeart = heart = Random.Range(25, 35);
                sword = Random.Range(6, 8);
                shield = Random.Range(1, 3);
                break;
            case 3:
                maxHeart = heart = Random.Range(30, 40);
                sword = Random.Range(10, 14);
                shield = Random.Range(2, 3);
                break;
            case 4:
                maxHeart = heart = Random.Range(40, 50);
                sword = Random.Range(12, 20);
                shield = Random.Range(3, 4);
                break;
            default:
                maxHeart = heart = Random.Range(50, 70);
                sword = Random.Range(12, 20);
                shield = Random.Range(3, 5);
                break;
        }
        characterType = (CHARACTER_TYPE)Random.Range(0, 3);

        alive = true;
    }
}
