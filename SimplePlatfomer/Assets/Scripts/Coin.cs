using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject CoinSprite;
    public GameObject PickUpParticle;
    public bool canRespawn = true;
    GameObject childCoin;
    int spawnTimer;
    int SPAWNTIME = 200;
    
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
        if (canRespawn) {
            Destroy(childCoin);
            GameObject effectIns = (GameObject)Instantiate(PickUpParticle, transform.position, transform.rotation);
            Destroy(effectIns, 2f);
        } else {
            Destroy(gameObject);
            GameObject effectIns = (GameObject)Instantiate(PickUpParticle, transform.position, transform.rotation);
            Destroy(effectIns, 2f);
        }
        
    }

    void SpawnCoin() {
        childCoin = Instantiate(CoinSprite, transform.position, transform.rotation) as GameObject;
        childCoin.transform.SetParent(transform);
    }
}
