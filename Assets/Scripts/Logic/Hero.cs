using UnityEngine;
using System.Collections;

public class Hero : GameCharacter {
    public Hero(int _x, int _y, HeroController heroCtrl) : base(_x, _y)
    {
        // Find World Position
        controller = heroCtrl;
    }
}
