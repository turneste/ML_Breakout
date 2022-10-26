using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    public GameObject panelGameOver;
    public GameObject panelContinue;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI livesText;
    public int CC;


    private int maxScoreLevel1 = 432;
    private int maxScoreLevel2 = 864;

    private int score;
    public int Score
    {
        get { return score; }
        set { score = value; scoreText.text = "Score: " + score; }
    }

    private int lives;
    public int Lives
    {
        get { return lives;}
        set { lives = value; livesText.text = "Lives: " + lives; }
    }

    private int level;
    public int Level
    {
        get { return level; }
        set { level = value; levelText.text = "Level: " + level; }
    }

    
    /// <summary>
    /// Unity function - called automatically when script initialized
    /// Game Manager is global and needs to persist during each scene
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    /// <summary>
    /// First function called after Awake() before the first frame update
    /// Loads level 1
    /// </summary>
    private void Start()
    {      
        panelGameOver.SetActive(false);
        panelContinue.SetActive(false);
        Instance = this;
        NewGame();
    }

    /// <summary>
    /// Begins a new game and loads level 1
    /// </summary>
    private void NewGame()
    {
        panelGameOver.SetActive(false);
        Score = 0;
        Lives = 1;
        Level = 1;
    }


    /// <summary>
    /// Check if the player has lost or won
    /// Called once per frame
    /// </summary>
    void Update()
    {
        // Test level
        bool testLevel = true;
        if (testLevel) {
            maxScoreLevel1 = 14;
            maxScoreLevel2 = 28;
        }

        // Player is out of lives and has lost
        if (lives <= 0)
        {
            GameOver();
        }

        // All Bricks have been cleared frist round
        else if(score == maxScoreLevel1 && level == 1)
        {
            Cleared();
        }

        else if(score == maxScoreLevel2 && level == 2)
        {
            GameOver();
        }

    }

    /// <summary>
    /// Player has lost
    /// Destroy all bricks and display game over menu
    /// </summary>
    private void GameOver()
    {
        FindObjectOfType<Paddle>().GetComponent<SpriteRenderer>().enabled = false;
        FindObjectOfType<Ball>().GetComponent<SpriteRenderer>().enabled = false;

        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        foreach(GameObject brick in bricks)
        {
            brick.GetComponent<SpriteRenderer>().enabled = false;
        }
        panelGameOver.SetActive(true);
    }

    /// <summary>
    /// Resets bricks paddle and ball after first level of bricks is destroyed
    /// </summary>
    private void Cleared()
    {
        Level += 1;

        // Reload ball, bricks, and paddle
        FindObjectOfType<Paddle>().ResetPaddle();
        StartCoroutine(FindObjectOfType<Ball>().ResetBall());

        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        foreach(GameObject brick in bricks)
            {
                // Unhide Brick
                brick.GetComponent<SpriteRenderer>().enabled = true;
                brick.GetComponent<BoxCollider2D>().enabled = true;
            }
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("QUIT");
        UnityEngine.Application.Quit();
    }

}
