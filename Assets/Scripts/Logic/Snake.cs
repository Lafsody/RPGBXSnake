﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Snake {
    public enum DIRECTION
    {
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3
    }

    private List<Hero> heroes;
    private DIRECTION direction;

    public Snake()
    {
        heroes = new List<Hero>();
        direction = DIRECTION.RIGHT;
    }

    public void AddHero(Hero hero)
    {
        heroes.Add(hero);
    }

    public void PopFront()
    {
        Hero hero = heroes[0];
        FrontRotateHero();
        heroes.Remove(hero);
        // TODO hero.clearStatus or remove or whathever
    }

    public Hero GetFirst()
    {
        if (IsEmpty())
        {
            Debug.Log("No hero left in list");
            return null;
        }
        return heroes[0];
    }

    public void FrontRotateHero()
    {
        if (IsEmpty())
        {
            Debug.Log("No hero left in list");
            return;
        }

        FrontShiftPosition();
    }

    public void BackRotateHero()
    {
        if (IsEmpty())
        {
            Debug.Log("No hero left in list");
            return;
        }

        BackShiftPosition();
    }

    private void FrontShiftPosition()
    {
        Hero firstHero = heroes[0];

        int x = firstHero.GetX();
        int y = firstHero.GetY();
        HeroController firstHeroCtrl = firstHero.GetController<HeroController>();
        Vector3 positionCtrl = firstHeroCtrl.GetPosition();
        Vector3 targetPositionCtrl = firstHeroCtrl.GetTargetPosition();

        bool first = true;
        //Swap Position
        foreach (Hero hero in heroes)
        {
            if (first)
            {
                first = false;
                continue;
            }

            int tempX = hero.GetX();
            int tempY = hero.GetY();
            HeroController heroCtrl = hero.GetController<HeroController>();
            Vector3 tempPositionCtrl = heroCtrl.GetPosition();
            Vector3 tempTargetPositionCtrl = heroCtrl.GetTargetPosition();

            hero.SetAllPosition(x, y, positionCtrl, targetPositionCtrl);

            x = tempX;
            y = tempY;
            positionCtrl = tempPositionCtrl;
            targetPositionCtrl = tempTargetPositionCtrl;
        }
        firstHero.SetAllPosition(x, y, positionCtrl, targetPositionCtrl);
        
        heroes.Remove(heroes[0]);
        heroes.Add(firstHero);
    }
    
    private void BackShiftPosition()
    {
        heroes.Reverse();
        FrontShiftPosition();
        heroes.Reverse();
    }

    public bool IsEmpty()
    {
        return heroes.Count == 0;
    }
}
