using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class Ball : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public float speed = 10f;

    int score = 0;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(ResetBall());
    }

    /// <summary>
    /// Places ball at starting positions and sets random trajectory
    /// </summary>
    private IEnumerator ResetBall()
    {
        rigidbody.velocity = Vector2.zero;
        transform.position = Vector2.zero;

        yield return waitForUser();
        Invoke(nameof(SetRandomTrajectory), 1f);
    }

    private IEnumerator waitForUser()
    {
        bool done = false;
        while(!done)
        {
            if(Input.anyKey)
            {
                done = true;
            }
            yield return null;
        }
    }

    /// <summary>
    /// Moves ball from starting position to a random trajectory
    /// </summary>
    private void SetRandomTrajectory()
    {
        Vector2 force = new Vector2();
        force.x = Random.Range(-1f, 1f);
        force.y = -1f;

        rigidbody.AddForce(force.normalized * speed);
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = rigidbody.velocity.normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Out of Bounds") {
            StartCoroutine(ResetBall());
            GameManager.Instance.Lives -= 1;
        }

        if (collision.gameObject.CompareTag("Brick"))
        {
            Destroy(collision.gameObject);
            score += 10;
            GameManager.Instance.Score = score;
        }

    }

}
