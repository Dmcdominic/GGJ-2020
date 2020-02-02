using System.Collections;
using System.Collections.Generic;
using TypeUtil;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(playerID))]
public class IconManager : MonoBehaviour
{

    [SerializeField] private Image BrakeImage;

    [SerializeField] private Image SteeringWheelImage;

    [SerializeField] private PartList partList;

    [SerializeField] private Image X_mark;

    private Sum<FlashImage, Unit> BrakeFlash;
    private Sum<FlashImage, Unit> SteerFlash;

    [SerializeField] private PlayerBoolRef alive;
    
    
    private int id;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        id = GetComponent<playerID>().p;
        alive[id] = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        BrakeFlash = Sum<FlashImage, Unit>.Inr(new Unit());
        SteerFlash = Sum<FlashImage, Unit>.Inr(new Unit());
    }

    // Update is called once per frame
    void Update()
    {
        if (alive[id])
        {
            _canvasGroup.alpha = 1;
            BrakeFlash.Match<Unit>(f =>
                {
                    if (partList[id].val[(int) part.brake] > 0)
                    {
                        Destroy(f.gameObject);
                        BrakeFlash = Sum<FlashImage, Unit>.Inr(new Unit());
                    }
                    return new Unit();
                },
                u =>
                {
                    if (partList[id].val[(int) part.brake] < 1)
                        BrakeFlash = Sum<FlashImage,Unit>.Inl(Instantiate(X_mark.gameObject,BrakeImage.transform).GetComponent<FlashImage>());
                    return new Unit();
                });
     
            SteerFlash.Match<Unit>(f =>
                {
                    if (partList[id].val[(int) part.steering_wheel] > 0)
                    {
                        Destroy(f.gameObject);
                        SteerFlash = Sum<FlashImage, Unit>.Inr(new Unit());
                    }
                    return new Unit();
                },
                u =>
                {
                    if (partList[id].val[(int) part.steering_wheel] < 1)
                        SteerFlash = Sum<FlashImage,Unit>.Inl(Instantiate(X_mark.gameObject,SteeringWheelImage.transform).GetComponent<FlashImage>());
                    return new Unit();
                });
        }
        else
        {
            _canvasGroup.alpha = 0;
        }

    }
}
