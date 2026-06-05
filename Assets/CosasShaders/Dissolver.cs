using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    public float dissolveDuration = 2f;
    public float dissolveAmount;

    public Color startColor;
    public Color endColor;

    public void StartDissolve()
    {
        StartCoroutine(dissolver());
    }

    public IEnumerator dissolver()
    {
        float elapsedTime = 0f;

        Material dissolveMaterial = GetComponent<Renderer>().material;
        Color LerpedColor;


        while(elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;
            dissolveAmount = Mathf.Lerp (0f, 1f, elapsedTime / dissolveDuration);
            dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);

            LerpedColor = Color.Lerp(startColor,endColor,dissolveAmount);
            dissolveMaterial.SetColor("_BaseColor", LerpedColor);

            yield return null;
        }
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartDissolve();
        }
    }
}
