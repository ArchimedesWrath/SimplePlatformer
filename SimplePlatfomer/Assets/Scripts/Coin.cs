using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject CoinSprite;
    public GameObject PickUpParticle;
    GameObject childCoin;
    int spawnTimer;
    int SPAWNTIME = 200;
    bool canRespawn = true;
    void Start() {
        childCoin = gameObject.transform.GetChild(0).gameObject;
        spawnTimer = SPAWNTIME;
    }

    void FixedUpdate() {
        if (childCoin == null && canRespawn) {
            spawnTimer--;
            if (spawnTimer <= 0) {
                SpawnCoin();
                spawnTimer = SPAWNTIME;
            }
        }
    }

    public void PickUpCoin() {
        Destroy(childCoin);
        Instantiate(PickUpParticle, transform.position, transform.rotation);
    }

    void SpawnCoin() {
        childCoin = Instantiate(CoinSprite, transform.position, transform.rotation) as GameObject;
        childCoin.transform.SetParent(transform);
    }
}
