using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using TMPro;

public class PaddleAgent : Agent
{
    public GameObject BallAgent;
    public Vector2 direction { get; private set; }
    Rigidbody2D paddleBody;
    public float speed = 30f;
    public int currentScore;
    public int currentLives;
    public int checkScore;
    public int checkLives;
    public Vector2 startPosition;
    bool training = false;

    public void Start () {
        startPosition = this.transform.localPosition;
        paddleBody = this.GetComponent<Rigidbody2D>();
        BallAgent = GameObject.FindGameObjectWithTag("BallAgent");
    }


    public void ResetPaddle()
    {
        this.transform.localPosition = startPosition;
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public override void OnEpisodeBegin()
    {
        if (training) {
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

            // Reset lives & Score
            currentScore = 0;
            currentLives = 3;

            checkScore = 0;
            checkLives = 3;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {

        // Paddle position
        sensor.AddObservation(this.transform.localPosition.x);
        sensor.AddObservation(this.transform.localPosition.y);
        
        // Distance from ball to paddle
        sensor.AddObservation(this.transform.localPosition.x - BallAgent.transform.localPosition.x);
        sensor.AddObservation(this.transform.localPosition.y - BallAgent.transform.localPosition.y);

        // Velocity
        sensor.AddObservation(BallAgent.GetComponent<Rigidbody2D>().velocity);

        // Angle between ball and walls
        sensor.AddObservation(Vector2.Angle(this.transform.localPosition, BallAgent.transform.localPosition));

    }

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

        // Small penalty for not completing level
        SetReward(-.01f);
                
        float distanceToBall = Vector2.Distance(this.transform.localPosition, BallAgent.transform.localPosition);
        // Small reward for being close to the ball
        if (distanceToBall < this.GetComponent<BoxCollider2D>().size.x/2 && BallAgent.transform.localPosition.y > -7.5f)
        {
            SetReward(0.5f);
        }
                
        // Large penalty for going out of bounds
        if (currentLives < checkLives)
        {
            SetReward(-1.0f);
            //EndEpisode();
            
            checkLives = currentLives;

            if (checkLives == 0) {
                EndEpisode();
            }
            
        }

        // Reward for breaking bricks
        if (currentScore > checkScore) {
            checkScore = currentScore;
            SetReward(0.5f);
        }

        if (currentScore % 192 == 0 && currentScore != 0) {
            EndEpisode();
        }

    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        // Small reward for hitting ball
        if(collision.gameObject.tag == "BallAgent")
        {
            SetReward(1.0f);
        }
        
    }

}
