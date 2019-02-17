using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnScript : MonoBehaviour
{
    public GameObject Player;
    public PlayerController PlayerScript;

    void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
        Player.transform.position = transform.position;

        PlayerScript = Player.GetComponent<PlayerController>();

        if (PlayerScript.currentSpawn == null) {
            PlayerScript.currentSpawn = gameObject;
        }
    }
}
