using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashImage : MonoBehaviour
{
    private Image im;
    private void Start()
    {
        im = GetComponent<Image>();
        StartCoroutine(flash());
    }

    IEnumerator flash()
    {
        while (0.01f < im.color.a)
        {
            var imColor = im.color;
            imColor.a -= Time.deltaTime;
            im.color = imColor;
            yield return null;
        }
        while (im.color.a < .99f)
        {
            var imColor = im.color;
            imColor.a += Time.deltaTime;
            im.color = imColor;
            yield return null;
        }
    }
}
