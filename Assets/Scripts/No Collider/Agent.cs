using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField]
    private int _swarmIndex;
    public int SwarmIndex { get { return _swarmIndex; } private set { } }

    [SerializeField]
    private float _noClumpingRadius;
    public float NoClumpingRadius { get { return _noClumpingRadius; } private set { } }

    [SerializeField]
    private float _localAreaRadius;
    public float LocalAreaRadius { get { return _localAreaRadius; } private set { } }

    [SerializeField]
    private float _speed;
    public float Speed { get { return _speed; } private set { } }

    [SerializeField]
    private float _steeringSpeed;
    public float SteeringSpeed { get { return _swarmIndex; } private set { } }

    private void Start()
    {
        _swarmIndex = Random.Range(0, 3);
    }

    public void SimulateMovement(List<Agent> agents, float time)
    {
        var steering = Vector3.zero;


        // Calculate Separation
        steering += CalculateSeparation(agents) * 0.5f; // * weight
        steering += CalculateAlignment(agents) * 0.34f; // * weight
        steering += CalculateCohesion(agents) * 0.16f; // * weight
        steering += CalculateLeadership(agents);

        steering += CalculateTarget();


        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _localAreaRadius, LayerMask.GetMask("Default")))
        {
            steering = ((hit.point + hit.normal) - transform.position).normalized;
        }



        if (steering != Vector3.zero)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), _steeringSpeed * time);
        }

        transform.position += transform.TransformDirection(new Vector3(0, 0, _speed)) * time;
    }

    private Vector3 CalculateSeparation(List<Agent> agents)
    {
        Vector3 separationDirection = Vector3.zero;
        int separationCount = 0;

        foreach (var agent in agents)
        {
            if (agent == this)
            {
                continue;
            }

            var distance = Vector3.Distance(agent.transform.position, this.transform.position);

            if (distance < _noClumpingRadius)
            {
                separationDirection += agent.transform.position - transform.position;
                separationCount++;
            }
        }

        if (separationCount > 0)
        {
            separationDirection /= separationCount;
        }

        separationDirection *= -1f;

        return separationDirection.normalized;
    }

    private Vector3 CalculateAlignment(List<Agent> agents)
    {
        Vector3 alignmentDirection = Vector3.zero;
        int alignmentCount = 0;

        foreach (var agent in agents)
        {
            if (agent == this)
            {
                continue;
            }

            var distance = Vector3.Distance(agent.transform.position, this.transform.position);

            if (distance < _localAreaRadius && agent.SwarmIndex == this._swarmIndex)
            {
                alignmentDirection += agent.transform.forward;
                alignmentCount++;
            }
        }

        if (alignmentCount > 0)
        {
            alignmentDirection /= alignmentCount;
        }

        return alignmentDirection.normalized;
    }

    private Vector3 CalculateCohesion(List<Agent> agents)
    {
        Vector3 cohesionDirection = Vector3.zero;
        int cohesionCount = 0;

        foreach (var agent in agents)
        {
            if (agent == this)
            {
                continue;
            }

            var distance = Vector3.Distance(agent.transform.position, this.transform.position);

            if (distance < _localAreaRadius && agent.SwarmIndex == this._swarmIndex)
            {
                cohesionDirection += agent.transform.position - transform.position;
                cohesionCount++;
            }
        }

        if (cohesionCount > 0)
        {
            cohesionDirection /= cohesionCount;
        }

        cohesionDirection -= transform.position;

        return cohesionDirection.normalized;
    }

    private Vector3 CalculateLeadership(List<Agent> agents)
    {
        Vector3 leaderDirection = Vector3.zero;
        float leaderAngle = 180f;
        Agent leader = agents[0];

        foreach (var agent in agents)
        {
            if (agent == this)
            {
                continue;
            }

            var distance = Vector3.Distance(agent.transform.position, this.transform.position);

            if (distance < _localAreaRadius && agent.SwarmIndex == this._swarmIndex)
            {
                var angle = Vector3.Angle(agent.transform.position - transform.position, transform.forward);

                if (angle < leaderAngle && angle < 90f)
                {
                    leader = agent;
                    leaderAngle = angle;
                }
            }
        }

        if (leader != null)
        {
            leaderDirection = (leader.transform.position - transform.position).normalized;

            return leaderDirection;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private Vector3 CalculateTarget()
    {
        var targetDirection = Vector3.zero;

        if (Vector3.Distance(transform.position, targetDirection) < _localAreaRadius)
        {
            //targetDirection = (targetDirection - transform.position).normalized;
        }

        return targetDirection;
    }
}
