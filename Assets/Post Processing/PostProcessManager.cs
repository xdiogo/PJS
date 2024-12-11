using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using DG.Tweening;

public class PostProcessManager : MonoBehaviour
{
    public static PostProcessManager current;
    public Material postProcessMaterial;
    public Material instancePostProcessMaterial;

    private FullScreenPassRendererFeature postPass;

    private void Awake()
    {
        current = this;
        var render = (GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset).GetRenderer(0);
        var property = typeof(ScriptableRenderer).GetProperty("rendererFeatures", BindingFlags.NonPublic | BindingFlags.Instance);

        var features = property.GetValue(render) as List<ScriptableRendererFeature>;
        postPass = features.Where(x => x is FullScreenPassRendererFeature).Select(x => x as FullScreenPassRendererFeature).FirstOrDefault();

        postProcessMaterial = postPass.passMaterial;

        instancePostProcessMaterial = new Material(postProcessMaterial);
        postPass.passMaterial = instancePostProcessMaterial;

    }

    private void OnDestroy()
    {
        postPass.passMaterial = postProcessMaterial;
    }

    [ContextMenu("Fade Out")]
    public void FadeOut()
    {
        instancePostProcessMaterial.DOFloat(1, "_Fade", 1f);
    }

    [ContextMenu("Fade In")]
    public void FadeIn()
    {
        instancePostProcessMaterial.DOFloat(0, "_Fade", 1f).SetEase(Ease.OutQuad);



    }

    [ContextMenu("Test")]
    public void Test()
    {
        StartCoroutine(Test_Routine());
    }

    IEnumerator Test_Routine()
    {
        FadeOut();
        yield return new WaitForSeconds(2f);
        //camera entre o fdaein e fade out 
        Debug.Log("Depois do fade");
        FadeIn();
    }

}