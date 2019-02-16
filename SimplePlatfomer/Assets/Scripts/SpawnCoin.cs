using System.Collections;
using UnityEngine;

public class SpawnCoin : MonoBehaviour
{
    public GameObject coin;
    public GameObject coinPrefab;

    int spawnTimer;
    int spawnTime = 200;

    void Start() {
        spawnTimer = spawnTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (coin == null) {
            if (spawnTimer <= 0) {
                Spawn();
                spawnTimer = spawnTime;
            }
            spawnTimer--;
        }
    }

    void Spawn() {
        GameObject _coin = Instantiate(coinPrefab, gameObject.transform.position, gameObject.transform.rotation);
        coin = _coin;
    }
}
