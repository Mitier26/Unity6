using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class RollerAgent : Agent
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // 타겟의 위치를 식별하기 위한 Transform
    public Transform target;

    // 새 에핏드 환경 성정을 위해 호출
    public override void OnEpisodeBegin()
    {
        // 에이전트가 플랫폽에서 떨어졌을 땨, 위치 정보 초기화
        if (this.transform.localPosition.y < 0)
        {
            this.rb.angularVelocity = Vector3.zero;
            this.rb.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
        
        // 에이선트가 타겟에 도달할 때마다
        // 에피소를 끝내고 타겟의 위치를 플렛폼 내에서 랜덤하게 변경
        target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 타겟과 에이전트의 위치를 관측
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        
        // 에이전트의 속도를 관측
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.z);
    }

    public float forceMultiplier = 10;
    
    //에이전트의 행동과 보상을 처리
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // 행동(Action) - 에이전트가 타겟을 향해 이동하도록 행동을 설정
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rb.AddForce(controlSignal * forceMultiplier);
        
        // 보상(Reward) - 에이전트가 타겟에 도달했는지 거리 확인
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.localPosition);

        // 보상(Reward) - 에이전트가 타겟에 도달했다면 보상을 부여하고 에피소드를 종료
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // 보상(Reward) - 에이전트가 타겟에 도달히지 못하면 보상을 부여하지 않음
        else if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionOut)
    {
        var continuousActionsOut = actionOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
    
}
