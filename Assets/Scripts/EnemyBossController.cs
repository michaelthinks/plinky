using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossController : MonoBehaviour {


    // This script is used to control the Boss object. The Boss is pretty simple, like the normal enemies. He just moves back and forth, but
    // does so on 2 axes. The fun part about the boss is that it takes 10 shots to kill him! Boss shot/death is controlled by the ShotController.

    // Wublic variable used to adjust the speed of the boss (used to make the boss move at different speeds - is set in the unity editor)
    public float speed;

    // Wlag used to control which direction the boss is moving. It can backwards and forwards on 2 axes, so these booleans serve as appropriate flags
    // to determine when the object should reverse direction
    private bool xDirection = true;
    private bool zDirection = true;

    // We will put the movement code for the enemies in FixedUpdate
    // NOTE: if this is placed in update(), the enemies will continue to move when pausing the game using TimeScale.time
    void FixedUpdate()
    {
        // The if else statement tests to see whether the direction is true or false. If it is true, it moves the Boss object in one direction.
        // If it is false, it moves the Boss in the reverse direction. This applies to both the z and x axes. The boolean value is flipped every time an Boss collides with a normal wall (xDirection)
        // or a Z axis wall (zDirection) using the OnCollisionEnter method below. The reason we use 2 axes is to keep the Boss moving in more of a random motion
        // and not just back and forth.
        if (xDirection)
        { 
            Vector3 movement = new Vector3((0.01f * speed), 0, 0);
            transform.Translate(movement);
        }
        else if (!xDirection)
        {
            Vector3 movement = new Vector3((-0.01f * speed), 0, 0);
            transform.Translate(movement);
        }

        // We use the same logic from above, but apply it to the z access walls to control motion on the z axis
        if (!zDirection)
        { 
            Vector3 movement = new Vector3(0, 0, (0.01f * speed));
            transform.Translate(movement);
        }
        else if (zDirection)
        {
            Vector3 movement = new Vector3(0, 0, (-0.01f * speed));
            transform.Translate(movement);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        //When the Boss hits a normal wall, flip the direction boolean so that the FixedUpdate method will switch directions
        if (other.gameObject.CompareTag("Wall"))
        {
            //This performs the boolean flip. it's pretty simple. If it's true, change it to false. If it's false, change it to true
            if (xDirection)
            {
                xDirection = false;
            }
            else if (!xDirection)
            {
                xDirection = true;
            }
        }
        // This is pretty much the same logic as above - flip a boolean whenever a collision with a wall occurs. These walls are on the Z
        // axis of the Boss's movement though
        // There are 3 z wall game objects - one in front of and two behind the boss. These objects contrain the boss from the front
        // and the back (other wise he would just go everywhere)
        if (other.gameObject.CompareTag("Boss Z Wall"))
        {
            //This performs the boolean flip. it's pretty simple. If it's true, change it to false. If it's false, change it to true
            if (zDirection)
            {
                zDirection = false;
            }
            else if (!zDirection)
            {
                zDirection = true;
            }
        }
    }
}
