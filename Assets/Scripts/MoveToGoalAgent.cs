using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;  // Add this line

public class MoveToGoalAgent : Agent {
    
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;


public override void OnEpisodeBegin()
{
    // Set the agent's position to the center of a specific cube in the grid
    // Assuming each cube is 1 unit in size and the grid is 4x4
    float cubeSize = 1.0f;
    float gridCenterX = (4 - 1) * cubeSize / 2;
    float gridCenterZ = (4 - 1) * cubeSize / 2;
    
    // Set the agent's position to the center of the grid
    transform.localPosition = new Vector3(gridCenterX, 0.5f, gridCenterZ);
}


    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 1f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }
    public override void Heuristic(in ActionBuffers actionsOut) {
    ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
    continuousActions[0] = Input.GetAxisRaw("Horizontal");
    continuousActions[1] = Input.GetAxisRaw("Vertical");
}

    private void OnTriggerEnter(Collider other) 
    {
        if (other.TryGetComponent<Goal>(out Goal goal)) {
            SetReward(+1f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }

        if (other.TryGetComponent<Wall>(out Wall wall)) {
            SetReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
    }


}
