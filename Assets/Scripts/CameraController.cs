/*
 * CameraController has been deprecated!!
 * Due to the way movement is handled with the player object (tanks) in Plinky 3: Tank Wars, there is no need
 * for a camera controller because the camera are child elements of the player objects themselves, thus they follow
 * them around with need for a controller!
 * 
 * I am leaving this file here for review purposes or in case I even need it again
 * 
 * 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //This script makes the camera follow the player around

    //Public variable used to access the player object (player object is set in the Unity Editor)
    public GameObject player;

    // Vector (x,y,z coordinate) used to store the offset distance between the player object and the camera
    private Vector3 offset;

    void Start()
    {
        //Check to make sure player object is not null.
        if (player !=null) {
            //Calculate the difference in distance between the camera and the player object
            offset = transform.position - player.transform.position;
        }
    }

    void LateUpdate()
    {
        //Check to make sure player object is not null.
        if (player != null) {
            //Update the position of the camera in relation to the player object
            //transform.position = player.transform.position + offset;
        }
    }
}

*/