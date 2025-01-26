using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControler : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isJumpingHash;
    int isHittingHash;

    public PlayerMovementWithAutoRotation playerMovementScript;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing!");
        }

        // Cache Animator parameter hashes
        isWalkingHash = Animator.StringToHash("isWalking");
        isJumpingHash = Animator.StringToHash("isJumping");
        isHittingHash = Animator.StringToHash("isHitting");

        // Find and cache the PlayerMovementWithAutoRotation script reference
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerMovementScript = player.GetComponent<PlayerMovementWithAutoRotation>();
            if (playerMovementScript == null)
            {
                Debug.LogError("PlayerMovementWithAutoRotation script is missing on the Player object!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' is not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null || playerMovementScript == null)
        {
            return; // Avoid NullReferenceExceptions if setup is incorrect
        }

        // Get player movement inputs
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Check if the player is moving
        bool isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        // Check for walking
        animator.SetBool(isWalkingHash, isMoving);

        // Check for jumping
        if (Input.GetKeyDown(KeyCode.Space) && playerMovementScript.isGrounded)
        {
            animator.SetBool(isJumpingHash, true);
        }
        else
        {
            // Reset jumping state when not jumping
            animator.SetBool(isJumpingHash, false);
        }

        // Check for mouse click to trigger hitting animation
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            animator.SetBool(isHittingHash, true);
            StartCoroutine(ResetHittingAnimation());
        }
    }

    // Coroutine to reset the hitting animation
    IEnumerator ResetHittingAnimation()
    {
        yield return new WaitForSeconds(0.1f); // Adjust duration to match your animation
        animator.SetBool(isHittingHash, false);
    }
}
