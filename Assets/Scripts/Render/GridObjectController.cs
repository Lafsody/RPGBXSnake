using UnityEngine;
using System.Collections;

public class GridObjectController : MonoBehaviour {

    protected Vector3 targetPosition;
	
    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }

    public void SetPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    public void MoveTo(float x, float y)
    {
        SetTargetPosition(new Vector3(x, 0, y));
    }

}
