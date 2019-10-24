using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public AudioClip skeleton;
    public AudioClip potion;
    public AudioClip sword;
    public AudioClip won;
    public AudioClip died;

    public void OnSkeletonSee()
    {
        clipToPlay = skeleton;
    }

    public void OnPotionPickup()
    {
        clipToPlay = potion;
    }

    public void OnSwordPickup()
    {
        clipToPlay = sword;
    }

    public void OnWon()
    {
        clipToPlay = won;
    }

    public void OnDied()
    {
        clipToPlay = died;
    }



    public AudioClip clipToPlay = null;

    public bool finishedTrack = false;

    IEnumerator PlaySoundAfterSound()
    {
        SoundManager.Instance.PlayMusic(clipToPlay);
        
        yield return new WaitForSeconds(clipToPlay.length);
        clipToPlay = null;
        finishedTrack = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (finishedTrack && clipToPlay != null)
        {
            finishedTrack = false;
            StartCoroutine(PlaySoundAfterSound());
        }
    }
}
