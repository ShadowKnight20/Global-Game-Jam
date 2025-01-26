using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Text element to display the player's money.")]
    public Text moneyText;

    [Header("Game Manager Reference")]
    [Tooltip("Reference to the GameManagerController.")]
    public GameManagerControler gameManager;

    private void Start()
    {
    }

    private void Update()
    {
        // Update the money display with the value from GameManagerController
        if (gameManager != null && moneyText != null)
        {
            moneyText.text = "Pebbles: " + gameManager.playerMoney.ToString();
        }
    }
}
