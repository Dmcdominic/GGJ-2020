using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAfterTime : MonoBehaviour
{
    [SerializeField] private float timeToDie;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToDie -= Time.deltaTime;

        if(timeToDie < 0)
        {
            Destroy(gameObject);
        }
    }
}
