using UnityEngine;
using System.Collections;

public abstract class GridObject {

    protected int x;
    protected int y;
    protected GridObjectController controller;

    public GridObject(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public T GetController<T>() where T : GridObjectController
    {
        if(!controller is T)
        {
            Debug.Log("Not match type");
            return null;
        }

        return (T)controller;
    }
    
    public void SetAllPosition(int _x, int _y, Vector3 _position, Vector3 _targetPosition, int setId)
    {
        if ((setId & 1) != 0)
        {
            SetX(_x);
            SetY(_y);
        }

        if ((setId & 2) != 0)
            controller.SetPosition(_position);

        if( (setId & 4) != 0)
            controller.SetTargetPosition(_targetPosition);
    }

    public void SetX(int _x)
    {
        x = _x;
    }

    public void SetY(int _y)
    {
        y = _y;
    }
}
