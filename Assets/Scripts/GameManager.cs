using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    public GameObject ballPrefab;
    public GameObject playerPrefab;
    public GameObject panelGameOver;
    public GameObject panelContinue;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI livesText;
    public int CC;

    GameObject ball;
    GameObject player;

    public Vector2Int size;
    public Vector2 offset;

    // Bricks
    public GameObject blueBrick;
    public GameObject greenBrick;
    public GameObject yellowBrick;
    public GameObject goldBrick;
    public GameObject orangeBrick;
    public GameObject redBrick;

    //**************No longer needed
    //public GameObject brickPrefab;

    //Color[] blockColour = {Color.blue, Color.green, Color.yellow, 
    //    new Color(0.7f, 0.47f, 0.19f), new Color(0.81f, 0.52f, 0.22f), Color.red};

    //******************


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

    private void NewGame()
    {
        panelGameOver.SetActive(false);
        Score = 0;
        Lives = 3;
        Level = 1;

        LoadLevel(Level);
    }

    public void LoadLevel(int level)
    {
        GenerateLevel();
        player = Instantiate(playerPrefab);
        ball = Instantiate(ballPrefab);
        scoreText.enabled = true;
        levelText.enabled = true;
        livesText.enabled = true;
    }

    public void GenerateLevel()
    {
        GameObject[] bricks = {blueBrick, greenBrick, yellowBrick, goldBrick, orangeBrick, redBrick};

        // Starting position
        float positionX = -15.1f;

        float y = 0.0f;
        // Rows
        for (int i = 0; i < 6; i++)
        {
            float x = positionX;

            // Columns
            for (int j = 0; j < 18; j++)
            {
                
                GameObject newBrick = Instantiate(bricks[i], transform);
                newBrick.transform.position = transform.position + new Vector3(x, y, 0.0f);
                x += newBrick.GetComponent<BoxCollider2D>().size.x * .05f;

                // newBrick.transform.position = transform.position + new Vector3((float)((size.x - 1) * 0.5f - i)*offset.x, j * offset.y, 0);
                // newBrick.GetComponent<SpriteRenderer>().color = blockColour[j];
                // newBrick.hits = 6 - i;
            }
            y += blueBrick.GetComponent<BoxCollider2D>().size.y * .03f;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        panelGameOver.SetActive(false);
        panelContinue.SetActive(false);
        scoreText.enabled = false;
        levelText.enabled = false;
        livesText.enabled = false;
        Instance = this;
        CC = 1;
        NewGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (lives <= 0)
        {
            GameOver();
        }

        if(GameObject.FindGameObjectsWithTag("Brick").Length == 0 && CC == 1)
        {
            CC = 0;
            Cleared();
        }
    }

    private void GameOver()
    {
        CC = 0;
        Destroy(player);
        Destroy(ball);
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        foreach(GameObject brick in bricks)
        {
            GameObject.Destroy(brick);
        }
        panelGameOver.SetActive(true);
    }

    private void Cleared()
    {
        if( CC != 0)
        {
            Destroy(player);
            Destroy(ball);
            panelContinue.SetActive(true);
            Level += 1;
        }
    }

    public void ResetLevel()
    {
        NewGame();
    }

    public void NextLevel()
    {
        CC = 1;
        LoadLevel(Level);
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("QUIT");
        UnityEngine.Application.Quit();
    }

    // Game Manager is global and needs to persist during each scene
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
