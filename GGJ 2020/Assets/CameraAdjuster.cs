using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraAdjuster : MonoBehaviour
{
    [SerializeField] private Camera orthoCam;

    private int player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<playerID>().p;
        switch (player) {
            case 0:
                orthoCam.rect = new Rect(-0.04f, 0, 0.17f / 2, 0.25f);
                break;
            case 1:
                orthoCam.rect = new Rect(0.93f, 0, 0.17f / 2, 0.25f);
                break;
            case 2:
                orthoCam.rect = new Rect(0.93f, 0.66f, 0.17f / 2, 0.25f);
                break;
            case 3:
                orthoCam.rect = new Rect(-0.04f, 0.66f, 0.17f / 2, 0.25f);
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
