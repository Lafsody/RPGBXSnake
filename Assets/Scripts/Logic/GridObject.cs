using UnityEngine;
using System.Collections;

public class GridObject {

    protected int x;
    protected int y;
    protected GridObjectController controller;

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

    public void SetAllPosition(int _x, int _y, Vector3 _position, Vector3 _targetPosition)
    {
        SetX(_x);
        SetY(_y);

        controller.SetPosition(_position);
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
