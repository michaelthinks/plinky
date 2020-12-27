using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scores : MonoBehaviour {
    //This class keeps track of the scores for each player. Since each player has there own controller and each shot object
    //takes care of enemy destruction and score increment, we are using a central, static class in order to keep track of and update
    //the scores in the UI. This also allows us to access the Scores from anywhere in the game code
    //This class is instantiated by an invisible object in the game called Scores - without it we are not able to use the class, which
    //wouldn't be good because we need scores!


    //The score variables for each player
    static private int playerOneScore;
    static private int playerTwoScore;

    //Used to access the UI elements for the player scores
    static public Text playerOneScoreText;
    static public Text playerTwoScoreText;

    //Boss shot counter
    //This counter is used in the ShotController file to determine when to destroy the boss
    static private int bossShotCounter;

    void Start ()
    {
        //Intialize our text components to our text objects
        playerOneScoreText = GameObject.Find("Player One Score Text").GetComponent<Text>();
        playerTwoScoreText = GameObject.Find("Player Two Score Text").GetComponent<Text>();
    }

    public Scores()
    {
        //Initialize scores and shot counter to 0
        playerOneScore = 0;
        playerTwoScore = 0;
        bossShotCounter = 0;
    }

    //Increment Player 1 score by 1 point when an enemy is destroyed
    public static void IncrementPlayerOneEnemy() 
    {
        playerOneScore++;

        //Update our UI element with the new score
        playerOneScoreText.text = "Score: " + playerOneScore.ToString();
    }
    //Increment Player 2 score by 1 when an enemy is destroyed
    public static void IncrementPlayerTwoEnemy() 
    {
        playerTwoScore++;

        //Update our UI element with the new score
        playerTwoScoreText.text = "Score: " + playerTwoScore.ToString();
    }
    //Increment Player 1 score by 5 when an enemy spawner is destroyed
    public static void IncrementPlayerOneSpawner() 
    {
        playerOneScore += 5;

        //Update our UI element with the new score
        playerOneScoreText.text = "Score: " + playerOneScore.ToString();
    }
    //Increment Player 2 score by 5 when an enemy spawner is destroyed
    public static void IncrementPlayerTwoSpawner() 
    {
        playerTwoScore += 5;

        //Update our UI element with the new score
        playerTwoScoreText.text = "Score: " + playerTwoScore.ToString();
    }
    //Increment Player 1 score by 10 when an enemy boss is destroyed
    public static void IncrementPlayerOneBoss() 
    {
        playerOneScore += 10;

        //Update our UI element with the new score
        playerOneScoreText.text = "Score: " + playerOneScore.ToString();
    }
    //Increment Player 2 score by 10 when an enemy boss is destroyed
    public static void IncrementPlayerTwoBoss() 
    {
        playerTwoScore += 10;

        //Update our UI element with the new score
        playerTwoScoreText.text = "Score: " + playerTwoScore.ToString();
    }

    //Increment the boss shot counter - it takes 10 shots to kill the boss
    //This counter is used in the ShotController file to determine when to destroy the boss
    public static void IncrementBossShotCounter()
    {
        bossShotCounter++;
    }

    // Returns the boss shot counter - used in ShotController to determine when to destroy the boss
    public static int BossShotCounter()
    {
        return bossShotCounter;
    }

    // Resets the boss shot counter
    public static void ResetBossShotCounter()
    {
        bossShotCounter = 0;
    }

    // Returns Player Ones score
    public static int PlayerOneScore 
    {
        get { return playerOneScore; }
    }

    // Returns Player Twos Score
    public static int PlayerTwoScore
    {
        get { return playerTwoScore; }
    }

    // Resets both players score
    public static void ResetPlayerScores()
    {
        playerOneScore = 0;
        playerTwoScore = 0;
    }

    //Used to reset player 1 score
    public static void ResetPlayerOneScore()
    {
        playerOneScore = 0;

        //Update our UI element with the new score
        playerOneScoreText.text = "Score: " + playerOneScore.ToString();
    }

    //Used to reset player 2 score
    public static void ResetPlayerTwoScore()
    {
        playerTwoScore = 0;

        //Update our UI element with the new score
        playerTwoScoreText.text = "Score: " + playerTwoScore.ToString();
    }
        
}
