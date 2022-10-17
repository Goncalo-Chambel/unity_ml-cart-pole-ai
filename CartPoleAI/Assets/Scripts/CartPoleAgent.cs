using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CartPoleAgent : Agent
{
    Rigidbody rb;

   
    [SerializeField]
    private Transform pole;
    Rigidbody poleRb;

    [SerializeField]
    private Rigidbody poleTipRb;

    [SerializeField]
    private float speed = 0.5f;

    private Vector3 startingPos;

    float RoundAngle(float angle) // convert angle range to remove discontinuities in the desired range
    {
        angle %= 360;
        return angle > 180 ? angle - 360 : angle < -180 ? angle + 360 : angle;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        poleRb = pole.GetComponent<Rigidbody>();
        startingPos = transform.position;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(poleRb.angularVelocity.z);
        sensor.AddObservation(RoundAngle(pole.eulerAngles.z));
    }
    
    public override void OnActionReceived(ActionBuffers actions)
    {
        int move = actions.DiscreteActions[0];

        if (move == 1)
            rb.velocity += new Vector3(speed, 0, 0);
        else
            rb.velocity -= new Vector3(speed, 0, 0);

        
        if (Mathf.Abs(RoundAngle(pole.eulerAngles.z)) < 20)
            AddReward(0.1f);
        else
            EndEpisode();        
    }
    
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        // 1 -> move right
        // 0 -> move left
        if (Input.GetAxisRaw("Horizontal") > 0)
            discreteActions[0] = 1;
        else if (Input.GetAxisRaw("Horizontal") < 0)
            discreteActions[0] = 0;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 randomForce = new Vector3(Random.Range(5f, 10f) * Mathf.Sign(Random.Range(-1f,1f)), 0, 0);
            poleRb.AddForce(randomForce);
        }
    }
    public override void OnEpisodeBegin()
    {
        rb.transform.localPosition = Vector3.zero + new Vector3(Random.Range(-2f, 2f), 0, 0);
        rb.velocity = Vector3.zero;
        pole.localRotation = Quaternion.Euler(0, 0, Random.Range(-5f, 5f));
        pole.localPosition = new Vector3(0, 2, 0);
        poleRb.velocity = poleRb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            AddReward(-75);
            EndEpisode();
        }
    }
}
