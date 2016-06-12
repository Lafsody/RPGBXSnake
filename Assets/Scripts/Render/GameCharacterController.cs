using UnityEngine;
using System.Collections;

public class GameCharacterController : MonoBehaviour {

    protected float targetX;
    protected float targetY;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MoveTo(float x, float y)
    {
        targetX = x;
        targetY = y;
    }
}
