using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLocked : MonoBehaviour
{
    public bool playerInDoor = false;
    public GameObject UnlockedDoor;
    public GameObject LinkedDoor;
     
    void SpawnDoor() {
        GameObject _door = Instantiate(UnlockedDoor, transform.position, transform.rotation);
        _door.GetComponent<Door>().SetLinkedDoor(LinkedDoor);
        Destroy(gameObject);
    } 

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name == "Player") {
            playerInDoor = true;
            if (col.gameObject.GetComponent<PlayerController>().hasKey) {
                col.gameObject.GetComponent<PlayerController>().UseKey();
                SpawnDoor();
            }
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name == "Player") {
            playerInDoor = false;
        }
    }
}
