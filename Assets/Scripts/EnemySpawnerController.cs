using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class EnemySpawnerController : MonoBehaviour {

    // This script is controlled by an invisible object on the map called Spawner Point
    // It will create spawners, 3 at a time, at 3 different locations contained in the spawnPoints coordinate list
    // The spawners will exist for a random amount of time between 7 and 20 seconds
    // The spawner direction will be corrected using the corresponding item in the spawnRotation list

    // !!For spawner movement code, please see the SpawnerMovementController!!

    // List of the spawn points that the spawner enemies can appear at. They are scattered throughout the 3 floors
    // These points had to be hardcoded as it would have been difficult to have the spawner appear in truly random places
    // but not land on other walls or other obects
    List<Vector3> spawnPoints = new List<Vector3>();

    // List to hold the rotation values for the spawn points (so the spawner will be facing the correct direction upon initial spawn)
    // The index here corresponds to the same index in the spawnPoints list
    List<int> spawnRotation = new List<int>();

    // Used to randomly pick the amount of seconds the spawners will live when created
    // Have to use the full class name for Random since unity has its own Random class.
    System.Random rnd;

    // Used to pick a random number for the spawn location
    int randomSpawnLocation;

    // The spawner object - set in the unity editor
    public GameObject spawnerObject;

    // Used for the respawn audio that occurs with each spawn
    AudioSource audio;

	// Use this for initialization
	void Start () {
        // Initialize AudioSource component
        audio = GetComponent<AudioSource>();

		// Initialize the spawn point list
        // 1st Floor
        spawnPoints.Add(new Vector3(-17.62f, 2.43f, 0.948f));
        spawnPoints.Add(new Vector3(-25f, 2.43f, 0.59f));
        // 2nd Floor
        spawnPoints.Add(new Vector3(-1.695f, 0.041f, 2.929f));
        spawnPoints.Add(new Vector3(2.844f, 0.041f, 2.929f));
        spawnPoints.Add(new Vector3(1.55f, 0.041f, -3.15f));
        spawnPoints.Add(new Vector3(-3.21f, 0.041f, -3.15f));
        //3rd Floor
        spawnPoints.Add(new Vector3(-2.556f, -1.117f, -13.37f));
        spawnPoints.Add(new Vector3(-7.231f, -1.117f, -13.37f));
        spawnPoints.Add(new Vector3(-11.022f, -1.117f, -16.996f));
        spawnPoints.Add(new Vector3(-11.022f, -1.117f, -22.93f));
        spawnPoints.Add(new Vector3(-7.49f, -1.117f, -28.44f));
        spawnPoints.Add(new Vector3(-5.75f, -1.117f, -18.16f));
        spawnPoints.Add(new Vector3(-4.08f, -1.117f, -28.97f));
        spawnPoints.Add(new Vector3(-1.88f, -1.117f, -23.86f));
        spawnPoints.Add(new Vector3(-1.88f, -1.117f, -26.63f));
        spawnPoints.Add(new Vector3(-1.306f, -1.117f, -29.445f));
        spawnPoints.Add(new Vector3(-0.72f, -1.117f, -25.14f));
        spawnPoints.Add(new Vector3(4.38f, -1.117f, -23.95f));
        spawnPoints.Add(new Vector3(4.38f, -1.117f, -14.58f));
        spawnPoints.Add(new Vector3(1.8f, -1.117f, -12.92f));
            
        // Initialize the spawn rotation list
        // 1st Floor
        spawnRotation.Add(180);
        spawnRotation.Add(180);
        // 2nd Floor
        spawnRotation.Add(270);
        spawnRotation.Add(270);
        spawnRotation.Add(270);
        spawnRotation.Add(270);
        //3rd Floor
        spawnRotation.Add(90);
        spawnRotation.Add(90);
        spawnRotation.Add(0);
        spawnRotation.Add(0);
        spawnRotation.Add(0);
        spawnRotation.Add(0);
        spawnRotation.Add(0);
        spawnRotation.Add(0);
        spawnRotation.Add(0);
        spawnRotation.Add(0);
        spawnRotation.Add(0);
        spawnRotation.Add(0);
        spawnRotation.Add(0);
        spawnRotation.Add(0);

        // Initialize our random
        rnd = new System.Random();


        // Begin spawning - this starts spawning after 2 seconds and respawns with whatever value it recieves from rnd.Next - somewhere between
        // 7 and 20 seconds
        InvokeRepeating("SpawnEnemies", 2, rnd.Next(7, 20));

	}        

    // This method is used to spawn the enemies on the maps. It goes through various checks to make sure 2 enemies
    // aren't spawning in the same location
    void SpawnEnemies() {
        // Destroy any existing spawners on the map
        //Find the existing spawners
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("Enemy Spawner");

        if (objectsToDestroy.Length == 0)
        {
            //Do nothing - there is nothing to destroy
        }
        else
        {
            // Go through the array of spawners and destroy them
            foreach (GameObject destroyThis in objectsToDestroy)
            {
                Destroy(destroyThis);
            }
        }
            

        // Variables used to store the locations we have used so we can avoid using the same location twice
        int usedLocationOne = 0;
        int usedLocationTwo = 0;
        int usedLocationThree = 0;

        // Flag used to stop the while loop if we run into a conflicting spawn location
        bool isValid = false;

        // Loop through the spawn process 3 times since we want 3 enemies
        for (int i = 0; i < 3; i++)
        {
            // Grab a new random coordinate
            randomSpawnLocation = rnd.Next(0, 19);

            // Check to make sure we haven't used to coordinate yet. If we have, pick another random number and test it again.
            // Once we have an unused number, we assign it to a usedLocation variable and instantiate the gameobject
            // in the chosen coordinate with its corresponding rotation.

            // This is where the magic happens
            // We begin with a switch statement that tests how many times we have looped through our for loop using the for loops i variable.
            // It will pick the correct case based on which loop - the first loop we will just instantiate the first
            // enemy and assign it's location to the first used location. The second loop we will check to make sure our
            // new random location does NOT equal the first location. If it does, we use a while loop to create a new
            // random number and check it again until we have a unique number that hasn't been used. We continue this for 
            // the last case, but we instead test both of the spawn locations until we find a unique 3rd location for another enemy
            switch (i)
            {
                case 0:
                    usedLocationOne = randomSpawnLocation;
                    Instantiate(spawnerObject, spawnPoints[usedLocationOne], Quaternion.Euler(new Vector3(0, spawnRotation[usedLocationOne])));
                    break;
                case 1:
                    do {
                        if (randomSpawnLocation == usedLocationOne)
                        {
                            randomSpawnLocation = rnd.Next(0, 19);
                            isValid = false;
                        }
                        else {
                            usedLocationTwo = randomSpawnLocation;
                            Instantiate(spawnerObject, spawnPoints[usedLocationTwo], Quaternion.Euler(new Vector3(0, spawnRotation[usedLocationTwo])));
                            isValid = true;
                        }
                    } while (!isValid);
                    break;
                case 2:
                    do {
                        if (randomSpawnLocation == usedLocationOne || randomSpawnLocation == usedLocationTwo)
                        {
                            randomSpawnLocation = rnd.Next(0, 19);
                            isValid = false;
                        }
                        else {
                            usedLocationThree = randomSpawnLocation;
                            Instantiate(spawnerObject, spawnPoints[usedLocationThree], Quaternion.Euler(new Vector3(0, spawnRotation[usedLocationThree])));
                            isValid = true;
                        }
                    } while (!isValid);
                    break;
            }
        }

        // Play the Audio associated with spawner spawns so the player knows a new spawn has occurred
        // I took this audio from the Space Shooter tutorial. No Regrets!
        audio.Play();
    }

}
