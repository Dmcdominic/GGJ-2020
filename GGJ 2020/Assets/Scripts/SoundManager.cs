using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject AudioPlayer;
    public GameObject OneShotPlayer;

    private Dictionary<string, GameObject> myLoops = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tmp (AudioClip clip)
    {
        StartLoop(clip, "01");
    }

    public void Tmp2 (AudioClip clip)
    {
        StopLoop(clip, "01");
    }

    public void StartLoop(AudioClip clip,string carID)
    {
        //If already playing, don't do it again you stupid
        if (myLoops.ContainsKey(clip.ToString() + carID)) return; 
        GameObject newPlayer = Instantiate(AudioPlayer);
        newPlayer.transform.SetParent(transform);
        AudioSource newSrc = newPlayer.GetComponent<AudioSource>();
        newSrc.loop = true;
        newSrc.clip = clip;
        newPlayer.name = clip.ToString() + carID;
        newSrc.Play();
        myLoops.Add(newPlayer.name, newPlayer);
    }
    

    public void StopLoop(AudioClip clip, string carID)
    {
        string search = clip.ToString() + carID;
        Destroy(myLoops[search]);
        myLoops.Remove(search);
    }

    public void PlayOnce(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
