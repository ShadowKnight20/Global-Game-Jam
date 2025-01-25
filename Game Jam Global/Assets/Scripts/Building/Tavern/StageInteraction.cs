using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageInteraction : MonoBehaviour
{
    public GameObject stageMenuCanvas; // The canvas for the stage menu
    public KeyCode interactKey = KeyCode.F; // The key to open the menu
    public Transform player;
    public float interactionDistance; // Maximum distance for interaction

    private bool isPlayerNear = false; // Tracks if the player is near the stage
    private bool isMenuActive = false; // Tracks if the menu is currently active

    Vector3 newRotationEulerAngles = new Vector3(0, -140, 0); // For example, (0, 90, 0)
    public Quaternion newRotation;

    void Start()
    {
        newRotation = Quaternion.Euler(newRotationEulerAngles);

        // Ensure the stage menu is initially hidden
        if (stageMenuCanvas != null)
        {
            stageMenuCanvas.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Stage Menu Canvas is not assigned.");
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(interactKey))
        {
            ToggleStageMenu();
        }
    }

    private void ToggleStageMenu()
    {
        if (stageMenuCanvas != null)
        {
            isMenuActive = !isMenuActive;
            stageMenuCanvas.SetActive(isMenuActive);
            CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
            // Lock or unlock the player's movement and cursor

            //Cursor.lockState = isMenuActive ? CursorLockMode.None : CursorLockMode.Locked;

            if (isMenuActive)
            {
                cameraMovement.offset = new Vector3(2, 1f, -6f);
                player.GetComponent<PlayerMovementWithAutoRotation>().gameObject.transform.rotation = newRotation;
            }
            else if (!isMenuActive)
            {
                cameraMovement.offset = new Vector3(0, 10f, -5f);
            }

            //Cursor.visible = isMenuActive;

            // Optionally, you can implement logic here to enable/disable player movement
            player.GetComponent<PlayerMovementWithAutoRotation>().enabled = !isMenuActive;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            // Ensure the menu is closed when the player leaves the stage
            if (isMenuActive)
            {
                ToggleStageMenu();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the interaction range in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}