﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public GameObject AudioPlayer;
    public GameObject OneShotPlayer;
    public AudioConfig config;

    private Dictionary<string, GameObject> myLoops = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if(instance != null)
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

    public GameObject StartLoop(AudioClip clip,string carID, float volume = 1f)
    {
        //If already playing, don't do it again you stupid
        if (myLoops.ContainsKey(clip.ToString() + carID)) return null; 
        GameObject newPlayer = Instantiate(AudioPlayer);
        newPlayer.transform.SetParent(transform);
        AudioSource newSrc = newPlayer.GetComponent<AudioSource>();
        newSrc.loop = true;
        newSrc.clip = clip;
        newSrc.volume = volume;
        newPlayer.name = clip.ToString() + carID;
        newSrc.Play();
        myLoops.Add(newPlayer.name, newPlayer);
        return newPlayer;
    }
    

    public void StopLoop(AudioClip clip, string carID)
    {
        string search = clip.ToString() + carID;
        if (myLoops.ContainsKey(search))
        {
            Destroy(myLoops[search]);
            myLoops.Remove(search);
        }
    }

    public void PlayWhile(Func<bool> f,AudioClip ac,string carID,float volume = 1)
    {
        StartCoroutine(_PlayWhile(f,ac,carID,volume));
    }

    public void PlayWhen(Func<bool> f, AudioClip ac, float volume = 1)
    {
        StartCoroutine(_PlayWhen(f, ac, volume));
    }
    
    IEnumerator _PlayWhen(Func<bool> f, AudioClip ac, float volume)
    {
        while (true)
        {
            yield return new WaitUntil(f);
            PlayOnce(ac,volume);
            yield return new WaitWhile(f);
        }
    }
    
    IEnumerator _PlayWhile(Func<bool> f, AudioClip ac, string carID,float volume)
    {
        while (true)
        {
            yield return new WaitUntil(f);
            StartLoop(ac, carID, volume);
            yield return new WaitWhile(f);
            StopLoop(ac,carID);
        }
    }

    public void PlayOnce(AudioClip clip ,float volume = 1f, bool random = false)
    {
        if (random)
            OneShotPlayer.GetComponent<AudioSource>().pitch = 1f + UnityEngine.Random.Range(-0.1f, 0.1f);
        else
            OneShotPlayer.GetComponent<AudioSource>().pitch = 1f;
        OneShotPlayer.GetComponent<AudioSource>().PlayOneShot(clip,volume);
    }
}
