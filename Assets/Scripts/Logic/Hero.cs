using UnityEngine;
using System.Collections;
using System;

public class Hero : GameCharacter {

    private bool isInList;

    public Hero(int _x, int _y, GridObjectController _controller) : base(_x, _y, _controller)
    {
        isInList = false;
    }

    public override int GetMultiplier(CHARACTER_TYPE _type)
    {
        return (_type == characterType) ? 2 : 1;
    }

    public void SetInList(bool b)
    {
        isInList = b;
    }

    public bool IsInList()
    {
        return isInList;
    }
}
