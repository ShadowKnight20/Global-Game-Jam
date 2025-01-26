using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementWithAutoRotation : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;           // Movement speed
    public float rotationSpeed = 10f;     // Speed of smooth rotation

    [Header("Jump Settings")]
    public float jumpForce = 5f;          // Force applied when jumping
    public string groundTag = "Ground";   // Tag for ground objects

    private Rigidbody rb;
    public bool isGrounded;

    [Header("Sprite Settings")]
    public Transform spriteTransform;     // The child object holding the 2D sprite

    [Header("Carry Settings")]
    public GameObject objectToCarry;       // Prefab of the object to spawn
    public Transform carryPosition;        // Position above the player's head to carry the object
    private GameObject carriedObject;      // Reference to the carried object

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.E; // Key to deliver food

    Animator animator;
    int isWalkingHash;
    int isJumpingHash;

    void Start()
    {
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isJumpingHash = Animator.StringToHash("isJumping");

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody is missing on the player object!");
        }

        if (spriteTransform == null)
        {
            Debug.LogError("Sprite Transform is not assigned! Assign the 2D sprite object in the Inspector.");
        }

        if (carryPosition == null)
        {
            Debug.LogError("Carry Position is not assigned! Assign a Transform in the Inspector.");
        }
    }

    void Update()
    {
        // Handle movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude > 0.1f) // Ensure input is significant
        {
            // Normalize direction to prevent faster diagonal movement
            direction.Normalize();

            // Smoothly rotate the player to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the player in the desired direction
            Vector3 move = direction * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + move);

            animator.SetBool(isWalkingHash, true);

            // Correct the sprite's rotation to avoid flipping
            FixSpriteRotation(direction);
        }
        else
        {
            animator.SetBool(isWalkingHash, false);
        }

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Handle spawning and carrying an object
        if (Input.GetMouseButtonDown(1)) // Right mouse click
        {
            AttemptToSpawnCarryObject();
        }

        // Handle food delivery interaction
        if (Input.GetKeyDown(interactKey))
        {
            AttemptToDeliverFood();
        }
    }

    private void FixSpriteRotation(Vector3 direction)
    {
        if (spriteTransform != null)
        {
            // Flip the sprite along the X-axis depending on the movement direction
            if (direction.x > 0)
            {
                spriteTransform.localScale = new Vector3(2, 2, 2); // Facing right
            }
            else if (direction.x < 0)
            {
                spriteTransform.localScale = new Vector3(-2, 2, 2); // Facing left
            }

            // Keep the sprite upright by resetting its rotation
            spriteTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the ground tag
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the exited collision object has the ground tag
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = false;
        }
    }

    private void AttemptToSpawnCarryObject()
    {
        // Check if an object is already being carried
        if (carriedObject != null)
        {
            Debug.Log("Already carrying an object.");
            return;
        }

        // Check for nearby objects with a BubbleController and ensure their currentBubble >= 75
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, 5f); // Detect objects within a radius of 5
        foreach (Collider nearbyObject in nearbyObjects)
        {
            BubbleController bubbleController = nearbyObject.GetComponent<BubbleController>();
            if (bubbleController != null && bubbleController.currentBubble >= 75f)
            {
                // Spawn the object above the player's head
                carriedObject = Instantiate(objectToCarry, carryPosition.position, Quaternion.identity);
                carriedObject.transform.parent = carryPosition; // Attach the object to the carry position
                Debug.Log("Spawned and carrying the object.");
                return;
            }
        }

        Debug.Log("No suitable object found with enough bubbles to interact.");
    }

    private void AttemptToDeliverFood()
    {
        if (carriedObject == null)
        {
            Debug.Log("You are not carrying any food to deliver.");
            return;
        }

        // Check for nearby tables with CustomerInteraction
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, 5f);
        foreach (Collider nearbyObject in nearbyObjects)
        {
            CustomerInteraction customer = nearbyObject.GetComponent<CustomerInteraction>();
            if (customer != null)
            {

                // Deliver the food
                customer.DeliverFood(carriedObject.name);

                // Remove the carried object
                Destroy(carriedObject);
                carriedObject = null;
                Debug.Log("Delivered food to the customer!");
                return;
            }
        }

        Debug.Log("No customer nearby to deliver food.");
    }
}
