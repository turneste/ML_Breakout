using System.Diagnostics;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    //public Ball ball { get; private set; }
    //public Paddle paddle { get; private set; }
    //public Brick[] bricks { get; private set; }

    public int score = 0;
    public int lives = 3;
    public int level = 1;
    public int numPlayers = 1;  // one of two player
    public int difficulty = 1;  // Range of 1 (easy) to 3 (hard)

    public void updateNumPlayers(int num)
    {
        this.numPlayers = num;
    }

    public void updateDifficulty(int diff)
    {
        if (diff > 0 && diff < 4)
        {
            this.difficulty = diff;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        NewGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);

        //SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void NewGame()
    {
        //this.score = 0;
        //this.lives = 3;

        //LoadLevel(1);
    }

    public void LoadLevel(int level)
    {
        //this.level = level;
        //SceneManager.LoadScene("Level" + level);
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        //this.ball = FindObjectOfType<Ball>();
        //this.paddle = FindObjectOfType<Paddle>();
        //this.bricks = FindObjectsOfType<Brick>();
    }

    private void ResetLevel()
    {
        //this.ball.ResetBall();
        //this.paddle.ResetPaddle();
    }

    private void GameOver()
    {
        //SceneManager.LoadScene("GameOver");
        NewGame();
    }

    //public void Hit(Brick brick)
    //{
    //    //this.score += brick.points;

    //    //if (Cleared())
    //    //{
    //    //    LoadLevel(this.level + 1);
    //    //}
    //}

    public void Miss()
    {
        //this.lives--;

        //if (this.lives > 0)
        //{
        //    ResetLevel();
        //}
        //else
        //{
        //    GameOver();
        //}
    }

    private bool Cleared()
    {
        //for (int i = 0; i < this.bricks.Length; i++)
        //{
        //    if (this.bricks[i].gameObject.activeInHierarchy && !this.bricks[i].unbreakable)
        //    {
        //        return false;
        //    }
        //}

        return true;
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("QUIT");
        UnityEngine.Application.Quit();
    }

}
