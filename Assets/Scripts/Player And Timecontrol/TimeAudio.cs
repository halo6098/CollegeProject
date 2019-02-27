using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAudio : MonoBehaviour
{
    public AudioClip timeStop;
    public AudioClip timeStart;
    public AudioSource audioSource;

    private bool stopTimePlayed;

    // Start is called before the first frame update
    void Awake()
    {
        stopTimePlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
       if(PlayerController.slowTime == true)
        {
            if (stopTimePlayed == false)
            {
                audioSource.clip = timeStop;
                audioSource.Play();
                stopTimePlayed = true;
            }            
        }
       else if (stopTimePlayed == true)
        {
            audioSource.clip = timeStart;
            audioSource.Play();
            stopTimePlayed = false;
        }
    }
}
