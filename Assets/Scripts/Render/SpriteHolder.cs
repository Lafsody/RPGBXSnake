using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteHolder : MonoBehaviour {

    private static SpriteHolder _instance;
    public static SpriteHolder Instance { get { return _instance; } }

    public GameObject heroPrefab;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;

    public Sprite[] heroes;
    public Sprite[] enemies;
    public Sprite[] enemiesLv2;
    public Sprite[] enemiesLv3;
    public Sprite[] enemiesLv4;
    public Sprite[] enemiesBoss;

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    public GameObject GetHeroPrefab()
    {
        return heroPrefab;
    }

    public Sprite GetRandomHeroSprite()
    {
        int r = Random.Range(0, heroes.Length);
        return heroes[r];
    }

    public Sprite GetRandomEnemySprite(int level)
    {
        int r;
        switch(level)
        {
            case 1:
                r = Random.Range(0, enemies.Length);
                return enemies[r];
            case 2:
                r = Random.Range(0, enemiesLv2.Length);
                return enemiesLv2[r];
            case 3:
                r = Random.Range(0, enemiesLv3.Length);
                return enemiesLv3[r];
            case 4:
                r = Random.Range(0, enemiesLv4.Length);
                return enemiesLv4[r];
            default:
                r = Random.Range(0, enemiesLv4.Length);
                return enemiesLv4[r];
        }
    }

    public Sprite GetRandomBossSprite()
    {
        int r = Random.Range(0, enemiesBoss.Length);
        return enemiesBoss[r];
    }
}
