using UnityEngine;
using TMPro;
using EasyTextEffects;
using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class EndLevelController : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public TextEffect NarrationEffect;
    public Animator Animator;

    public float MinimumDelay = 50;

    private float DelayEndTime = 0;

    private IDisposable m_AnyButtonSubscription;

    private bool bFinalTextComplete = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Animator.SetTrigger("Start");

        m_AnyButtonSubscription = InputSystem.onAnyButtonPress.Call(_ => ActivateNewScene());

        DelayEndTime = Time.time + MinimumDelay;

    }

    public void OnAnimationComplete()
    {
        Debug.Log("Final Text Complete");

        NarrationEffect.StartManualEffect("text-entry");
        Text.alpha = 1;

        bFinalTextComplete = true;

    }

    private void ActivateNewScene()
    {
        Debug.Log("Hello!");
        if (Time.time >= DelayEndTime)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnDestroy()
    {
        m_AnyButtonSubscription.Dispose();
    }
}
