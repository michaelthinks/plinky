using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMovementController : MonoBehaviour {

    //This script is used to control the movement of spawner enemy objects. They have pretty simply logic. They move back and forth between wall objects (objects that are tagged
    //"wall"). When they collide with a wall, the direction of the enemy object reverses. Collision with the player is controlled by the PlayerController script. They are also
    // constantly rotating on the y axis, which makes them move all over the place instead of just back and forth.

    //public variable used to adjust the speed of the enemy (used to make the enemies move at different speeds - is set in the unity editor)
    public float speed;

    //flag used to control which direction the enemy is moving. It can only move in 2 directions, backwards and forwards, so a boolean serves as an appropriate flag
    private bool direction = true;


    // we will put the movement code for the enemies in FixedUpdate
    // NOTE: if this is placed in update(), the enemies will continue to move when pausing the game using TimeScale.time
    void FixedUpdate()
    {
        //the if else statement tests to see whether the direction is true or false. if it is true, it moves the enemy object in one direction
        //if it is false, it moves the enemy in the reverse direction. The boolean value is flipped every time an enemy collides with a wall
        //the boolean flag is flipped in the OnCollisionEnter method below
        if (direction)
        { 
            Vector3 movement = new Vector3((0.01f * speed), 0, 0);
            transform.Translate(movement);
        }
        else if (!direction)
        {
            Vector3 movement = new Vector3((-0.01f * speed), 0, 0);
            transform.Translate(movement);
        }
    }

    void Update()
    {
        //Make the spawners rotate while moving back and forth! It makes their movement much more random!
        //However, make sure they do not rotate on the first floor since there isn't a lot of room and they just kind of twitch around and eventually get stuck

        //Check to make sure the spawner isn't on the first floor (spawners on the first floor spawn at the y coordinate 2.43). If they aren't, begin rotation
        if (this.transform.position.y != 2.43f)
        {
            transform.Rotate(new Vector3(0, 40, 0) * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        //when the enemies hits a wall, flip the direction boolean so that the FixedUpdate method will switch directions
        if (other.gameObject.CompareTag("Wall"))
        {
            //this performs the boolean flip. it's pretty simple. If it's true, change it to false. If it's false, change it to true
            if (direction)
            {
                direction = false;
            }
            else if (!direction)
            {
                direction = true;
            }
        }
    }
}
