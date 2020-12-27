using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour {
    //This script controls the forward motion of the tank shots
    //Get the rigidbody component of the shot so that we can control it
    Rigidbody rigidbody;

    //Used to adjust the speed of the shot - set in Unity
    public float speed;

    private string player;

    // Explosion prefab for the enemy - set in Unity
    public GameObject enemyExplosion;

    void Start () {
        //Intialize Rigidbody
        rigidbody = GetComponent<Rigidbody>();

        //Move the shot forward!
        rigidbody.velocity = transform.forward * speed;

        //This is a failsafe - it will destroy the shot 5 seconds after it is loaded. This is to keep shot objects from piling up in memory
        //in the event that they don't hit an enemy or anything
        Destroy(this.gameObject, 5);
    }

    // Here we will control encounters with enemies. If we hit an enemy, we will destroy it and increment the score of 
    // player that hit it. We determine the player based on what this object name is (Player 1 or Player 2 shot)
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy")) 
        {
            //Create an explosion!
            Instantiate(enemyExplosion, other.transform.position, other.transform.rotation);

            //Destroy the enemy game object
            Destroy(other.gameObject);

            //Figure out which player made the shot and call the appropriate method to update the score
            if (this.gameObject.CompareTag("Player One Shot"))
            {
                Scores.IncrementPlayerOneEnemy();
            }
            if (this.gameObject.CompareTag("Player Two Shot"))
            {
                Scores.IncrementPlayerTwoEnemy();
            }

            //Destroy our shot so it doesn't sail off into the horizon
            Destroy(this.gameObject);
        }

        // Here we will control encounters with spawner enemies. If we hit an enemy, we will destroy it and increment the score of 
        // player that hit it. We determine the player based on what this object name is (Player 1 or Player 2 shot)
        if (other.gameObject.CompareTag("Enemy Spawner"))
        {
            //Create an explosion!
            Instantiate(enemyExplosion, other.transform.position, other.transform.rotation);

            //Destroy the enemy game object
            Destroy(other.gameObject);

            //Figure out which player made the shot and call the appropriate method to update the score
            if (this.gameObject.CompareTag("Player One Shot"))
            {
                Scores.IncrementPlayerOneSpawner();
            }
            if (this.gameObject.CompareTag("Player Two Shot"))
            {
                Scores.IncrementPlayerTwoSpawner();
            }

            //Destroy our shot so it doesn't sail off into the horizon
            Destroy(this.gameObject);
        }

        // Here we will control encounters with the Boss. It takes 10 TOTAL (between both players) to kill the boss.
        // Like most games, the player who makes the killing shot gets the points (10).
        // We will keep track of the amount of shots that have been made in the Scores class, since that is our only
        // global static area to keep track of things (and it makes sense anyway ... it is sort of a shot score)
        // The shot count does NOT reset upon player dead (since there are 2 players and, lets be real, this alien isn't going
        // to heal that fast).
        if (other.gameObject.CompareTag("Enemy Boss"))
        {
            int shotSoFar = Scores.BossShotCounter();

            // Check to see if we have hit the boss 10 times using the shot counter in the Scores class
            if (shotSoFar >= 10)
            {
                // Yay, we have finally killed the boss!
                //Create an explosion!
                Instantiate(enemyExplosion, other.transform.position, other.transform.rotation);

                //Destroy the boss game object
                Destroy(other.gameObject);

                //Figure out which player made the final shot and call the appropriate method to update the score
                if (this.gameObject.CompareTag("Player One Shot"))
                {
                    Scores.IncrementPlayerOneBoss();
                }
                if (this.gameObject.CompareTag("Player Two Shot"))
                {
                    Scores.IncrementPlayerTwoBoss();
                }

            }
            else
            {
                // Increment the shot counter
                Scores.IncrementBossShotCounter();
            }

            //Destroy our shot so it doesn't sail off into the horizon
            Destroy(this.gameObject);
        }

        //Destroy the shot if it hits a wall or one of the players
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Boss Z Wall") || other.gameObject.name == "Player 1" || other.gameObject.name == "Player 2" || other.gameObject.name == "Finish Trophy")
        {
            Destroy(this.gameObject);
        }

    }
}
