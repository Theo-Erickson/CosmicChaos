using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SanityTiedPostProcess: MonoBehaviour
{
    public GameObject player;
    PostProcessVolume v_Volume;
    PostProcessVolume ch_Volume;
    PostProcessVolume ld_Volume;
    Vignette m_Vignette;
    ChromaticAberration m_chroma;

    void Start()
    {
        player = GameObject.Find("Player");

        m_Vignette = ScriptableObject.CreateInstance<Vignette>();
        m_Vignette.enabled.Override(true);
        m_Vignette.intensity.Override(1f);
        m_Vignette.opacity.Override(1f);

        m_chroma = ScriptableObject.CreateInstance<ChromaticAberration>();
        m_chroma.enabled.Override(true);
        m_chroma.intensity.Override(1f);

        v_Volume = PostProcessManager.instance.QuickVolume(8, 100f, m_Vignette);
        ch_Volume = PostProcessManager.instance.QuickVolume(8, 100f, m_chroma);
    }

    private void Update()
    {
        float newValue;
        float curSTSanity = (player.GetComponent<Player>().shortTermSanity)/100;
        
        m_Vignette.opacity.value = 1f - curSTSanity;

        newValue = (1f - curSTSanity) * 1.33f;
        if (newValue > 1) newValue = 1;

        m_chroma.intensity.value = newValue;

        //m_Vignette.intensity.value = Mathf.Sin(Time.realtimeSinceStartup);
        //m_Vignette.opacity.value = Mathf.Sin(Time.realtimeSinceStartup);
    }

    void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(v_Volume, true, true);
        RuntimeUtilities.DestroyVolume(ch_Volume, true, true);
    }
}  
