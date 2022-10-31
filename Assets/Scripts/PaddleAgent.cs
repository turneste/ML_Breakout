using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PaddleAgent : Agent
{
    public Vector2 direction { get; private set; }
    Rigidbody2D paddleBody;
    float speed = 30f;

    // Start is called before the first frame update
    void Start() {
        paddleBody = GetComponent<Rigidbody2D>();
    }

    // OnEpisodeBegin Considerations
    // 1. Is an "Episode" hitting one brick or clearing the bricks?

    /// <summary>
    /// Set up episode for Agent learning environmnet
    /// Episodes end when Agent solves the task of fails
    /// </summary>
    public override void OnEpisodeBegin() {
        base.OnEpisodeBegin();
    }


    // CollectObservations Considerations
    // 1. Potential Observations
    //      a. Distance from paddle to ball
    //      b. Direction of ball relative to center of paddle
    //      c. Distance from each wall to edge of paddle
    //      d. Brick locations

    /// <summary>
    /// Collect observations for Agent to use in learning
    /// </summary>
    /// <param name="sensor"></param>
    public override void CollectObservations(VectorSensor sensor) {
        base.CollectObservations(sensor);
    }

    // OnActionReceived Considerations
    // 1. Potential Rewards
    //      a. Tiny reward for moving the ball
    //      b. Tiny reward for defelcting ball with paddle
    //      c. Large reward for breaking bricks
    //      d. Large penalty for losing the ball

    /// <summary>
    /// Moves Agent and adds rewards
    /// </summary>
    /// <param name="actions"></param>
    public override void OnActionReceived(ActionBuffers actions) {

        // Movement

        // Single array representing 0, 1, or 2, Set by Agent
        // ***Needs set in Unity Editor***
        int movement = actions.DiscreteActions[0];

        // Move Left
        if (movement == 0) {this.direction = Vector2.left;}

        // Move Right
        else if (movement == 1) {this.direction = Vector2.right;}

        // Do nothing
        else if (movement == 2) {this.direction = Vector2.zero;}

        
    }

    /// <summary>
    /// Updates ball position
    /// </summary>
    private void FixedUpdate()
    {
        if (this.direction != Vector2.zero)
        {
            this.paddleBody.AddForce(this.direction * this.speed);
        }
    }


}
