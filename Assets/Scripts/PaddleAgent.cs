using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PaddleAgent : Agent
{
    public Transform BallAgent;
    public Vector2 direction { get; private set; }
    Rigidbody2D paddleBody;
    float speed = 30f;
    public int currentScore;
    public int currentLives;
    public int checkScore;
    public int checkLives;
    public Vector2 startPosition;

    public override void Initialize()
    {
        startPosition = this.transform.localPosition;
    }


    public void ResetPaddle()
    {
        this.transform.localPosition = startPosition;
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public override void OnEpisodeBegin()
    {
        // Reset Ball
        StartCoroutine(transform.parent.gameObject.GetComponentInChildren<Ball>().ResetBall());

        // Reset Bricks
        Transform allBricks = transform.parent.GetChild(0);
        for (int row=0; row<6; row++) {
            Transform brickRow = allBricks.GetChild(row);
            for (int brick=0; brick<8; brick++) {
                brickRow.GetChild(brick).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                brickRow.GetChild(brick).gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        // Reset Paddle
        ResetPaddle();
        paddleBody = this.GetComponent<Rigidbody2D>();

        // Reset lives & Score
        currentScore = 0;
        currentLives = 3;

        checkScore = 0;
        checkLives = 3;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Ball position
        sensor.AddObservation(BallAgent.transform.localPosition.x);
        sensor.AddObservation(BallAgent.transform.localPosition.y);

        // Paddle position
        sensor.AddObservation(this.transform.localPosition.x);

        // Ball velocity
        sensor.AddObservation(BallAgent.GetComponent<Rigidbody2D>().velocity.x);
        sensor.AddObservation(BallAgent.GetComponent<Rigidbody2D>().velocity.y);

    }

    /// <summary>
    /// Updates paddle position
    /// </summary>
    private void FixedUpdate()
    {
        if (this.direction != Vector2.zero)
        {
            this.paddleBody.AddForce(this.direction * this.speed);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int moveX = actionBuffers.DiscreteActions[0];

        if (moveX == 0) { this.direction = Vector2.zero; }
        if (moveX == 1) { this.direction = Vector2.left; }
        if (moveX == 2) { this.direction = Vector2.right; }
        
        float distanceToBall = Vector2.Distance(this.transform.localPosition, BallAgent.transform.localPosition);
        
        // Small reward for being close to the ball
        if (distanceToBall < 1f)
        //if (distanceToBall < this.GetComponent<BoxCollider2D>().size.x/2 && BallAgent.transform.position.y > -8f)
        {
            SetReward(.01f);
        }        

        // Large penalty for going out of bounds
        if (currentLives < checkLives)
        {
            SetReward(-.05f);
            checkLives = currentLives;

            if (checkLives == 0) {
                EndEpisode();
            }
        }

        // Large Reward for breaking bricks
        if (currentScore > checkScore) {
            checkScore = currentScore;
            SetReward(.1f);
        }

        if (currentScore % 192 == 0 && currentScore != 0) {
            Debug.Log("LEVEL CLEAR" + currentScore);
            EndEpisode();
        }

    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Small reward for hitting ball
        if(collision.gameObject.tag == "BallAgent")
        {
            SetReward(.25f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
    }
}
