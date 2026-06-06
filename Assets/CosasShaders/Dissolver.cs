using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    public float dissolveDuration = 2f;
    public float dissolveAmount;

    [SerializeField] private Material dissolveMaterial;

    public void StartDissolve()
    {
        StartCoroutine(Dissolve());
    }

    public IEnumerator Dissolve()
    {
        float elapsedTime = 0f;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] newMaterials = new Material[renderer.materials.Length];

            for (int i = 0; i < renderer.materials.Length; i++)
            {
                Material oldMat = renderer.materials[i];

                Material newMat = new Material(dissolveMaterial);

                if (oldMat.HasProperty("_BaseMap") &&
                    newMat.HasProperty("_BaseMap"))
                {
                    newMat.SetTexture(
                        "_BaseMap",
                        oldMat.GetTexture("_BaseMap"));
                }

                if (oldMat.HasProperty("_BaseColor") &&
                    newMat.HasProperty("_BaseColor"))
                {
                    newMat.SetColor(
                        "_BaseColor",
                        oldMat.GetColor("_BaseColor"));
                }

                newMaterials[i] = newMat;
            }

            renderer.materials = newMaterials;
        }

        while (elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;

            dissolveAmount = Mathf.Lerp(
                0f,
                1f,
                elapsedTime / dissolveDuration);

            foreach (Renderer renderer in renderers)
            {
                foreach (Material mat in renderer.materials)
                {
                    mat.SetFloat("_DissolveAmount", dissolveAmount);
                }
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
