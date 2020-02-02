using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(playerID))]
public class CrashSound : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (gameObject.CompareTag(other.gameObject.tag))
        {
            playSound(1);
            StartCoroutine(play());
        }
        GetComponent<Rigidbody>().AddExplosionForce(other.relativeVelocity.sqrMagnitude * other.rigidbody.mass * 1.25f,other.contacts[0].point + Vector3.down *.001f,10);
    }

    void playSound(float k)
    {
        SoundManager.instance
            .PlayOnce(SoundManager.instance.config
                    .crashes[(int)(Random.value * SoundManager.instance.config.crashes.Count)],
                Random.value * k);
    }

    IEnumerator play()
    {
        var r = Random.value;
        var power = 1f;
        for (int i = 0;r > power; i++)
        {
            yield return new WaitForSeconds(1.2f - power);
            playSound(power);
            power /= 2;
        }
    }
}
