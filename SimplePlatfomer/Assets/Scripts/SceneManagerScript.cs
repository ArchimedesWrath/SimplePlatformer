using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance = null;
    public GameObject Player;

    void Start() {
        if (instance == null) 
            instance = this;
        else if (instance != this) 
            Destroy(gameObject);

        if (Player == null) Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void RequestNextScene(string nextScene) {
        if (Application.CanStreamedLevelBeLoaded(nextScene)) {
            // Load the scene
            DontDestroyOnLoad(Player);
            DontDestroyOnLoad(this);
            SceneManager.LoadScene(nextScene);
        } else {
            // Throw an error here
            Debug.Log("Cannot find scene: " + nextScene);
        }
    }
}
