using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameIntroManager : MonoBehaviour
{
    static private int numPlayers;  // one or two player
    static public int PlayerSelect
    {
        get { return numPlayers; }
        set { numPlayers = value; }
    }
    
    static private int difficulty;  // Range of 1 (easy) to 3 (hard)
    static public int DifficultySelect
    {
        get { return difficulty; }
        set { difficulty = value; }
    }

    public void LoadField()
    {
        if (PlayerSelect == 1)
        {
            SceneManager.LoadScene("OnePlayer");
            DifficultySelect = 1;   // default to 1, but there is no purpose in 1 player mode.
        }
        if (PlayerSelect == 2)
        {
            SceneManager.LoadScene("TwoPlayer");
        }
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("QUIT");
        UnityEngine.Application.Quit();
    }
}
