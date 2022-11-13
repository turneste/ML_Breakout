using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PaddleAgent : Agent
{
    public int agentScoreFlag = 0;
    public int livesFlag = 0;
    public Transform BallAgent;
    public Vector2 direction { get; private set; }
    Rigidbody2D paddleBody;
    float speed = 30f;
    public int currentScore;

    public void ResetPaddle()
    {
        this.transform.position = new Vector2(8.75f, this.transform.position.y);
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public override void OnEpisodeBegin()
    {
        GameManager.Instance.trainingEpisodeBegin();
        ResetPaddle();
        paddleBody = this.GetComponent<Rigidbody2D>();
        currentScore = GameManager.Instance.ScoreAgent;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Ball position
        sensor.AddObservation(BallAgent.transform.position);

        // Paddle position
        sensor.AddObservation(this.transform.position);

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
        
        float distanceToBall = Vector2.Distance(this.transform.position, BallAgent.transform.position);
        
        
        // Small reward for being close to the ball
        if (distanceToBall < this.GetComponent<BoxCollider2D>().size.x/2 && BallAgent.transform.position.y > -8f)
        {
            SetReward(.5f);
        }
        
        

        // Large penalty for going out of bounds
        if (BallAgent.transform.position.y < -8f)
        {
            SetReward(-0.25f);
            Debug.Log(GameManager.Instance.LivesAgent);
            if (GameManager.Instance.LivesAgent == 0) {
                EndEpisode();
            }
        }

        // Large Reward for breaking brick - 280 total per level
        if (currentScore < GameManager.Instance.ScoreAgent) {
            SetReward(0.25f);
        }

        if (GameManager.Instance.ScoreAgent % 192 == 0 && GameManager.Instance.ScoreAgent != agentScoreFlag) {
            Debug.Log("LEVEL CLEAR" + GameManager.Instance.ScoreAgent);
            agentScoreFlag = GameManager.Instance.ScoreAgent;
            SetReward(0.5f);
            EndEpisode();
        }

    }
    public void OnCollision(Collision collision)
    {
        // Small reward for hitting ball
        if(collision.gameObject.tag == "BallAgent")
        {
            SetReward(0.25f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
    }
}
