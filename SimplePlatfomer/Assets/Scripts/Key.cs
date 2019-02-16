using UnityEngine;

public class Key : MonoBehaviour
{
    public bool isPickedUp = false;
    float speed = 8f;
    public Transform destination = null;
    

    void FixedUpdate() {
        if (isPickedUp && destination != null) {
            Follow();
        }
    }

    public void PickUp(Transform transform) {
        destination = transform;
        isPickedUp = true;
    }

    public void Use() {
        Destroy(gameObject);
    }

    void Follow() {
        transform.position = Vector3.MoveTowards(transform.position, destination.position, speed * Time.deltaTime);
    }
}
