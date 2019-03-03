using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject CoinSprite;
    public GameObject PickUpParticle;
    public bool canRespawn = true;
    public bool isStatic = false;
    public Transform groundCheck;
    public LayerMask GroundLayer;
    GameObject childCoin;
    int spawnTimer;
    int SPAWNTIME = 200;
    bool grounded = false;
    
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

        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, GroundLayer);

        if (!grounded && !isStatic) {
            Vector2 pos = transform.position;
            pos.y -= 5f * Time.deltaTime;
            transform.position = pos;
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
