using UnityEngine;
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

    public void MoveTo(Vector3 nextPosition)
    {
        Hero firstHero = heroes[0];

        ShiftPosition(5);

        firstHero.SetAllPosition(GetNextX(), GetNextY(), nextPosition, nextPosition, 5);
    }

    public int GetFirstX()
    {
        return GetFirst().GetX();
    }

    public int GetFirstY()
    {
        return GetFirst().GetY();
    }

    public int GetNextX()
    {
        int[] plusValue = { 0, 1, 0, -1 };
        return GetFirstX() + plusValue[(int)direction];
    }

    public int GetNextY()
    {
        int[] plusValue = { 1, 0, -1, 0 };
        return GetFirstY() + plusValue[(int)direction];
    }

    public void TurnLeft()
    {
        direction = (DIRECTION)(((int)direction + 3) % 4);
    }

    public void TurnRight()
    {
        direction = (DIRECTION)(((int)direction + 1) % 4);
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

        int setId = 7;
        ShiftPosition(setId);

        firstHero.SetAllPosition(x, y, positionCtrl, targetPositionCtrl, setId);
        
        heroes.Remove(heroes[0]);
        heroes.Add(firstHero);
    }
    
    private void BackShiftPosition()
    {
        heroes.Reverse();
        FrontShiftPosition();
        heroes.Reverse();
    }

    private void ShiftPosition(int typeId)
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

            if (typeId == 7)
            {
                hero.SetAllPosition(x, y, positionCtrl, targetPositionCtrl, 7);
            }
            else if(typeId == 5)
            {
                hero.SetAllPosition(x, y, tempPositionCtrl, positionCtrl, 5);
            }

            x = tempX;
            y = tempY;
            positionCtrl = tempPositionCtrl;
            targetPositionCtrl = tempTargetPositionCtrl;
        }
    }

    public bool IsEmpty()
    {
        return heroes.Count == 0;
    }
}
