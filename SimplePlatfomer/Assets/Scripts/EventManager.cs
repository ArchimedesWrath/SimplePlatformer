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
        if (PlayerController.isInDoor && PlayerController.GetDoor() != null) {
            PlayerController.Move(door.GetComponent<Door>().LinkedDoor.transform);
        }
    }
}
