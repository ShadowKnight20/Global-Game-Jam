using UnityEngine;
public enum ResourceType
{
    Wood,
    Stone,
    Food
}
public class BuildingResourceHandler : MonoBehaviour
{
    public string buildingName = "Building";
    public ResourceType resourceType = ResourceType.Wood;

    public int MaxEmployees = 5;
    private int currentEmployees = 0;

    public float productionRate = 5f; // Resource production per game hour per employee
    private float productionTimer = 0f;
    public float gameHourDuration = 10f; // Game hour duration in seconds

    private ResourceManager resourceManager;

    public int CurrentEmployees => currentEmployees; // Public getter for current employees

    void Start()
    {
        resourceManager = ResourceManager.Instance; // Use the singleton instance of ResourceManager
    }

    void Update()
    {
        // Produce resources only if there are employees
        if (currentEmployees > 0)
        {
            productionTimer += Time.deltaTime;

            // Check if a game hour has passed
            if (productionTimer >= gameHourDuration)
            {
                ProduceResources();
                productionTimer = 0f;
            }
        }
    }

    public void EmployPerson()
    {
        if (resourceManager.unemployed > 0 && currentEmployees < MaxEmployees)
        {
            currentEmployees++;
            resourceManager.employed++;
            resourceManager.unemployed--;
        }
        else
        {
            Debug.Log($"{buildingName}: No available workers or max capacity reached.");
        }
    }

    public void FirePerson()
    {
        if (currentEmployees > 0)
        {
            currentEmployees--;
            resourceManager.employed--;
            resourceManager.unemployed++;
        }
        else
        {
            Debug.Log($"{buildingName}: No employees to fire.");
        }
    }

    private void ProduceResources()
    {
        int resourcesProduced = Mathf.RoundToInt(productionRate * currentEmployees);

        switch (resourceType)
        {
            case ResourceType.Wood:
                resourceManager.wood += resourcesProduced;
                break;
            case ResourceType.Stone:
                resourceManager.stone += resourcesProduced;
                break;
            case ResourceType.Food:
                resourceManager.food += resourcesProduced;
                break;
        }

        Debug.Log($"{buildingName}: Produced {resourcesProduced} {resourceType}.");
    }
}
