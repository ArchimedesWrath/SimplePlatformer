using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instnace = null;
    public GameObject Player;
    PlayerController PlayerController;
    void Start()
    {
        if (instnace == null)
            instnace = this;
        else if (instnace != this)
            Destroy(gameObject);

        if (Player == null) Player = GameObject.FindGameObjectWithTag("Player");
        
        if (PlayerController == null) PlayerController = Player.GetComponent<PlayerController>();
    }

    public void RequestDoor(GameObject door) {
        Door DoorScript = door.GetComponent<Door>();
        if (PlayerController.isInDoor && PlayerController.GetDoor() != null) {
            if (DoorScript.nextSceneDoor) {
                // Reqeust next scene from SceneManager
                Debug.Log("Requesting to change to scene: " + DoorScript.nextScene);
                SceneManagerScript.instance.RequestNextScene(DoorScript.nextScene);
            } else {
                PlayerController.Move(DoorScript.LinkedDoor.transform);
            }
        }
    }
}
