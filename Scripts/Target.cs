using System.Runtime.CompilerServices;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private GameManager gameManager; // Referencing GameManager.cs script
    private float minSpeed = 8;
    private float maxSpeed = 13;
    private float maxTorque = 2;
    private float xRange = 4;
    private float ySpawnPos = -2;

    AudioManager audioManager;

    public int pointValue;
    public ParticleSystem explosionParticle;

    // Find the "Game Manager" and "Audio Manager" objects and get their respective components for usage
    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // GameManager.cs reference
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    void Start()
    {
        // Get the Rigidbody off of the items, those are the targets
        targetRb = GetComponent<Rigidbody>();

        // Generate random properties
        var (force, torque, spawnPos) = GenerateRandomProperties();

        // Apply force and torque and set spawn position
        targetRb.AddForce(force, ForceMode.Impulse);
        targetRb.AddTorque(torque, torque, torque, ForceMode.Impulse);

        transform.position = spawnPos;
    }

    // This code is to only destroy the item when it is hit within the hitbox for as long as the game is active
    // If I just added the GetMouseButton() to destroy it, it would've just destroyed it when I am clicking on the background
    private void Update()
    {
        if (gameManager.isGameActive)
        {
            // Check if the left mouse button is clicked
            if (Input.GetMouseButtonDown(0))
            {
                // So apparently raycasts are a thing, and this creates a ray from wherever the camera is facing to the mouse position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Perform the raycast
                if (Physics.Raycast(ray, out hit))
                {
                    // Check if the raycast hit the item (within hitbox of target), then destroy
                    // The UpdateScore() function is within GameManager.cs and referenced here
                    // When you successfuly hit a target, it adds the point value of the item to the score and plays a sound effect
                    // Targets explode with their own uniquely-coloured explosion effect
                    if (hit.collider.gameObject == gameObject)
                    {
                        //Debug.Log("Object destroyed");
                        Destroy(gameObject);
                        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation); // Explode from targets position
                        audioManager.PlaySFX(audioManager.explosion); // Play pop sound effect
                        gameManager.UpdateScore(pointValue);
                    }
                }
            }
        }
    }

    // Generate random force, torque, and spawn position for targets
    // This is more of a minimalist code snippet of the commented RandomForce(), RandomTorque(), and RandomSpawnPos() functions below
    private (Vector3 force, float torque, Vector3 spawnPos) GenerateRandomProperties()
    {
        Vector3 force = Vector3.up * Random.Range(minSpeed, maxSpeed);
        float torque = Random.Range(-maxTorque, maxTorque);
        Vector3 spawnPos = new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
        return (force, torque, spawnPos);
    }

    /* 
    * This new method does NOT work.
    private void OnMouseDown()
    {
        Debug.Log("Mouse clicked on object");
        Destroy(gameObject);
    }
    
    * Random upward force to project items at
    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    * Random rotation and speed for items
    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    * Random spawn position so you have to move around the mouse insted of being on the same spot the whole time
    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }
    */
}
