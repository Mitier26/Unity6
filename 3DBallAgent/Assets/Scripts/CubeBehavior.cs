using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CubeBehavior : Agent
{
    public float cubeSpeed = 0.1f;

    public ObstacleGenerator obstacleGenerator;
    private Vector3 startingPosition;
    public event Action OnReset;

    public override void Initialize()
    {
        startingPosition = transform.position;
    }

    public override void Heuristic(in ActionBuffers actionBuffers)
    {
        var discreteAction = actionBuffers.DiscreteActions;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            discreteAction[0] = 1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            discreteAction[0] = 2;
        }
        else
        {
            discreteAction[0] = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Score"))
        {
            AddReward(2f);
        }
    }

    private void Reset()
    {
        transform.position = startingPosition;
        obstacleGenerator.RemoveObstacles();
        OnReset?.Invoke();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];
        AddReward(-0.001f);

        if (action == 1)
        {
            MoveLeft();
        }
        else if (action == 2)
        {
            MoveRight();
        }
    }

    private void FixedUpdate()
    {
        RequestDecision();
        if (transform.position.y < -1f)
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    private void MoveLeft()
    {
        Vector3 position = transform.position;
        position.x -= cubeSpeed;
        transform.position = position;
        //transform.Translate(Vector3.left * cubeSpeed * Time.deltaTime);
    }

    private void MoveRight()
    {
        Vector3 position = transform.position;
        position.x += cubeSpeed;
        transform.position = position;
        transform.Translate(Vector3.right * cubeSpeed * Time.deltaTime);
    }

    public override void OnEpisodeBegin()
    {
        Reset();
    }
}

