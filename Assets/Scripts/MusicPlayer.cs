using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	// This script just players the background music for the game
    // Since there are 2 players (and multiples of pretty much everything) I didn't really have a central place to start the music
    // So it has it's on script
    // It is used by the invisible Background Music object in Unity
    // Music provided by http://www.bensound.com/ under a Creative Commons License

    AudioSource audio;

    void Start() {
        // Intialize audio component
        audio = GetComponent<AudioSource>();

        // Play that sci-fi jam
        audio.Play();
    }
   
}
