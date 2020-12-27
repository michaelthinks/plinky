using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

    // This is a simple script attached to objects like explosions that destroys them after a set amount of time (5 seconds) so they don't stack
    // up in memory. It is attached to the explosion prefabs.
	void Start () {
        Destroy(this.gameObject, 5);
	}
}
