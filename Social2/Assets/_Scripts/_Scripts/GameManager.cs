using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject snakes;
    [SerializeField]
    private GameObject redLight;

    private int countSnakes = 5;
    private VignetteControl vignetteControl;

    private void Start()
    {
        vignetteControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<VignetteControl>();
    }

    public void ButcherDied()
    {
        StartCoroutine(ExecuteAfterTime(5));
    }

    public void SnakeDied()
    {
        countSnakes--;

        if (countSnakes <= 0)
        {
            vignetteControl.ChangeVignetteAtEnd();
            redLight.SetActive(false);
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        snakes.SetActive(true);
    }
}
