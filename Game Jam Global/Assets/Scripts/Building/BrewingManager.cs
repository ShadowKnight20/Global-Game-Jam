using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BubbleController : MonoBehaviour
{
    public float maxBubble = 100f;        // Maximum bubble amount
    public float currentBubble;          // Current bubble amount
    public float bubbleDecayRate = 1f;   // Amount of bubble reduced per second
    public float bubbleIncreaseAmount = 20f; // Amount of bubble increased on collision
    public float minBubble = 0f;         // Minimum bubble amount

    public Slider bubbleSlider;          // Reference to the UI Slider
    public Gradient gradient;
    public Image fill;
    public GameObject particleObject;    // Object with particles to show/hide

    private bool isParticleActive = false;
    private AudioSource audioSource;

    void Start()
    {   
        // Initialize current bubble to the maximum value at the start
        currentBubble = 0;
        fill.color = gradient.Evaluate(0f);
        audioSource = GetComponent<AudioSource>();

        // Ensure the slider is properly initialized
        if (bubbleSlider != null)
        {
            bubbleSlider.maxValue = maxBubble;
            bubbleSlider.value = currentBubble;
        }
        else
        {
            //Debug.LogError("Bubble Slider is not assigned in the Inspector!");
        }

        // Hide particle object initially if it exists
        if (particleObject != null)
        {
            particleObject.SetActive(false);
        }
        else
        {
            //Debug.LogError("Particle Object is not assigned in the Inspector!");
        }

        // Start the bubble decay timer
        StartCoroutine(BubbleDecayTimer());
    }

    void Update()
    {
        // Prevent currentBubble from going below the minimum value
        currentBubble = Mathf.Clamp(currentBubble, minBubble, maxBubble);

        fill.color = gradient.Evaluate(bubbleSlider.normalizedValue);

        // Update the slider value to reflect the current bubble amount
        if (bubbleSlider != null)
        {
            bubbleSlider.value = currentBubble;
        }

        // Show or hide the particle object based on the current bubble amount
        HandleParticleObject();

        // Additional logic (e.g., game over) when bubble reaches 0 can be added here
        if (currentBubble <= minBubble)
        {
            //Debug.Log("Bubble amount depleted!");
        }
    }

    // Coroutine to gradually reduce the bubble amount
    IEnumerator BubbleDecayTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            currentBubble -= bubbleDecayRate;
        }
    }

    // Trigger event when player collides with this object
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            //Debug.Log("Player hit the object! Increasing bubble amount.");
            IncreaseBubble();
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    // Increase bubble amount
    void IncreaseBubble()
    {
        currentBubble += bubbleIncreaseAmount;

        // Prevent exceeding the maximum bubble amount
        currentBubble = Mathf.Clamp(currentBubble, minBubble, maxBubble);

        // Update the slider value after increasing the bubble
        if (bubbleSlider != null)
        {
            bubbleSlider.value = currentBubble;
        }
    }

    // Show or hide the particle object based on the bubble amount
    void HandleParticleObject()
    {
        if (currentBubble >= 75f && !isParticleActive)
        {
            // Activate particle object
            if (particleObject != null)
            {
                particleObject.SetActive(true);
                isParticleActive = true;
            }
        }
        else if (currentBubble < 75f && isParticleActive)
        {
            // Deactivate particle object
            if (particleObject != null)
            {
                particleObject.SetActive(false);
                isParticleActive = false;
            }
        }
    }
}
