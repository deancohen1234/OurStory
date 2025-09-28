using UnityEngine;
using EasyTextEffects;
using TMPro;
using System.Collections;

public class TextManager : MonoBehaviour
{
    public static TextManager Singleton;

    public TextMeshProUGUI TextAsset;
    public TextEffect NarrationEffect;

    public float DisplayTime = 10; // in seconds

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (TextManager.Singleton == null)
        {
            Singleton = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            NarrationEffect.StartManualEffect("text-entry");
        }
    }

    public void DisplayMessage(string Message)
    {
        TextAsset.text = Message;

        NarrationEffect.StartManualEffect("text-entry");

        StartCoroutine(WaitForDisplayTime(DisplayTime));
    }

    //after the text animation finishes, show text
    public void PostAnimationShowText(TextMeshProUGUI Text)
    {
        Text.alpha = 1;
    }

    public void PostAnimationHideText(TextMeshProUGUI Text)
    {
        Text.alpha = 0;
    }

    private IEnumerator WaitForDisplayTime(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);

        NarrationEffect.StartManualEffect("text-exit");
    }
}
