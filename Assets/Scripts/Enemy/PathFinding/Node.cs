using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neightbourds;
    [SerializeField] private float distance = 5f;

    private void Start()
    {
        GetNeightbourd(Vector3.right);
        GetNeightbourd(Vector3.left);
        GetNeightbourd(Vector3.forward);
        GetNeightbourd(Vector3.back);
    }
    void GetNeightbourd(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            var node = hit.collider.GetComponent<Node>();
            if (node != null)
                neightbourds.Add(node);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (var node in neightbourds)
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
