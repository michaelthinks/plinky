using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishRotator : MonoBehaviour {

    // This script simply rotates the finish line object (the big trophy) around in a circle.
    // It also plays "Victory!" when a player reaches it.

    AudioSource audio;
	
	//Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 40, 0) * Time.deltaTime);
    }

    void Start() {
        //Initialize our audio source
        audio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        //Play to finish line audio when a player collides with the trophy
        if (other.gameObject.name == "Player 1" || other.gameObject.name == "Player 2")
        {
            audio.Play();
        }
    }
}
