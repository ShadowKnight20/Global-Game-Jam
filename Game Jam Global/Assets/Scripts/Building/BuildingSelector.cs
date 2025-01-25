using UnityEngine;

public class BuildingSelector : MonoBehaviour
{
    public BuildingUI buildingUI;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                BuildingResourceHandler building = hit.collider.GetComponent<BuildingResourceHandler>();
                if (building != null)
                {
                    buildingUI.SelectBuilding(building);
                }
            }
        }
    }
}
