/*
 * 
 * Welcome to Plinky 3!
 * The glorious rolly battery balls have been replaced by rainbow tanks! Why? Because it's difficult to shoot a laser out of a rolling ball!
 * So now we have Plinky 3: Laser Tanks!
 * It's new and improved. There are the old fashioned box skulls along with the new spawner enemies and the final boss to take down before the trophy! Woo!
 * Complete with awesome new sound effects, shiny new colors, a giant alien frog dude, and some sweet new background music. Enjoy!
 * 
 * 
 * 
 * Notes from days gone past: Plinky 2:
 * It took me FOREVER to get this to work correctly. I ALMOST resorted to running 2 separate playerController scripts,
 * but I ended up discovering a fix for my problem. As it turns out, settings a GameObject's setActive value to false 
 * completely cuts you off from accessing the game object UNLESS you declare a class variable for the object in playerController (supposedly). Alright, no
 * problem. The only problem is, once the game object is deactivated, so is are the script methods, so the method that is waiting 
 * on the reset key to be pressed is no longer available, thus there is no way to reset the game and bring back your object without using an outside script. 
 * It took me 4 hours to figure out how to destroy the players independently without having to pause the entire game or affect the other player who was playing through. 
 * 
 * I also ran into a weird bug where if you use an if/else statement to try to assign a playerobject to a GameObject variable, the
 * objects would be linked together (as if by faith) and would work perfectly until you attempted to reset the object position
 * in which case both the object would reset to the same position and it was a huge mess. ANYWAY. Here is a single
 * PlayerController script that controls 2 separate characters. Hopefully project 3 doesn't involve 4 player split screen, 
 * or else this is going to get messy :/
 * 
 * For the explosion, I shamelessly grabbed the explosion from the SpaceShooter tutorial. However, I didn't set the
 * appropriate materials/textures for the explosion. It ended up looking like purple/white lasers shooting out of the player ball
 * instead of a fireball/sparks like in SpaceShooter. I thought it fit the aethetic of this game better, so I just left
 * it as-is. Turns out bugs really can become features.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    //Origin positions for the player objects. used when we need to reset the player back to the start on floor 1. It 
    //is also used as the origin for a full game reset. We use a separate origin for each player so in the event that both
    //players died and spawn at the same time, they don't bounce off of each other and cause a ruckus. This applies to the
    //spawn points on the other floors too.
    private Vector3 originPositionPlayerOne = new Vector3(-20.32f, 2.44f, -4.97f);
    private Vector3 originPositionPlayerTwo = new Vector3(-18.42f, 2.44f, -4.97f);

    //origin positions for the player objects. used when we need to reset the player back to the start on floor 2
    private Vector3 originPositionFloorTwoPlayerOne = new Vector3(-4.809f, 0.086f, 1.225f);
    private Vector3 originPositionFloorTwoPlayerTwo = new Vector3(-4.809f, 0.086f, 2.913f);

    //origin positions for the player objects. used when we need to reset the player back to the start on floor 3
    private Vector3 originPositionFloorThreePlayerOne = new Vector3(-0.05f, -1.224f, -15.658f);
    private Vector3 originPositionFloorThreePlayerTwo = new Vector3(-0.05f, -1.224f, -14.185f);

    //this is the rigidbody of the player balls - used to apply physics to the balls to make it move around
    private Rigidbody rb;

    // Public speed variable that is set in the Unity editor. Makes the player ball move at an appropriate speed 
    // 10 is normally good. You can set it higher for high-speed fun.
    public float speed;

    //used to display death/respawn text on the player 1 (left) side of the screen
    public Text playerOneText;

    //used to display death/respawn text on the player 2 (right) side of the screen
    public Text playerTwoText;

    //used to access the player score text
    public Text playerOneScoreText;
    public Text playerTwoScoreText;

    //used to show a pretty explosion when we hit an enemy. Shamelessly taken from the Space Shooter tutorial
    public GameObject playerExplosion;

    //These will represent the player GameObjects for the script. They are initialized in Start(). These make it much easier
    //to deal with the players through the process of the game and also allows us to pass them by reference to certain functions,
    //like resetGame()
    private GameObject playerOne;
    private GameObject playerTwo;

    //This is used as a flag to determine whether the game is ready to be reset when the R or P key is pressed (see OnCollisionEnter and OnTriggerEnter)
    //There are individual resets for each players, along with a flag to reset the entire game after a player makes it to the trophy (wins).
    private bool canResetPlayerOne;
    private bool canResetPlayerTwo;
    private bool canResetEntireGame;

    //These variables hold the player position in the game (particularly, what level they are on)
    //This allows the player to be spawned at the origin point of the level they died on instead of at the beginning
    //of the game. These variables will store the Y value only. We don't really care where they player is on the x and z
    //axes, only the y, since that will tell us what floor the player is one at death.
    private int playerOnePosition;
    private int playerTwoPosition;

    //The following variables are used for accessing spawning shots from the player tanks
    public GameObject playerOneShot;
    public GameObject playerTwoShot;

    //Used to access the invisible game object where the shots spawn from in front of the tank barrel
    public Transform playerOneShotSpawn;
    public Transform playerTwoShotSpawn;

    //Used to control the fire rate of the tanks so they don't fire 1000 a second (if the player can smash the fire button
    //that fast). nextFire keeps up with the time between each shot.
    public float fireRate;
    private float nextFire;

    //Audio Source used for sound effects (shot fire sounds)
    //This accesses the audio source component of the object that is set up in the unity editor
    AudioSource audio;

    //This will store the player who has the highest score once someone reaches the finish line
    private int highestScorePlayer;

    // Flags used to determine whether the game is ready to start (works with the Enter key input to start the game) 
    private bool isGameReady;

    // Initialization
    void Start () {
        //Set rb to the RigidBody component of the player object in order to apply physics - NOTE this does nothing if there is no rigid body component
        rb = this.GetComponent<Rigidbody> ();

        //Initialize the audio source
        audio = GetComponent<AudioSource>();

        //Set timescale to 0 while the player is reading instructions - wait on player input before starting
        Time.timeScale = 0;

        //Game is not ready yet - we have to display instructions and such first
        isGameReady = false;

        //Initialize our player position to floor 1
        playerOnePosition = 1;
        playerTwoPosition = 1;

        //Initialize our two GameObject variables to the player 1 and player 2 game objects
        playerOne = GameObject.Find("Player 1");
        playerTwo = GameObject.Find("Player 2");

        //Begin the game. Give the players instructions and what not.
        WaitToStart();


    }

    //Update is mainly used to check for key presses in order to reset the game after a win/lose/death
    void Update()
    {
        //Start the game if isGameReady is true and the user press the Enter key. We then set isGameReady to false so
        //if the user accidentally presses enter it doesnt trigger a restart
        if (Input.GetKey(KeyCode.Space) && isGameReady)
        {
            //Reset the player score text fields
            playerOneScoreText.text = "Score: 0";
            playerTwoScoreText.text = "Score: 0";

            //Clear out the text fields.
            playerOneText.text = "";
            playerTwoText.text = "";

            //Set time to normal speed
            Time.timeScale = 1;

            //Reset isGameReady back to false
            isGameReady = false;
        }

        //after player 1 has died, let the user press R to reset the game
        if (Input.GetKeyDown(KeyCode.R) && canResetPlayerOne)
        {
            ///set the canResetPlayerOne value back to false since the game is being reset for a new playthrough
            canResetPlayerOne = false;

            //Reset player score
            Scores.ResetPlayerOneScore();

            //call resetGame() to put player one back in start position
            resetGameAfterPlayerDeath(playerOne);
        }

        //after player 2 has died, let the user press P to reset the game
        if (Input.GetKeyDown(KeyCode.P) && canResetPlayerTwo)
        {
            //set the canResetPlayerTwo back to false since the game is being reset for a new playthrough
            canResetPlayerTwo = false;

            //Reset player score
            Scores.ResetPlayerTwoScore();

            //call resetGame() to put player two back in start position
            resetGameAfterPlayerDeath(playerTwo);

        }

        //after the game is completely over (someone got to the trophy), this allows the entire game to be reset
        if (Input.GetKeyDown(KeyCode.R) && canResetEntireGame) 
        {
            //set the canResetEntireGame flag back to false now that we are resetting it
            canResetEntireGame = false;

            //reset the game
            resetEntireGame();
        }

        //quit the game if the user hits the escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //Detects when the player 1 hit the fire button and fires a shot accordingly
        if (this.gameObject.name == "Player 1")
        {
            if (Input.GetKeyDown(KeyCode.E) && Time.time > nextFire)
            {
                // Update nextFire and create the shot
                nextFire = Time.time + fireRate;
                Instantiate(playerOneShot, playerOneShotSpawn.position, playerOneShotSpawn.rotation);
                // Play Shot sound
                audio.Play();
            } 
        }
        //Detects when the player 2 hit the fire button and fires a shot accordingly
        if (this.gameObject.name == "Player 2")
        {
            if(Input.GetKeyDown(KeyCode.RightShift) && Time.time > nextFire) {
                // Update nextFire and create the shot
                nextFire = Time.time + fireRate;
                Instantiate(playerTwoShot, playerTwoShotSpawn.position, playerTwoShotSpawn.rotation);
                // Play the shot sound
                audio.Play();
            } 
        }
    }

    // FixedUpdate is called once per frame, used for physics operations (moving objects, like the player object)
    void FixedUpdate () {
        //We use an if/else state to determine which player we are working with.
        //2 sets of axes have been defined for each player (Project Settings->Input). The arrow keys control player 1 and
        //WASD keys control player 2
        if (this.gameObject.name == "Player 1") 
        {
            //When the up arrow is pressed, move the player object forward
            if (Input.GetKey(KeyCode.W) && !canResetPlayerOne)
            {
                transform.position += transform.forward * speed;
            }
            // When the down arrow is pressed, moved the player object backward
            if (Input.GetKey(KeyCode.S) && !canResetPlayerOne)
            {
                transform.position += transform.forward * -speed;
            }

            // Rotate the player object when the user presses the left and right arrow keys
            if (Input.GetKey(KeyCode.A) && !canResetPlayerOne) 
            {
                rb.transform.Rotate(new Vector3(0f, -1f, 0f));
            }
            if (Input.GetKey(KeyCode.D) && !canResetPlayerOne)
            {
                rb.transform.Rotate(new Vector3(0f, 1f, 0f));
            }
        } 
        else if (this.gameObject.name == "Player 2") 
        {
            //When the up arrow is pressed, move the player object forward
            if (Input.GetKey(KeyCode.UpArrow) && !canResetPlayerTwo)
            {
                transform.position += transform.forward * speed;
            }
            // When the down arrow is pressed, moved the player object backward
            if (Input.GetKey(KeyCode.DownArrow) && !canResetPlayerTwo)
            {
                transform.position += transform.forward * -speed;
            }

            // Rotate the player object when the user presses the left and right arrow keys
            if (Input.GetKey(KeyCode.LeftArrow) && !canResetPlayerTwo) 
            {
                rb.transform.Rotate(new Vector3(0f, -1f, 0f));
            }
            if (Input.GetKey(KeyCode.RightArrow) && !canResetPlayerTwo)
            {
                rb.transform.Rotate(new Vector3(0f, 1f, 0f));
            }
        }
    }

    //This event occurs when the player objects collides with a trigger object - we will use to it to detect when the player
    //hits the death floor (which then allows the user to respawn)
    //The event will also display text on the screen stating what has happened (using the separate player text objects)
    //If a player falls off to the death floor, they are reset to the beginning of the game. Why? Because there are walls EVERYWHERE!
    //If you fall off it's your own fault
    void OnTriggerEnter(Collider other)
    {
        //Check and make sure what the player object hit was the death floor
        if (other.gameObject.CompareTag("Death Floor"))
        {
            //check to see if player 1 hit the death floor
            if (this.gameObject.name == "Player 1") {
                //set the object scale to 0 and set isKinematic to true. This makes it looks like the object has been
                //destroyed because it is scaled to 0 (doesn't exist) and isKenematic keeps it from falling through
                //the floor and reacting to physics until we reset the game
                playerOne.transform.localScale = new Vector3(0, 0, 0);
                rb.isKinematic = true;


                //Instantiate a pretty explosion
                Instantiate(playerExplosion, playerOne.transform.position, playerOne.transform.rotation);

                //Make sure the text is white
                playerOneText.color = Color.white;

                //Display text saying the player has fell off the game floors
                playerOneText.text = "Aww man, you fell off the grid :(\n\nBack To The Start You Go!\n\nPress R to Play Restart!";

                //Clear player score
                playerOneScoreText.text = "";

                //Set the canResetPlayerOne flag to true, allowing the game to be reset
                canResetPlayerOne = true;
            }

            //check to see if player 2 hit the death floor
            if (this.gameObject.name == "Player 2") {
                //Set the object scale to 0 and set isKinematic to true. This makes it looks like the object has been
                //destroyed because it is scaled to 0 (doesn't exist) and isKenematic keeps it from falling through
                //the floor and reacting to physics until we reset the game
                playerTwo.transform.localScale = new Vector3(0, 0, 0);
                rb.isKinematic = true;

                //Instantiate a pretty explosion
                Instantiate(playerExplosion, playerTwo.transform.position, playerTwo.transform.rotation);

                //Make sure the text is white
                playerTwoText.color = Color.white;

                //Display text saying the player has fell off the game floors
                playerTwoText.text = "Aww man, you fell off the grid :(\n\nBack To The Start You Go!\n\nPress P to Restart!";

                //Reset player score
                playerTwoScoreText.text = "";

                //Set the canResetPlayerTwo flag to true, allowing the game to be reset
                canResetPlayerTwo = true;
            }
        }
    }

    //This event handles collisions with the enemies and the finish trophy.
    //If the player object hits an enemy, it is reset to the beginning of the level it died on, a message is displayed on screen, and the players side is paused
    //waiting on the user to hit the reset key.
    //NOTE: while it seems like triggers  and the OnTriggerEnter method could be used for this, it tends to mess up the physics of 
    //the enemies and how they react to walls, therefore it is much easier to use OnCollisionEnter to take care of player->enemy and player->trophy collisions
    //
    //If the player hits the finish trophy, then they win the game, text is displayed on the screen, and the game is paused awaiting user input. 
    void OnCollisionEnter(Collision other)
    {
        //Check and make sure what the player object hit was an enemy, spawner, or the boss
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Enemy Spawner") || other.gameObject.CompareTag("Enemy Boss"))
        {
            //If statement to check which player we are dealing with
            if (this.gameObject.name == "Player 1") 
            {
                //Make sure the text is white
                playerOneText.color = Color.white;

                //Misplay text saying the player has won
                playerOneText.text = "You Died!\nYou should probably avoid the enemies :/\n\nPress R to Respawn!";

                //Clear the player 1 score text
                playerOneScoreText.text = "";

                //Instantiate the pretty explosion
                Instantiate(playerExplosion, GameObject.Find("Player 1").transform.position, GameObject.Find("Player 1").transform.rotation);

                //Set the object scale to 0 and set isKinematic to true. This makes it looks like the object has been
                //destroyed because it is scaled to 0 (doesn't exist) and isKenematic keeps it from falling through
                //the floor and reacting to physics until we reset the game
                playerOne.transform.localScale = new Vector3(0, 0, 0);
                rb.isKinematic = true;

                //We also set the canResetPlayerOne flag to true, allowing the game to be reset
                canResetPlayerOne = true;
            }
            else if (this.gameObject.name == "Player 2") 
            {
                //Make sure the text is white
                playerTwoText.color = Color.white;

                //Display text saying the player has won
                playerTwoText.text = "You Died!\nYou should probably avoid the enemies :/\n\nPress P to Respawn!";

                //Clear the player 2 score text
                playerTwoScoreText.text = "";

                //Instantiate the pretty explosion
                Instantiate(playerExplosion, GameObject.Find("Player 2").transform.position, GameObject.Find("Player 2").transform.rotation);

                //Set the object scale to 0 and set isKinematic to true. This makes it looks like the object has been
                //destroyed because it is scaled to 0 (doesn't exist) and isKenematic keeps it from falling through
                //the floor and reacting to physics until we reset the game
                playerTwo.transform.localScale = new Vector3(0, 0, 0);
                rb.isKinematic = true;

                //We also set the canResetPlayerTwo flag to true, allowing the game to be reset
                canResetPlayerTwo = true;
            }
        }

        //When the player collides when the finish trophy, they win! Woo!
        //Check to make sure what the player hit was the finish trophy
        if (other.gameObject.CompareTag("Finish"))
        {
            //Pause the whole game, wait for the user to press R to reset the game (see Update() method)
            Time.timeScale = 0;

            //Clear the score text so it doesn't overlap with the winning text messages
            playerOneScoreText.text = "";
            playerTwoScoreText.text = "";

            //Here we will determine who has the highest score and assign it to highestScore
            int playerOneScore = Scores.PlayerOneScore;
            int playerTwoScore = Scores.PlayerTwoScore;

            // Check to see which score is the highest and assign it to the highestScorePlayer
            if (playerOneScore > playerTwoScore)
                highestScorePlayer = 1;
            else
                highestScorePlayer = 2;

            //Display text saying the player has won on  both sides of the display. Also, check to see which player has won.
            //Easily done by checking the name property of the instance of the object that collided
            //with the finish trophy.
            if (this.gameObject.name == "Player 1") 
            {
                //Set the playerOne side text to a finish line message and display the reset message
                //Check the scores to see if player 1 actually won and display the appropriate message and score
                playerOneText.color = Color.white;
                playerOneText.text = "Congratulations Player 1!\nYou Reached the Finish Line First!\n";

                //Check to see if player 1 won or not
                if (highestScorePlayer == 1)
                    playerOneText.text += "You WON With a Score of " + playerOneScore.ToString() + "!\n\n";
                else
                    playerOneText.text += "However, your score was only " + playerOneScore.ToString() + " so you have lost :(\n\n";
                      
                playerOneText.text += "Press R to Play Again!";


                //Display a message letting player two know that the game is over
                //Check the scores to see if Player 2 won and display the appropriate message and score
                playerTwoText.color = Color.white;
                playerTwoText.text = "Aww man, you didn't make it to the finish line first :(\n";

                //Check to see if player 2 won or not
                if (highestScorePlayer == 2)
                    playerTwoText.text += "But you WON with a score of " + playerTwoScore.ToString() + "!\n\n";
                else
                    playerTwoText.text += "Your score of " + playerTwoScore.ToString() + " was also the lowest.\nBetter luck next time.\n\n";

                playerTwoText.text += "Press R to Play Again";

            } 
            else 
            {
                //Display a message letting player one know that the game is over
                //Check the scores to see if player 1 actually won and display the appropriate message and score
                playerOneText.color = Color.white;
                playerOneText.text = "Aww man, you didn't make it to the finish line first :(\n";

                //Check to see if player 1 won or not
                if (highestScorePlayer == 1)
                    playerOneText.text += "But you WON with a score of " + playerOneScore.ToString() + "!\n\n";
                else
                    playerOneText.text += "Your score of " + playerOneScore.ToString() + " was also the lowest.\nBetter luck next time.\n\n";

                playerOneText.text += "Press R to Play Again!";

                //Set the playerTwo side text to a finish line message and display the reset message
                //Check the scores to see if Player 2 won and display the appropriate message and score
                playerTwoText.color = Color.white;
                playerTwoText.text = "Congratulations Player 2!\nYou Reached the Finish Line First!\n";

                //Check to see if player 2 won or not
                if (highestScorePlayer == 2)
                    playerTwoText.text += "You WON With a Score of " + playerTwoScore.ToString() + "!\n\n";
                else
                    playerTwoText.text += "However, your score was only " + playerTwoScore.ToString() + " so you have lost :(\n\n";

                playerTwoText.text += "Press R to Play Again";
            }

            //Set the canResetEntireGame flag to true allowing the entire game to be reset
            canResetEntireGame = true;
        }

        //The following colliders will be used to set the player position within the game. When they player objects fall and collide with
        //one of the levels, this method will change the player position to the correct level, allowing us to respawn on the correct
        //floor.
        //NOTE: having a collider for floor one isn't necessary as it is the default, but we will make one anyway for security.
        if (other.gameObject.CompareTag("Floor One"))
        {
            //Check to see which player it was that collided with floor one and set the player floor position to 1
            if (this.gameObject.name == "Player 1") {
                playerOnePosition = 1;
            }

            if (this.gameObject.name == "Player 2") {
                playerTwoPosition = 1;
            }
        }

        if (other.gameObject.CompareTag("Floor Two"))
        {
            //Check to see which player it was that collided with floor two and set the player floor position to to 2
            if (this.gameObject.name == "Player 1") {
                playerOnePosition = 2;
            }

            if (this.gameObject.name == "Player 2") {
                playerTwoPosition = 2;
            }
        }

        if (other.gameObject.CompareTag("Floor Three"))
        {
            //Check to see which player it was that collided with floor three and set the player floor position to 3
            if (this.gameObject.name == "Player 1") {
                playerOnePosition = 3;
            }

            if (this.gameObject.name == "Player 2") {
                playerTwoPosition = 3;
            }
        }

    }

    //This method resets the game when an enemy is hit or the player falls to the death floor. It is called in the Update() method when
    //the user presses the R (player one) or P (player two) key. The player to reset is passed as a parameter.
    private void resetGameAfterPlayerDeath(GameObject thePlayerToReset)
    {

        //Set the origin position of the player object. Use a quick and easy switch statement to check whether it is player 1
        //or player 2 that we are resetting so we can drop the player back to the correct origin position.
        if (thePlayerToReset.name == "Player 1")
        {
            //Check to see what the floor position player one was on to determine which floor origin to spawn the player at.
            switch (playerOnePosition)
            {
                case 1:
                    thePlayerToReset.transform.position = originPositionPlayerOne;
                    break;
                case 2:
                    thePlayerToReset.transform.position = originPositionFloorTwoPlayerOne;
                    break;
                case 3:
                    thePlayerToReset.transform.position = originPositionFloorThreePlayerOne;
                    break;
            }
        }
        else
        {
            //Check to see what the floor position player two was on to determine which floor origin to spawn the player at.
            switch (playerTwoPosition)
            {
                case 1:
                    thePlayerToReset.transform.position = originPositionPlayerTwo;
                    break;
                case 2:
                    thePlayerToReset.transform.position = originPositionFloorTwoPlayerTwo;
                    break;
                case 3:
                    thePlayerToReset.transform.position = originPositionFloorThreePlayerTwo;
                    break;
            }
        }

        //Remove any force from the object so it doesn't keep rolling after reset, we can use the rb variable
        //already defined above. This actually does not work too well and I am not sure why. It takes some of the inertia away,
        //but not all of it. Consider it a feature, I suppose?
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.ResetInertiaTensor ();
        rb.ResetCenterOfMass ();

        //Set the object back to it's correct scale and set isKinematic back to false so our object magically
        //respawns and it's physics abilities return to normal.
        thePlayerToReset.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        thePlayerToReset.transform.rotation = new Quaternion(0, 0, 0, 0);
        rb.isKinematic = false;

        //Remove whatever is in the player text field.
        if (thePlayerToReset.name == "Player 1") 
        {
            playerOneText.text = "";
        }
        else 
        {
            playerTwoText.text = "";
        }

    }


    //This method resets the ENTIRE game. It is used after one of the players has reached the trophy and won the game.
    private void resetEntireGame() {
        
        // Reset the game by reloading the entire scene - pretty much like closing and restarting the whole application
        SceneManager.LoadScene("plinkymain");

    }

  
    //This is used to provide game play instructions to both players before starting the game 
    //isGameReady is set to true and the game awaits to user to press the space bar to begin (see Update())
    private void WaitToStart() {
        //Set time scale to zero - freeze the game
        Time.timeScale = 0;

        //Display player instructions and what not.
        playerOneText.text = "Player 1!\n\nReady?\n\nInstructions:\nUse The WASD Keys To Control Your Tank.\nPress E To Fire Your Cannon!\n\nThere Are Random Enemies That Spawn At Different Times - Watch Out!\n\nThe Final Boss Takes 10 Shots To Kill!\n\nIf You Die, Your Score Resets :(\n\nPress Space To Begin or Esc To Exit";
        playerTwoText.text = "Player 2!\n\nReady?\n\nInstructions:\nUse The Arrow Keys To Control Your Tank.\nPress Right Shift To Fire Your Cannon!\n\nThere Are Random Enemies That Spawn At Different Times - Watch Out!\n\nThe Final Boss Takes 10 Shots To Kill!\n\nIf You Die, Your Score Resets :(\n\nPress Space To Begin or Esc To Exit";

        //Set isGameReady to true to let game know it is ready to begin
        isGameReady = true;
    }

}