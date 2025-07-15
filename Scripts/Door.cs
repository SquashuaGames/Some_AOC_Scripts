using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string currentScene;
    public string doorScene;
    public string playerTag = "Player";
    private bool inRange = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals(playerTag))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inRange = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown("e") && inRange)
        {
            
            SceneManager.LoadScene(doorScene);
            Scene nextScene = SceneManager.GetSceneAt(SceneManager.loadedSceneCount-1);
            SceneManager.SetActiveScene(nextScene);
        }
    }
}
