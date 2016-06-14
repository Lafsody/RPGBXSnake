using UnityEngine;
using System.Collections;

public abstract class GridObjectController : MonoBehaviour {

    protected Vector3 currentStartPosition;
    protected Vector3 targetPosition;
    protected float elapsedTime;
    protected float timeToReach;

    void Awake()
    {
        elapsedTime = 0;
        targetPosition = transform.position;
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
        elapsedTime = 0;
        targetPosition = position;
        currentStartPosition = transform.position;
        timeToReach = GameManager.Instance.GetTranslateTime();
    }

    public void MoveTo(float x, float y)
    {
        SetTargetPosition(new Vector3(x, y, 0));
    }

    void FixedUpdate()
    {
        Vector3 currentPosition = gameObject.transform.position;
        if(MathFunction.GetDistanceSquare(currentPosition, targetPosition) < 1e-6)
        {
            elapsedTime = 0;
            gameObject.transform.position = targetPosition;
        }
        else
        {
            elapsedTime += Time.deltaTime / timeToReach;
            gameObject.transform.position = Vector3.Lerp(currentStartPosition, targetPosition, elapsedTime);
        }
    }

    public void Dead()
    {
        gameObject.SetActive(false);
    }
}
