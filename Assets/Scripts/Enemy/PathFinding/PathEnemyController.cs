using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PathEnemyController: EnemyController
{
    [Header("Pathfinding")]
    [SerializeField] private Node currentNode;
    [SerializeField] private Node targetNode;

    private List<Node> currentPath = new List<Node>();
    private int currentIndex;

    public override void Wander(float animMultiplier)
    {
        if(currentPath == null || currentPath.Count == 0)
        {
            CalculatePath();
        }

        FollowPath(animMultiplier);
    }

    private void CalculatePath()
    {
        currentPath = AStar.Run(
            currentNode, 
            x => x == targetNode,
            x => x.neightbourds,
            (x, y) => Vector3.Distance(x.transform.position, y.transform.position),
            x => Vector3.Distance(x.transform.position, targetNode.transform.position)
            );

        currentIndex = 0;
    }

    private void FollowPath(float animMultiplier)
    {
        if (currentPath == null || currentIndex >= currentPath.Count) return;

        Node node = currentPath[currentIndex];

        Vector3 dir = SteeringBehaviours.Seek(transform, node.transform.position);

        Move(dir, 5f ,animMultiplier);

        float distance = Vector3.Distance(transform.position, node.transform.position);

        if (distance < 1f)
        {
            currentIndex++;
        }
    }
}

