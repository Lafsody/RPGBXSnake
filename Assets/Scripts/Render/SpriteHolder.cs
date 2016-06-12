using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteHolder : MonoBehaviour {
    public Sprite[] heroes;
    public Sprite[] enemies;

    public Sprite GetRandomHero()
    {
        int r = Random.Range(0, heroes.Length - 1);
        return heroes[r];
    }

    public Sprite GetRandomEnemy()
    {
        int r = Random.Range(0, enemies.Length - 1);
        return enemies[r];
    }
}
