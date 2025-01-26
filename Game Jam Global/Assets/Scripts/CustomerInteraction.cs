using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteraction : MonoBehaviour
{
    [Header("Customer Settings")]
    public GameObject customerPrefab;             // Prefab of the customer
    public Transform customerSpawnPoint;          // Where the customer spawns at the table
    public float customerWaitTime = 30f;          // Max time a customer will wait before leaving
    public float tableCooldownTime = 5f;          // Time the table stays empty after a customer leaves

    [Header("Food Settings")]
    public List<string> foodOptions;             // List of possible food demands
    public string currentFoodDemand;             // The current food demand for the customer
    public List<GameObject> foodPrefabs;         // List of food prefabs corresponding to the food options

    [Header("Payment Settings")]
    public int paymentAmount = 50;               // Payment received when food is delivered
    public int paymentPenalty = -10;             // Payment penalty if food is not delivered
    public GameManagerControler gameManager;     // Reference to the game manager for managing money

    private GameObject currentCustomer;          // Reference to the spawned customer
    private GameObject foodAboveCustomer;        // Reference to the food object above the customer
    private bool isTableOccupied = false;        // Flag to check if the table is occupied

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Cache the GameManagerControler reference
        gameManager = FindObjectOfType<GameManagerControler>();
        if (gameManager == null)
        {
            Debug.LogError("GameManagerControler is not found in the scene!");
        }

        SpawnCustomer(); // Spawn the first customer at the start
    }

    void Update()
    {
        // Continuously check if the customer has been served and the table is free for a new customer
        if (!isTableOccupied && currentCustomer == null)
        {
            SpawnCustomer();
        }
    }

    private void SpawnCustomer()
    {
        if (isTableOccupied) return; // Avoid spawning if table is already occupied

        // Spawn the customer
        currentCustomer = Instantiate(customerPrefab, customerSpawnPoint.position, Quaternion.identity);
        currentCustomer.transform.SetParent(transform); // Attach the customer to the table

        // Assign a random food demand
        currentFoodDemand = foodOptions[Random.Range(0, foodOptions.Count)];
        Debug.Log($"A customer has spawned and wants: {currentFoodDemand}");

        // Start the timer for the customer to wait
        StartCoroutine(CustomerWaitTimer());
        isTableOccupied = true;
    }

    private IEnumerator CustomerWaitTimer()
    {
        // Wait for the customer to be served or timeout
        float elapsed = 0f;
        while (elapsed < customerWaitTime)
        {
            if (currentCustomer == null) // Stop the timer if the customer despawns early
                yield break;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // If the customer hasn't been served in time, they leave
        if (currentCustomer != null)
        {
            Debug.Log("Customer left due to impatience!");
            gameManager.AddMoney(paymentPenalty);
            Destroy(currentCustomer);
            StartCoroutine(TableCooldown());
        }
    }

    public void DeliverFood(string deliveredFood)
    {
        // Remove "(Clone)" from the delivered food name if it exists
        if (deliveredFood.Contains("(Clone)"))
        {
            deliveredFood = deliveredFood.Replace("(Clone)", "").Trim();
        }

        if (!isTableOccupied || currentCustomer == null)
        {
            Debug.Log("No customer to serve at this table!");
            return;
        }

        // If the food delivered matches the customer's demand
        if (deliveredFood == currentFoodDemand)
        {
            Debug.Log("Customer is happy! Payment received.");

            // Display the food above the customer's head based on the current demand
            DisplayFoodAboveCustomer();

            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }

            // Add money to the game manager
            if (gameManager != null)
            {
                gameManager.AddMoney(paymentAmount);
            }
            else
            {
                Debug.LogError("GameManagerControler reference is missing!");
            }

            Destroy(currentCustomer);           // Despawn the customer
            Destroy(foodAboveCustomer);         // Destroy the food item above the customer
            StartCoroutine(TableCooldown());    // Start the cooldown timer
        }
        else
        {
            Debug.Log("Customer did not want this food!");
        }
    }

    private void DisplayFoodAboveCustomer()
    {
        // Find the index of the current food demand in the food options list
        int foodIndex = foodOptions.IndexOf(currentFoodDemand);
        if (foodIndex >= 0 && foodIndex < foodPrefabs.Count)
        {
            // Instantiate the corresponding food prefab at the customer's foodDemandSpawnPoint
            if (currentCustomer != null)
            {
                // Check if the customer has a foodDemandSpawnPoint and position the food there
                Transform foodSpawnPoint = currentCustomer.transform.Find("FoodDemandSpawnPoint");
                if (foodSpawnPoint != null)
                {
                    foodAboveCustomer = Instantiate(foodPrefabs[foodIndex], foodSpawnPoint.position, Quaternion.identity);
                    foodAboveCustomer.transform.SetParent(currentCustomer.transform);  // Make it follow the customer
                }
                else
                {
                    Debug.LogError("No 'FoodDemandSpawnPoint' found on the customer.");
                }
            }
        }
        else
        {
            Debug.LogError("Food prefab not found for the current food demand.");
        }
    }

    private IEnumerator TableCooldown()
    {
        isTableOccupied = true; // Mark the table as occupied during cooldown
        Debug.Log($"Table is empty. Waiting for {tableCooldownTime} seconds before the next customer.");

        yield return new WaitForSeconds(tableCooldownTime); // Wait for the cooldown time

        isTableOccupied = false; // Free the table after cooldown
        Debug.Log("Table is now ready for the next customer.");
    }
}
