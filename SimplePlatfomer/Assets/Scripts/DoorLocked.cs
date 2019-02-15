using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLocked : MonoBehaviour
{
    public bool playerInDoor = false;
    public GameObject UnlockedDoor;
     // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name == "Player") {
            playerInDoor = true;
            if (col.gameObject.GetComponent<PlayerController>().hasKey) {
                col.gameObject.GetComponent<PlayerController>().UseKey();
                Instantiate(UnlockedDoor, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name == "Player") {
            playerInDoor = false;
        }
    }
}
