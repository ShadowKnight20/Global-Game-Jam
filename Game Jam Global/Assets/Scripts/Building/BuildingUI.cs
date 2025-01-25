using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    public Button employButton;
    public Button fireButton;
    public Text buildingNameText;
    public Text employeeCountText;

    private BuildingResourceHandler selectedBuilding;

    void Start()
    {
        // Add listeners for the buttons
        employButton.onClick.AddListener(OnEmployButtonClicked);
        fireButton.onClick.AddListener(OnFireButtonClicked);

        // Disable buttons initially
        employButton.interactable = false;
        fireButton.interactable = false;
    }

    public void SelectBuilding(BuildingResourceHandler building)
    {
        selectedBuilding = building;
        buildingNameText.text = building.buildingName;
        UpdateEmployeeCount();

        // Enable buttons when a building is selected
        employButton.interactable = true;
        fireButton.interactable = true;
    }

    private void UpdateEmployeeCount()
    {
        if (selectedBuilding != null)
        {
            employeeCountText.text = $" {selectedBuilding.CurrentEmployees}/{selectedBuilding.MaxEmployees}";
        }
    }

    private void OnEmployButtonClicked()
    {
        if (selectedBuilding != null)
        {
            selectedBuilding.EmployPerson();
            UpdateEmployeeCount();
        }
    }

    private void OnFireButtonClicked()
    {
        if (selectedBuilding != null)
        {
            selectedBuilding.FirePerson();
            UpdateEmployeeCount();
        }
    }
}
