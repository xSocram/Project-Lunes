using UnityEngine;

public class LiquidWobble : MonoBehaviour
{
    [Header("Wobble")]
    [SerializeField] private float maxWobble = 0.03f;
    [SerializeField] private float wobbleSpeed = 1f;
    [SerializeField] private float recovery = 1f;

    private Renderer rend;

    private Vector3 lastPos;
    private Vector3 lastRot;

    private float wobbleX;
    private float wobbleZ;

    private float currentX;
    private float currentZ;

    private float time;

    private void Start()
    {
        rend = GetComponent<Renderer>();

        lastPos = transform.position;
        lastRot = transform.eulerAngles;
    }

    private void Update()
    {
        time += Time.deltaTime;

        Vector3 velocity = (transform.position - lastPos) / Time.deltaTime;

        float angVelX = Mathf.DeltaAngle(lastRot.x, transform.eulerAngles.x);
        float angVelZ = Mathf.DeltaAngle(lastRot.z, transform.eulerAngles.z);

        currentX = Mathf.Lerp(currentX, 0f, recovery * Time.deltaTime);
        currentZ = Mathf.Lerp(currentZ, 0f, recovery * Time.deltaTime);

        currentX += Mathf.Clamp(
            (velocity.x + angVelZ * 0.2f) * maxWobble,
            -maxWobble,
            maxWobble
        );

        currentZ += Mathf.Clamp(
            (velocity.z + angVelX * 0.2f) * maxWobble,
            -maxWobble,
            maxWobble
        );

        wobbleX = currentX * Mathf.Sin(time * wobbleSpeed * Mathf.PI * 2f);
        wobbleZ = currentZ * Mathf.Sin(time * wobbleSpeed * Mathf.PI * 2f);

        rend.material.SetFloat("_RotationX", wobbleX);
        rend.material.SetFloat("_RotationZ", wobbleZ);

        lastPos = transform.position;
        lastRot = transform.eulerAngles;
    }
}