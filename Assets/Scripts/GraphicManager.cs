using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GraphicManager : MonoBehaviour
{

    private static GraphicManager _instance;
    public static GraphicManager Instance { get { return _instance; } }

    // Use this for initialization
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public Text midText;
    public Text scoreText;
    public Image film;
    public GameObject BG;
    public GameObject BG2;

    public void Pause()
    {
        midText.text = "Press any key to Continue";
        midText.gameObject.SetActive(true);
        film.gameObject.SetActive(true);
    }

    public void UnPause()
    {
        midText.gameObject.SetActive(false);
        film.gameObject.SetActive(false);
    }

    public void GameEnd()
    {
        midText.text = "Game Over";
        midText.gameObject.SetActive(true);
        film.gameObject.SetActive(true);
    }

    public void Retry()
    {
        midText.text = "Press any key to Continue";
        midText.gameObject.SetActive(true);
        film.gameObject.SetActive(true);
    }
}
