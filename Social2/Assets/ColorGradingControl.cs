using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class ColorGradingControl : MonoBehaviour {


    public PostProcessingProfile ppProfile;

    public float saturation;

    ColorGradingModel.Settings colorGradingSettings;

    private void Start()
    {
        colorGradingSettings = ppProfile.colorGrading.settings;
    }

    public void ChangeSaturationAtRuntime(float saturation)
    {
        //copy current settings from the profile into a temporary variable
        colorGradingSettings = ppProfile.colorGrading.settings;

        Debug.Log("SATURATION " + saturation);
        colorGradingSettings.basic.saturation = saturation;

        ppProfile.colorGrading.settings = colorGradingSettings;
        //colorGradingSettings.color = Color.red;
        //new Color(195, 25, 25, 0.5f);

        //change the intensity in the temporary settings variable
        //StartCoroutine(ChangeVignette(0.0f, 1f, 0.5f, "runtime"));

    }

    public void CleanScreen()
    {
        colorGradingSettings = ppProfile.colorGrading.settings;
        colorGradingSettings.basic.saturation = 2;
        ppProfile.colorGrading.settings = colorGradingSettings;
    }

}
