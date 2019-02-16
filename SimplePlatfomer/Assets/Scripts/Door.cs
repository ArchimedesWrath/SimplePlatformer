using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject LinkedDoor;
    public bool playerInDoor;

    public void SetLinkedDoor(GameObject door) {
        LinkedDoor = door;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name == "Player") {
            playerInDoor = true;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name == "Player") {
            playerInDoor = false;
        }
    }
}
