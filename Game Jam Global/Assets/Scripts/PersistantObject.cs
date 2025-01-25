using UnityEngine;

public class PersistantObject : MonoBehaviour
{
    private void Awake()
    {
        // Check if another instance of this object already exists
        //if (FindObjectsOfType(GetType()).Length > 1)
        //{
        //    Destroy(gameObject); // Prevent duplicates
        //    return;
        //}

        // Make this object persist across scene loads
        DontDestroyOnLoad(gameObject);
    }
}