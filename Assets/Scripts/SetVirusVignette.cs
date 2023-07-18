using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SetVirusVignette : MonoBehaviour
{
    [SerializeField] private PlayerInfo AtlasPlayerInfo;

    [SerializeField] private Volume vignetteVolume;

    private Vignette vg;
    private void Start()
    {
        vignetteVolume.profile.TryGet(out vg);
    }


    private void Update()
    {
        vg.intensity.value = AtlasPlayerInfo.virus / AtlasPlayerInfo.battery;

    }

}

