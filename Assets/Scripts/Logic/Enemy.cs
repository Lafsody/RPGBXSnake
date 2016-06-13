using UnityEngine;
using System.Collections;
using System;

public class Enemy : GameCharacter {
    public Enemy(int _x, int _y, GridObjectController _controller) : base(_x, _y, _controller)
    {

    }

    public override int GetMultiplier(CHARACTER_TYPE _type)
    {
        return 1;
    }
}
