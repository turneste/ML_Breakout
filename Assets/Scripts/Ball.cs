using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class Ball : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public float speed = 10f;

    public Vector2 startPosition;

    int score = 0;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startPosition = this.transform.position;
        StartCoroutine(ResetBall());
    }

    /// <summary>
    /// Places ball at starting positions and sets random trajectory
    /// </summary>
    public IEnumerator ResetBall()
    {
        this.rigidbody.velocity = Vector2.zero;
        this.transform.position = startPosition;

        yield return waitForUser();
        Invoke(nameof(SetRandomTrajectory), 1f);
    }

    private IEnumerator waitForUser()
    {
        yield return new WaitForSeconds(1.0f);
    }

    /// <summary>
    /// Moves ball from starting position to a random trajectory
    /// </summary>
    private void SetRandomTrajectory()
    {
        Vector2 force = new Vector2();
        force.x = Random.Range(-0.5f, 0.5f);
        force.y = -1f;

        rigidbody.AddForce(force.normalized * speed);
    }

    private void FixedUpdate()
    {
        // Check if ball is stuck going in one direction
        // Set new trajectory
        if (rigidbody.velocity.y == 0) {
            rigidbody.velocity = new Vector2 (rigidbody.velocity.x, speed);
        } else if (rigidbody.velocity.x == 0) {
            rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
        }

        rigidbody.velocity = rigidbody.velocity.normalized * speed;

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Out of bounds (both user and agent)
        if (collision.gameObject.tag == "Out of Bounds") {
            StartCoroutine(ResetBall());
            if (this.tag == "BallUser")
            {
                GameManager.Instance.LivesUser -= 1;
            }
            else if (this.tag == "BallAgent")
            {
                GameManager.Instance.LivesAgent -= 1;
            }    
        }

        // User Brick Collisions
        if (collision.gameObject.CompareTag("BrickUser"))
        {
            // Hide brick
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;

            // Brick score based on color
            if (collision.gameObject.name == "blueBrick" || collision.gameObject.name == "greenBrick"){
                score += 1;
            } else if (collision.gameObject.name == "yellowBrick" || collision.gameObject.name == "goldBrick"){
                score += 4;
            } else if (collision.gameObject.name == "orangeBrick" || collision.gameObject.name == "redBrick"){
                score += 7;
            }

            GameManager.Instance.ScoreUser = score;
        }

        // Agent Brick Collisions
        else if (collision.gameObject.CompareTag("BrickAgent"))
        {
            // Hide brick
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;

            // Brick score based on color
            if (collision.gameObject.name == "blueBrick" || collision.gameObject.name == "greenBrick")
            {
                score += 1;
            }
            else if (collision.gameObject.name == "yellowBrick" || collision.gameObject.name == "goldBrick")
            {
                score += 4;
            }
            else if (collision.gameObject.name == "orangeBrick" || collision.gameObject.name == "redBrick")
            {
                score += 7;
            }

            GameManager.Instance.ScoreAgent = score;
        }

    }

}
