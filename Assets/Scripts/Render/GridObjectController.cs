using UnityEngine;
using System.Collections;

public class GridObjectController : MonoBehaviour {

    protected float targetX;
    protected float targetY;
	
    public void MoveTo(float x, float y)
    {
        targetX = x;
        targetY = y;
    }
}
