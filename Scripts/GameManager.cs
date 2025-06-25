using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    // Variables are all binded to their specific elements within the Unity interface, applies to all scripts for this game
    public bool playMusic; // This is too pass the music playing status for game started / over (to play and pause the background music)
    public bool isGameActive;
    public Button restartButton;
    public GameObject titlescreen; // Title screen contains the title text and difficulty buttons
    public List<GameObject> targets; // List of all targets' GameObject's
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI instructionsText;

    private int score;
    private Sensor sensorManager;
    private float spawnRate = 1.0f;
    public int maxMissedTargets; // To pass the number of targets that need to be missed per difficulty

    // Pass play music to true to indicate music must be playing and find the "Sensor" object and gets its component
    private void Awake()
    {
        playMusic = true;
        sensorManager = GameObject.Find("Sensor").GetComponent<Sensor>();
    }

    // Set score to 0, isGameActive to true, start spawning the targets and the updating score function
    // The spawnRate is calculated by dividing the assigned integer with the difficulties of each mode
    // The difficulties for Easy, Medium, and Hard modes are 1, 2, 3 respectively.
    public void StartGame(int difficulty)
    {
        // Set max missed targets based on selected difficulty
        // Also change the missed targets initial text to show up
        switch (difficulty)
        {
            case 1: // Easy
                maxMissedTargets = 3;
                sensorManager.targetsMissed.text = "Targets Missed: 0 / 3";
                break;
            case 2: // Medium
                maxMissedTargets = 5;
                sensorManager.targetsMissed.text = "Targets Missed: 0 / 5";
                break;
            case 3: // Hard
                maxMissedTargets = 7;
                sensorManager.targetsMissed.text = "Targets Missed: 0 / 7";
                break;
        }

        isGameActive = true; // Flag to indicate the game has begun / is in a "playing" state
        spawnRate /= difficulty; // Difficulty is passed by DifficultyButton.cs

        score = 0;
        StartCoroutine(SpawnTarget());
        UpdateScore(0);

        // I had to put instructionsText outside of all the "Title Screen" objects because otherwise it
        // would have different canvas dimensions and be on top of the game screen instead of on the bottom left
        instructionsText.gameObject.SetActive(false); titlescreen.gameObject.SetActive(false); // Disable the title screen objects (instructions, buttons, etc)
    }

    // Updates the score count each time you earn/lose points
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd; scoreText.text = "Score: " + score;
        if (score < 0) { GameOver(); } // If score goes in the negatives, it's game over
    }

    // When called, this sets the "Game Over!" text object and restart buton active so they shows up, they are disabled by default
    // Then sets isGameActive to false to indicate the game is over
    public void GameOver()
    {
        isGameActive = false; playMusic = false;
        gameOverText.gameObject.SetActive(true); restartButton.gameObject.SetActive(true);

        StartCoroutine(DestroyRemainingObjects()); // Start couroutine to invoke DestroyRemainingObjects() method again after 250ms (0.25 seconds) because there's always one object left
    }

    // This gets the current scene and reloads it to the starting state
    // Do note: The onClick() method for the restart button points to RestartGame() and is configured via the Unity Engine interface, not directly in this code
    public void RestartGame() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }

    // Quits the application, onClick() of quit button is binded to this function within the Unity Engine interface
    public void Quit() { Application.Quit(); }

    // Launches items for as long as the game is active
    IEnumerator SpawnTarget()
    {
        // Waits the appropriate amount of time (in seconds) according to the difficulty
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    // Loops through all gameObjects, filters them by the ones that contain "Good" and "Bad" in their name, then destroys those ones.
    private IEnumerator DestroyRemainingObjects()
    {
        bool objectsRemaining; // True / False

        do
        {
            objectsRemaining = false; // Reset the flag for each loop stage

            // Find all objects in the scene
            GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

            // Loop through all objects and destroy those containing "Good" or "Bad" in their name
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains("Good") || obj.name.Contains("Bad"))
                {
                    Destroy(obj); // Destroy it
                    objectsRemaining = true; // Set the flag to true if an object was destroyed
                }
            }

            // Yield return null to wait for the next frame
            yield return null;

        } while (objectsRemaining); // Continue looping until no objects are left
    }
}
