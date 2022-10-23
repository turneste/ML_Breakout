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

    public void loadField ()
    {
        if (PlayerSelect == 1)
        {
            SceneManager.LoadScene("Level");
        }
        if (PlayerSelect == 2)
        {
            SceneManager.LoadScene("2PLevel");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
