using System.Collections.Generic;
using UnityEngine;

public class TracingWayPoints : MonoBehaviour
{
    public List<Transform> nodes = new List<Transform>();

    Vector3 currentNode;
    Vector3 previousNode;
    int wayCount;

    private void OnDrawGizmos()
    {
        if (transform.childCount != wayCount)
        {
            nodes.Clear();
            wayCount = 0;
        }
        if (transform.childCount > 0)
        {
            foreach (Transform obj in transform)
            {
                if (!nodes.Contains(obj))
                {
                    nodes.Add(obj);
                }
                wayCount++;
            }
        }

        if (nodes.Count >= 2)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                currentNode = nodes[i].position;
                if (i > 0)
                {
                    previousNode = nodes[i - 1].position;
                }
                else if (i == 0 && nodes.Count > 1)
                {
                    previousNode = nodes[nodes.Count - 1].position;
                }
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(previousNode, currentNode);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(currentNode, Vector3.one.x / 2);
            }
        }
    }
}
