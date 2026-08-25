using UnityEngine;
using EasyTextEffects;
using TMPro;
using System.Collections;

public class TextManager : MonoBehaviour
{
    public static TextManager Singleton;

    public TextMeshProUGUI TextAsset;
    public TextEffect NarrationEffect;

    private AudioSource Source;

    public delegate void OnDisplayMessageFinished();

    public float DisplayTime = 10; // in seconds
    public float CharacterSoundDelay = 0.1f;

    private Coroutine ActiveRoutine;

    private float NextWordTime;
    private bool bIsSpeaking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (TextManager.Singleton == null)
        {
            Singleton = this;
        }

        Source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            NarrationEffect.StartManualEffect("text-entry");
        }

        if (bIsSpeaking && Time.time > NextWordTime)
        {
            NextWordTime = Time.time + CharacterSoundDelay;

            Source.pitch = Random.Range(0.8f, 1.2f);
            Source.Play();
        }

    }

    public void DisplayMessage(string Message)
    {
        TextAsset.text = Message;
        NarrationEffect.StopAllEffects();
        if (ActiveRoutine != null)
        {
            StopCoroutine(ActiveRoutine);
            ActiveRoutine = null;
        }

        NarrationEffect.StartManualEffect("text-entry");

        ActiveRoutine = StartCoroutine(WaitForDisplayTime(DisplayTime));

        bIsSpeaking = true;
    }

    public void DisplayMessage(string Message, OnDisplayMessageFinished Callback)
    {
        TextAsset.text = Message;

        NarrationEffect.StartManualEffect("text-entry");

        StartCoroutine(WaitForDisplayTime(DisplayTime, Callback));
    }


    //after the text animation finishes, show text
    public void PostAnimationShowText(TextMeshProUGUI Text)
    {
        Text.alpha = 1;
        bIsSpeaking = false;
        Source.Stop();
    }

    public void PostAnimationHideText(TextMeshProUGUI Text)
    {
        Text.alpha = 0;
    }

    private IEnumerator WaitForDisplayTime(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);

        bIsSpeaking = false;
        Source.Stop();

        NarrationEffect.StartManualEffect("text-exit");
    }

    private IEnumerator WaitForDisplayTime(float WaitTime, OnDisplayMessageFinished Callback)
    {
        yield return new WaitForSeconds(WaitTime);

        bIsSpeaking = false;
        Source.Stop();

        NarrationEffect.StartManualEffect("text-exit");
                
        if (Callback != null)
        {
            Callback.Invoke();
        }
    }
}
