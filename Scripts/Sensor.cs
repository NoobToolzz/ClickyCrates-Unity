using TMPro;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public TextMeshProUGUI targetsMissed; // Set in the Unity Engine interface

    private GameManager gameManager;
    private int hitObjectCount = 0; // Counter for all objects hitting the sensor

    // Find the "Game Manager" object and get its component
    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // GameManager.cs reference
    }

    // This is to destroy the items you miss when they hit a "Sensor" object below the game to free up space
    // However, when any object (except the skull) hits the sensor 3 times, the game is over
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is NOT tagged as "Bad"
        if (!other.CompareTag("Bad"))
        {
            if (gameManager.isGameActive) // Only occurs if game is still active
            {
                hitObjectCount++; // Increment the counter for any object hitting the sensor
                targetsMissed.text = "Targets Missed: " + hitObjectCount + " / " + gameManager.maxMissedTargets; // Update text to reflect count
            }

            // When any object has hit the sensor the maximum number of times, trigger game over
            if (hitObjectCount >= gameManager.maxMissedTargets)
            {
                gameManager.GameOver();
            }
        }

        // Destroy the object that hit the sensor
        Destroy(other.gameObject);
    }
}
