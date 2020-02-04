using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[RequireComponent(typeof(playerID))]
public class CrashSound : MonoBehaviour
{
    [SerializeField] private IntEvent shake;
    private void OnCollisionEnter(Collision other)
    {
        if (transform.root == other.gameObject.transform.root || other.gameObject.CompareTag("ground")) {
            return;
        }

        playSound(.5f);
        if (gameObject.CompareTag(other.gameObject.tag))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (!rb) return;
            
            StartCoroutine(play());
            var average_pos = other.contacts.Select(c => c.point).Aggregate((a, b) => a + b) / other.contacts.Length;
            Vector3 dir = average_pos - transform.position;
            float power = other.relativeVelocity.magnitude * Mathf.Pow(other.rigidbody.mass,1.4f);
             
            power *= Mathf.Lerp(2, .5f, Vector3.Dot(dir.normalized, rb.velocity.normalized));
            rb.AddExplosionForce(power,average_pos,10,.1f);
            rb.AddForceAtPosition(other.impulse - other.impulse.y * Vector3.up,average_pos,ForceMode.Impulse);

        }
        
    }

    void playSound(float k)
    {
        shake.Invoke(1);
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
