using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject CurrentSpawnedEnemy;
    public GameObject Item;
    public bool SpawnEnemyOnce = false;
    public bool AttachItemOnce = false;
    bool hasSpawnedAnEnemy = false;
    int SpawnCounter;
    int SpawnTime = 200;

    void Start() {
        if (Enemy) {
            SpawnEnemy();
            if (!hasSpawnedAnEnemy) hasSpawnedAnEnemy = true;
        }

        SpawnCounter = SpawnTime;
    }

    void FixedUpdate() {
        if (CurrentSpawnedEnemy == null && !SpawnEnemyOnce) {
            SpawnCounter--;
            if (SpawnCounter <= 0) {
                SpawnEnemy();
                SpawnCounter = SpawnTime;
            }
        }
    }

    void SpawnEnemy() {
        GameObject enemy = (GameObject)Instantiate(Enemy, transform.position, transform.rotation);
        if (Item) {
            if (AttachItemOnce) {
                if (!hasSpawnedAnEnemy) {
                    enemy.GetComponent<EnemyController>().Item = Item;
                }
            } else {
                enemy.GetComponent<EnemyController>().Item = Item;
            }
        }
        CurrentSpawnedEnemy = enemy;
    }
}
