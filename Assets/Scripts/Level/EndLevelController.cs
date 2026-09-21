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

    private IDisposable m_AnyButtonSubscription;

    private bool bFinalTextComplete = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Animator.SetTrigger("Start");

        m_AnyButtonSubscription = InputSystem.onAnyButtonPress.CallOnce(_ => ActivateNewScene());

    }

    public void OnAnimationComplete()
    {
        NarrationEffect.StartManualEffect("text-entry");
        Text.alpha = 1;

        bFinalTextComplete = true;

    }

    private void ActivateNewScene()
    {
        Debug.Log("Hello!");
        if (bFinalTextComplete)
        {
            SceneManager.LoadScene(0);
        }
    }
}
