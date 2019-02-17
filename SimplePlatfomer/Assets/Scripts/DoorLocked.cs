using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLocked : MonoBehaviour
{
    public bool playerInDoor = false;
    public GameObject UnlockedDoor;
    public GameObject LinkedDoor;
    public bool nextSceneDoor = false;
    public string nextScene;
     
    void SpawnDoor() {
        GameObject _door = Instantiate(UnlockedDoor, transform.position, transform.rotation);
        Door DoorScript = _door.GetComponent<Door>();
        if (LinkedDoor != null) {
           DoorScript.SetLinkedDoor(LinkedDoor);
            LinkedDoor.GetComponent<Door>().SetLinkedDoor(_door);
        }

        if (nextSceneDoor) {
            DoorScript.nextSceneDoor = true;
            DoorScript.nextScene = nextScene;
        }
        
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
