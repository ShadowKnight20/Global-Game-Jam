using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Ensure you include this for UI elements
using UnityEngine.SceneManagement; // For managing the game scene

public class GameManagerControler : MonoBehaviour
{
    public int playerMoney = 50;
    public float gameTime = 180f; // 3 minutes in seconds

    public Text timerText; // Assign this in the Inspector for displaying the timer
    public Text scoreText; // Assign this in the Inspector for displaying the score when the game ends

    private bool isGameOver = false;

    void Start()
    {
        // Ensure the timer text starts correctly
        UpdateTimerText();
    }

    void Update()
    {
        if (!isGameOver)
        {
            // Reduce the game time
            gameTime -= Time.deltaTime;

            // Update the timer display
            UpdateTimerText();

            // Check if the time is up
            if (gameTime <= 0)
            {
                EndGame();
            }
        }
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
    }

    void UpdateTimerText()
    {
        // Convert time to minutes and seconds
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);

        // Update the UI
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void EndGame()
    {
        isGameOver = true;
        gameTime = 0;

        // Display the score
        scoreText.text = "Game Over! Final Score: " + playerMoney;

        // Optionally, stop time in the game
        Time.timeScale = 0;

        // You can add additional actions, like restarting the game or loading a new scene, here.
    }
}
