using UnityEngine;

public class EnemyController : MonoBehaviour
{
    LineOfSight los;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        los = GetComponent<LineOfSight>();
    }

    private void Update()
    {
        if( los.CheckRange(transform, player.transform) && 
            los.CheckAngle(transform, player.transform) &&
            !los.CheckObstacles(transform, player.transform ))

        {
            Debug.Log("Player Detected");
        }
    }
}
