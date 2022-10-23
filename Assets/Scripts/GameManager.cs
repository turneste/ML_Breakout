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
    public GameObject brickPrefab;
    Color[] blockColour = {Color.blue, Color.green, Color.yellow, 
        new Color(0.7f, 0.47f, 0.19f), new Color(0.81f, 0.52f, 0.22f), Color.red};

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
        set { level = value; levelText.text = "Level " + level; }
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
        for (int i = 0; i < size.x; i++)
        {

            for (int j = 0; j < size.y; j++)
            {
                GameObject newBrick = Instantiate(brickPrefab, transform);
                newBrick.transform.position = transform.position + new Vector3((float)((size.x - 1) * 0.5f - i)*offset.x, j * offset.y, 0);
                newBrick.GetComponent<SpriteRenderer>().color = blockColour[j];
                // newBrick.hits = 6 - i;
            }
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

}
