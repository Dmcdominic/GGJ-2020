using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSelf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToDie());
    }

    private IEnumerator WaitToDie()
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitWhile(() => GetComponent<AudioSource>().clip != null);
        //Well, guess I die
        Destroy(gameObject);
    }
}
