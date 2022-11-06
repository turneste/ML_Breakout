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

    public override void OnEpisodeBegin()
    {
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
        //Debug.Log(distanceToBall);

        
        // Small reward for being close to the ball
        if (distanceToBall < 1f)
        {
            SetReward(1f);
            EndEpisode();
        }
        

        // Large penalty for going out of bounds
        if (BallAgent.transform.position.y < -8f)
        {
            SetReward(-15f);
            EndEpisode();
        }

        // Large Reward for breaking brick - 280 total per level
        if (currentScore < GameManager.Instance.ScoreAgent) {
            SetReward(25f);
            EndEpisode();
        }

    }
    public void OnCollision(Collision collision)
    {
        // Small reward for hitting ball
        if(collision.gameObject.tag == "BallAgent")
        {
            SetReward(1f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
    }
}
