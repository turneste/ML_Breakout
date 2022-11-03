using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PaddleAgent : Agent
{
    public Transform BallAgent;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(BallAgent.transform.position);
        sensor.AddObservation(this.transform.position);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int moveX = actionBuffers.DiscreteActions[0];
        int direction = 0;
        float moveSpeed = 15f;
        if (moveX == 0) { direction = 0; }
        if (moveX == 1) { direction = -1; }
        if (moveX == 2) { direction = 1; }
        transform.position += new Vector3(direction, 0, 0) * Time.deltaTime * moveSpeed;


        float distanceToBall = Vector2.Distance(this.transform.position, BallAgent.transform.position);
        Debug.Log(distanceToBall);

        if (distanceToBall < 1f)
        {
            SetReward(1f);
            EndEpisode();
        }

        if (BallAgent)

        if (BallAgent.transform.position.y < -8f)
        {
            SetReward(-5f);
            EndEpisode();
        }
    }
    public void OnCollision(Collision collision)
    {
        if(collision.gameObject.tag == "BallAgent")
        {
            SetReward(25f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
    }
}
