using UnityEngine;
using UnityEngine.AI;

public class MoveToClosestTarget : MonoBehaviour
{
    public Transform[] Targets;
    public NavMeshAgent Agent;

    public void ChooseTarget()
    {
        float closestTargetDistance = float.MaxValue;
        NavMeshPath Path = null;
        NavMeshPath ShortestPath = null;

        for (int i = 0; i < Targets.Length; i++)
        {
            if (Targets[i] == null)
            {
                continue;
            }
            Path = new NavMeshPath();

            if (NavMesh.CalculatePath(transform.position, Targets[i].position, Agent.areaMask, Path))
            {
                float distance = Vector3.Distance(transform.position, Path.corners[0]);

                for (int j = 1; j < Path.corners.Length; j++)
                {
                    distance += Vector3.Distance(Path.corners[j - 1], Path.corners[j]);
                }

                if (distance < closestTargetDistance)
                {
                    closestTargetDistance = distance;
                    ShortestPath = Path;
                }
            }
        }

        if (ShortestPath != null)
        {
            Agent.SetPath(ShortestPath);
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 300, 50), "Move To Target"))
        {
            ChooseTarget();
        }
    }
}
