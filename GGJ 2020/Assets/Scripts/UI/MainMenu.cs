using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private CanvasGroup main;
    [SerializeField] private float fadeTime;
    [SerializeField] private Rigidbody collisionCar;
    [SerializeField] private Animator cameraAnim;

    [SerializeField] private Button startGame;
    [SerializeField] private Button credits;
#pragma warning restore 0649

    private Coroutine running;
    private bool inCredits = false;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(startGame.gameObject);
    }

    public void StartGame()
    {
        if (running == null) running = StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        startGame.OnDeselect(new BaseEventData(EventSystem.current));
        collisionCar.velocity = collisionCar.transform.right * -500f;
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(0.05f);
        LoadManager.instance.LoadScene("deadline");

    }

    private IEnumerator FadeOut()
    {
        float timer = 0;
        while(timer < fadeTime)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            main.alpha = Mathf.Lerp(1, 0, timer / fadeTime);
        }
    }

    private IEnumerator FadeIn()
    {
        float timer = 0;
        while (timer < fadeTime)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            main.alpha = Mathf.Lerp(0, 1, timer / fadeTime);
        }
    }

    public void Credits()
    {
        if (!inCredits)
        {
            cameraAnim.SetBool("zoom", true);
            if(running == null) running = StartCoroutine(imbad());
        }

    }

    private IEnumerator imbad()
    {
        yield return new WaitForSeconds(0.1f);
        inCredits = true;
        running = null;
    }

    private void Update()
    {
        if (inCredits)
        {
            if(Input.anyKeyDown || Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel") || Input.GetAxis("Vertical")!= 0)
            {
                cameraAnim.SetBool("zoom", false);
                inCredits = false;
            }
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else 
        Application.Quit();
#endif
    }
}
