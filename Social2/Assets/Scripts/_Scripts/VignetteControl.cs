using UnityEngine.PostProcessing;
using UnityEngine;
using System.Collections;

public class VignetteControl : MonoBehaviour
{

    public PostProcessingProfile ppProfile;
    public GameObject blockingImage; 

    VignetteModel.Settings vignetteSettings;

    private void Start()
    {
        ChangeVignetteAtStart();
    }
    
    public void CleanScreen()
    {
        vignetteSettings = ppProfile.vignette.settings;
        vignetteSettings.intensity = 0;
        ppProfile.vignette.settings = vignetteSettings;
    }
    
    public void ChangeVignetteAtStart()
    {
        //copy current settings from the profile into a temporary variable
        vignetteSettings = ppProfile.vignette.settings;

        vignetteSettings.color = Color.black;

        //change the intensity in the temporary settings variable
        StartCoroutine(ChangeVignette(1f, 0f, 2f, "start"));

    }

    public void ChangeVignetteAtRuntime()
    {
        //copy current settings from the profile into a temporary variable
        vignetteSettings = ppProfile.vignette.settings;

        vignetteSettings.color = Color.red;
            //new Color(195, 25, 25, 0.5f);

        //change the intensity in the temporary settings variable
        StartCoroutine(ChangeVignette(0.0f, 1f, 0.5f, "runtime"));

    }

    public void ChangeVignetteAtEnd()
    {
        //copy current settings from the profile into a temporary variable
        vignetteSettings = ppProfile.vignette.settings;

        vignetteSettings.color = Color.black;

        //change the intensity in the temporary settings variable
        StartCoroutine(ChangeVignette(0.0f, 1f, 0.5f, "end"));

    }

    IEnumerator ChangeVignette(float v_start, float v_end, float duration, string vignetteType)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            vignetteSettings.intensity = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            ppProfile.vignette.settings = vignetteSettings;
            yield return null;
        }
        vignetteSettings.intensity = v_end;

        if (vignetteType == "end")
        {
            blockingImage.SetActive(true);
        }
        else if (vignetteType == "runtime")
        {
            elapsed = 0.0f;
            while (elapsed < duration)
            {
                vignetteSettings.intensity = Mathf.Lerp(v_end, v_start, elapsed / duration);
                elapsed += Time.deltaTime;
                ppProfile.vignette.settings = vignetteSettings;
                yield return null;
            }
            vignetteSettings.intensity = v_start;
        }
    }
}
