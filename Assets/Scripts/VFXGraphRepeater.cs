using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class VFXGraphRepeater : MonoBehaviour
{
    public VisualEffect vfx;  // Reference to the Visual Effect component
    public float repeatInterval = 3f;  // Time between each burst (3 seconds)

    void Start()
    {
        if (vfx != null)
        {
            StartCoroutine(RepeatVFX());
        }
    }

    IEnumerator RepeatVFX()
    {
        while (true)
        {
            vfx.Play();  // Play the VFX
            yield return new WaitForSeconds(repeatInterval);  // Wait for the set interval before repeating
        }
    }
}
