using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private float sightRange;
    [SerializeField] private int angleThreshold;
    [SerializeField] private LayerMask obs;


    public bool CheckRange(Transform self, Transform target)
    {
        return Vector3.Distance(self.position, target.position) <= sightRange;
    }

    public bool CheckAngle(Transform self, Transform target)
    {
        Vector3 directionToTarget = (target.position - self.position).normalized;
        return Vector3.Angle(self.forward, directionToTarget) <= angleThreshold / 2;
    }

    public bool CheckObstacles(Transform self, Transform target)
    {
        Vector3 directionToTarget = (target.position - self.position).normalized;
        return !Physics.Raycast(self.position,directionToTarget,directionToTarget.magnitude,obs);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
            Vector3 leftBoundary = Quaternion.Euler(0, -angleThreshold / 2, 0) * transform.forward * sightRange;
            Vector3 rightBoundary = Quaternion.Euler(0, angleThreshold / 2, 0) * transform.forward * sightRange;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary);


    }

}
