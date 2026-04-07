using UnityEngine;

public static class SteeringBehaviours
{
    public static Vector3 Seek(Transform self, Vector3 target)
    {
        Vector3 dir = target - self.position;
        dir.y = 0;
        return dir.normalized;
    }

    public static Vector3 Arrive(Transform self, Vector3 target, float slowingRadius)
    {
        Vector3 dir = target - self.position;
        float distance = dir.magnitude;

        if(distance < 0.01f)
        {
            return Vector3.zero;
        }

        float speed = Mathf.Clamp01(distance / slowingRadius);
        return dir.normalized * speed;

    }

    public static Vector3 Wander(Vector3 currentDirection, float maxAngleChange)
    {
        float randomAngle = Random.Range(-maxAngleChange, maxAngleChange);
        Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);

        Vector3 newDirection = rotation* currentDirection;
        newDirection.y = 0;

        return newDirection.normalized;
    }

    public static Vector3 Pursue(Transform self,Transform target, Vector3 targetVelocity,float maxPredictionTime)
    {
        Vector3 toTarget = target.position - self.position;
        toTarget.y = 0;

        float distance = toTarget.magnitude;

        float predictionTime = Mathf.Clamp(distance /5f,0f, maxPredictionTime);

        Vector3 futurePosition = target.position + targetVelocity * predictionTime;

        return Seek(self, futurePosition);
    }
}
