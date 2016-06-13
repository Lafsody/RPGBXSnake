using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteHolder : MonoBehaviour {

    private static SpriteHolder _instance;
    public static SpriteHolder Instance { get { return _instance; } }

    public GameObject heroPrefabs;

    public Sprite[] heroes;
    public Sprite[] enemies;

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    public GameObject GetHeroPrefab()
    {
        return heroPrefabs;
    }

    public Sprite GetRandomHeroSprite()
    {
        int r = Random.Range(0, heroes.Length - 1);
        return heroes[r];
    }

    public Sprite GetRandomEnemySprite()
    {
        int r = Random.Range(0, enemies.Length - 1);
        return enemies[r];
    }
}
