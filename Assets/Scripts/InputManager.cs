using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

	void Update()
    {
        if(Input.GetKeyDown("up"))
        {
            GameManager.Instance.SwitchHeroUp();
        }
        if(Input.GetKeyDown("down"))
        {
            GameManager.Instance.SwitchHeroDown();
        }
        if(Input.GetKeyDown("left"))
        {
            GameManager.Instance.TurnLeft();
        }
        if(Input.GetKeyDown("right"))
        {
            GameManager.Instance.TurnRight();
        }
    }
}
