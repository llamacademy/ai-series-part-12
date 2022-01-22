using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(MoveToClosestTarget))]
public class MoveToClosestTargetEditor : Editor
{
    public void OnSceneGUI()
    {
        GUIStyle Style = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                textColor = Color.white
            },
            fontSize = 1
        };

        MoveToClosestTarget moveToClosestTarget = (MoveToClosestTarget)target;

        if (moveToClosestTarget == null)
        {
            return;
        }

        int closestIndex = 0;
        float closestTargetDistance = float.MaxValue;
        List<NavMeshPath> Paths = new List<NavMeshPath>();

        for (int i = 0; i < moveToClosestTarget.Targets.Length; i++)
        {
            Paths.Add(new NavMeshPath());
            if (moveToClosestTarget.Targets[i] == null)
            {
                continue;
            }
            NavMeshPath Path = Paths[i];

            if (NavMesh.CalculatePath(moveToClosestTarget.transform.position, moveToClosestTarget.Targets[i].position, moveToClosestTarget.Agent.areaMask, Path))
            {
                float distance = Vector3.Distance(moveToClosestTarget.transform.position, Path.corners[0]);

                for (int j = 1; j < Path.corners.Length; j++)
                {
                    distance += Vector3.Distance(Path.corners[j - 1], Path.corners[j]);
                }

                if (distance < closestTargetDistance)
                {
                    closestTargetDistance = distance;
                    closestIndex = i;
                }

                Handles.Label(moveToClosestTarget.Targets[i].position, $"Vector3 Distance: {Vector3.Distance(moveToClosestTarget.transform.position, moveToClosestTarget.Targets[i].position).ToString("N3")}\r\nPath Distance: {distance.ToString("N3")}", Style);
            }
            else
            {
                Handles.Label(moveToClosestTarget.Targets[i].position, $"Vector3 Distance: {Vector3.Distance(moveToClosestTarget.transform.position, moveToClosestTarget.Targets[i].position).ToString("N3")}\r\nPath Distance: Invalid Path", Style);
            }
        }

        foreach (NavMeshPath Path in Paths)
        {
            if (Paths.IndexOf(Path) == closestIndex)
            {
                Handles.color = Color.green;
            }
            else
            {
                Handles.color = Color.red;
            }

            if (Path.corners.Length > 0)
            {
                Handles.DrawLine(moveToClosestTarget.transform.position, Path.corners[0]);
                for (int i = 0; i < Path.corners.Length - 1; i++)
                {
                    Handles.DrawLine(Path.corners[i], Path.corners[i + 1]);
                }
            }
        }
    }
}
