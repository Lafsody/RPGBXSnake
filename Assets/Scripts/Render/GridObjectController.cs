using UnityEngine;
using System.Collections;

public class GridObjectController : MonoBehaviour {

    protected Vector3 targetPosition;
    protected float elapsedTime;

    void Awake()
    {
        elapsedTime = 0;
    }

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

    void Update()
    {
        Vector3 currentPosition = gameObject.transform.position;
        if(MathFunction.GetDistanceSquare(currentPosition, targetPosition) < 1e-3)
        {
            elapsedTime = 0;
            gameObject.transform.position = targetPosition;
        }
        else
        {
            elapsedTime += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, elapsedTime);
        }
    }
}
