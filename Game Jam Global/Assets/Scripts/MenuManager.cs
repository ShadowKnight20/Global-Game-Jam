using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    // Function to load a new scene
    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Function to quit the game
    public void QuitGame()
    {
#if UNITY_EDITOR
            // If running in the Unity editor, stop play mode
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running in a built application, quit the game
        Application.Quit();
#endif
    }
}