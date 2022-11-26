using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;
using TMPro;
using System.Threading;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject panelGameOver;
    public GameObject panelPlayerWon;
    public GameObject panelContinue;
    public GameObject panelPause;
    public TextMeshProUGUI scoreTextUser;
    public TextMeshProUGUI scoreTextAgent;
    public TextMeshProUGUI levelTextUser;
    public TextMeshProUGUI levelTextAgent;
    public TextMeshProUGUI livesTextUser;
    public TextMeshProUGUI livesTextAgent;
    public int CC;

    private int maxScoreLevel1 = 432;   // 192 if two player
    private int maxScoreLevel2 = 864;   // 384 if two player

    // Game Settings
    private int DIFFICULTY = GameIntroManager.DifficultySelect;
    private int PLAYER_MODE = GameIntroManager.PlayerSelect;
    private bool PLAYER_WON = false;
    private bool AGENT_WON = false;
    private bool PLAYER_RESET = true;  // If false, bricks won't be able to reset
    private bool AGENT_RESET = true;

    public PaddleAgent AgentEASY;
    public PaddleAgent AgentMED;
    public PaddleAgent AgentHARD;

    // User Player Info Set-up
    private int scoreUser;
    public int ScoreUser
    {
        get { return scoreUser; }
        set { scoreUser = value; scoreTextUser.text = "P1 Score: " + scoreUser; }
    }

    private int livesUser;
    public int LivesUser
    {
        get { return livesUser;}
        set { livesUser = value; livesTextUser.text = "Lives: " + livesUser; }
    }

    private int levelUser;
    public int LevelUser
    {
        get { return levelUser; }
        set { levelUser = value; levelTextUser.text = "Level: " + levelUser; }
    }
    // END User Player Info

    // Agent Player Info Set-up
    private int scoreAgent;
    public int ScoreAgent
    {
        get { return scoreAgent; }
        set
        {
            if (PLAYER_MODE == 2)
            {
                scoreAgent = value; 
                scoreTextAgent.text = "P2 Score: " + scoreAgent; 
            }
        }
    }

    private int livesAgent;
    public int LivesAgent
    {
        get { return livesAgent; }
        set
        {
            if (PLAYER_MODE == 2)
            {
                livesAgent = value;
                livesTextAgent.text = "Lives: " + livesAgent;
            }
        }
    }

    private int levelAgent;
    public int LevelAgent
    {
        get { return levelAgent; }
        set
        {
            if (GameIntroManager.PlayerSelect == 2)
            {
                levelAgent = value;
                levelTextAgent.text = "Level: " + levelAgent;
            }
        }
    }
    // END Agent Player Info


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
        panelPlayerWon.SetActive(false);
        panelContinue.SetActive(false);
        panelPause.SetActive(false);
        Instance = this;
        NewGame();

        if (PLAYER_MODE == 2) {
            if (DIFFICULTY == 1) {
                Instantiate(AgentEASY);
            } else if (DIFFICULTY == 2) {
                Instantiate(AgentMED);
            } else if (DIFFICULTY == 3) {
                Instantiate(AgentHARD);
            }
        }
        
    }

    /// <summary>
    /// Begins a new game and loads level 1
    /// </summary>
    private void NewGame()
    {
        panelGameOver.SetActive(false);
        ScoreUser = 0;
        LivesUser = 3;
        LevelUser = 1;

        // Agent's values will be set regardless of mode (won't be used in 1 player)
        ScoreAgent = 0;
        LivesAgent = 3;
        LevelAgent = 1;

        if (PLAYER_MODE == 2)
        {
            maxScoreLevel1 = 192;   // 432 if one player
            maxScoreLevel2 = 384;   // 864 if one player
        }
    }


    /// <summary>
    /// Check if the player has lost or won
    /// Called once per frame
    /// </summary>
    void Update()
    {
        // Pause/Exit functionality
        if (Input.GetKey ("p"))
        {
            // Disable pause/resume if at end of game
            if (PLAYER_WON == false && AGENT_WON == false)
            {
                Time.timeScale = 0;
                panelPause.SetActive(true);
            }
        }

        else if (Input.GetKey ("r"))
        {
            // Disable pause/resume if at end of game
            if (PLAYER_WON == false && AGENT_WON == false)
            {
                Time.timeScale = 1;
                panelPause.SetActive(false);
            }
        }

        if (Input.GetKey ("escape"))
        {
            Time.timeScale = 1;
            Destroy(this.gameObject);
            SceneManager.LoadScene("Global");
        }

        
        // Player is out of lives and has lost
        if (livesUser <= 0)
        {
            if (PLAYER_MODE == 2)
            {
                AGENT_WON = true;
            }
            GameOver();
        }

        // Player passed level 1
        else if(scoreUser == maxScoreLevel1 && levelUser == 1)
        {
            LevelUser += 1;
            Cleared();
            PLAYER_RESET = false;
        }

        // Player won
        else if(scoreUser == maxScoreLevel2 && levelUser == 2)
        {
            PLAYER_WON = true;
            AGENT_WON = false;
            GameOver();
        }

        if (PLAYER_MODE == 2)
        {
            if (livesAgent <= 0)
            {
                PLAYER_WON = true;
                GameOver();
            }

            // Agent passed level 1
            else if (scoreAgent == maxScoreLevel1 && levelAgent == 1)
            {
                LevelAgent += 1;
                Cleared();
                AGENT_RESET = false;
            }

            // Agent won
            else if (scoreAgent == maxScoreLevel2 && levelAgent == 2)
            {
                AGENT_WON = true;
                PLAYER_WON = false;
                GameOver();
            }
        }

    }

    /// <summary>
    /// Player has lost
    /// Destroy all bricks and display game over menu
    /// </summary>
    private void GameOver()
    {
        // Turn off paddle
        GameObject paddleUser = GameObject.FindGameObjectWithTag("PaddleUser");
        paddleUser.GetComponent<SpriteRenderer>().enabled = false;

        // Turn off ball
        GameObject ballUser = GameObject.FindGameObjectWithTag("BallUser");
        ballUser.GetComponent<SpriteRenderer>().enabled = false;
        ballUser.GetComponent<BoxCollider2D>().enabled = false;

        // Turn off bricks
        GameObject[] bricksUser = GameObject.FindGameObjectsWithTag("BrickUser");
        foreach(GameObject brick in bricksUser)
        {
            brick.GetComponent<SpriteRenderer>().enabled = false;
        }

        // Repeate for agent if applicable
        if (PLAYER_MODE == 2)
        {
            GameObject paddleAgent = GameObject.FindGameObjectWithTag("PaddleAgent");
            paddleAgent.GetComponent<SpriteRenderer>().enabled = false;

            GameObject ballAgent = GameObject.FindGameObjectWithTag("BallAgent");
            ballAgent.GetComponent<SpriteRenderer>().enabled = false;
            ballAgent.GetComponent<BoxCollider2D>().enabled = false;

            GameObject[] bricksAgent = GameObject.FindGameObjectsWithTag("BrickAgent");
            foreach (GameObject brick in bricksAgent)
            {
                brick.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        if (PLAYER_WON)
        {
            panelPlayerWon.SetActive(true);
        }

        else if (AGENT_WON)
        {
            panelGameOver.SetActive(true);
        }

        else
        {
            panelGameOver.SetActive(true);
        }
    }

    /// <summary>
    /// Resets bricks paddle and ball after first level of bricks is destroyed
    /// </summary>
    private void Cleared()
    {
        if (PLAYER_RESET && LevelUser == 2)
        {
            // Reload ball, bricks, and paddle
            //GameObject.FindGameObjectWithTag("PaddleUser").GetComponent<Paddle>().ResetPaddle();
            StartCoroutine(GameObject.FindGameObjectWithTag("BallUser").GetComponent<Ball>().ResetBall());

            GameObject[] bricksUser = GameObject.FindGameObjectsWithTag("BrickUser");
            foreach (GameObject brick in bricksUser)
            {
                // Unhide Brick
                brick.GetComponent<SpriteRenderer>().enabled = true;
                brick.GetComponent<BoxCollider2D>().enabled = true;
            }

            PLAYER_RESET = false;
        }

        if (AGENT_RESET && LevelAgent == 2)
        {
            // Reload ball, bricks, and paddle
            //GameObject.FindGameObjectWithTag("PaddleAgent").GetComponent<Paddle>().ResetPaddle();
            StartCoroutine(GameObject.FindGameObjectWithTag("BallAgent").GetComponent<Ball>().ResetBall());

            GameObject[] bricksAgent = GameObject.FindGameObjectsWithTag("BrickAgent");
            foreach (GameObject brick in bricksAgent)
            {
                // Unhide Brick
                brick.GetComponent<SpriteRenderer>().enabled = true;
                brick.GetComponent<BoxCollider2D>().enabled = true;
            }

            AGENT_RESET = false;
        }

    }

    public void RetryGame()
    {
        Destroy(this.gameObject);
        if (GameIntroManager.PlayerSelect == 1)
        {
            SceneManager.LoadScene("OnePlayer");
        }
        if (GameIntroManager.PlayerSelect == 2)
        {
            SceneManager.LoadScene("TwoPlayer");
        }
    }

    public void ReturnToMain()
    {
        Destroy(this.gameObject);
        SceneManager.LoadScene("Global");
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("QUIT");
        UnityEngine.Application.Quit();
    }

}
