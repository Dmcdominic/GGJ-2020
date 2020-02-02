using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    private AudioSource MusicSource;

    private void Awake()
    {
        MusicSource = GetComponent<AudioSource>();
        MusicSource.volume = 0.60f;
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private AudioClip myintro;

    public void SetIntro (AudioClip intro)
    {
        myintro = intro;
    }

    public void Tmp (AudioClip main)
    {
        StartMusic(myintro, main);
    }

    public void StartMusic (AudioClip intro, AudioClip main)
    {
        StartCoroutine(StartM(intro, main));
    }

    private IEnumerator StartM (AudioClip intro, AudioClip main)
    {
        if(intro == null)
        {
            MusicSource.loop = true;
            MusicSource.clip = main;
            MusicSource.Play();
        }
        else
        {
            MusicSource.loop = false;
            MusicSource.PlayOneShot(intro);
            MusicSource.ignoreListenerPause = true;
            yield return new WaitWhile(() => MusicSource.isPlaying);
            MusicSource.ignoreListenerPause = false;
            MusicSource.loop = true;
            MusicSource.clip = main;
            MusicSource.Play();
        }
        yield return null;

    }

    public void StopMusic ()
    {
        MusicSource.Stop();
    }

    public void TogglePauseMusic()
    {
        if (MusicSource.isPlaying)
            MusicSource.Pause();
        else
            MusicSource.UnPause();
    }
}
