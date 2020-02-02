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
                orthoCam.rect = new Rect(-0.04f, 0, 0.17f, 0.57f);
                break;
            case 1:
                orthoCam.rect = new Rect(0.86f, 0, 0.17f, 0.57f);
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
