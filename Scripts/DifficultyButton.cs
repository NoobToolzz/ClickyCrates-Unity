using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    public int difficulty;

    private Button button;
    private GameManager gameManager;

    AudioManager audioManager;

    // Find the "Game Manager" and "Audio Manager" objects and get their respective components for usage
    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    // This gets the button that was clicked and sets it's corresponding difficulty through SetDifficulty()
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
    }

    // Function to set the difficulty depending on the button that was clicked
    // Plays a sound effect and starts the game at the difficulty level
    void SetDifficulty()
    {
        //Debug.Log(gameObject.name + " was clicked");
        audioManager.PlaySFX(audioManager.buttonClick); // Play button click SFX - Points to PlaySFX() in AudioManager.cs
        gameManager.StartGame(difficulty); // Points to StartGame() inside GameManager.cs
    }
}
