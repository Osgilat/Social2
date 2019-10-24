using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorReset : MonoBehaviour
{

    // Update is called once per frame
    void Start()
    {
        GetComponent<VignetteControl>().CleanScreen();
        GetComponent<ColorGradingControl>().CleanScreen();
    }
}
