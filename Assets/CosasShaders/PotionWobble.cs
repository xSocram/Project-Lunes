using UnityEngine;

public class PotionWobble : MonoBehaviour
{

    [SerializeField] private float angle = 10f;
    [SerializeField] private float speed = 2f;

    private void Update()
    {
        float wobble = Mathf.Sin(Time.time * speed) * angle;

        transform.rotation = Quaternion.Euler(
            -90f + wobble,
            0f,
            0f
        );
    }
}
