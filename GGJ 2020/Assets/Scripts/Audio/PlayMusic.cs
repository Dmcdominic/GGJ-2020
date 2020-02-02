using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioClip intro;
    public AudioClip loop;

    // Start is called before the first frame update
    void Start()
    {
        MusicManager.instance.StartMusic(intro, loop);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
