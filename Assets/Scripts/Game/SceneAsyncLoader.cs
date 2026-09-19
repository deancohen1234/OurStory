using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;
using System;
using UnityEngine.InputSystem.Utilities;

public class SceneAsyncLoader : MonoBehaviour
{
    public Animator FadeInAnim;
    public TextMeshProUGUI ContinueText;

    private AsyncOperation AsyncLoadingOperation;
    private bool bAsyncOperationComplete = false;

    private IDisposable m_AnyButtonSubscription;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FadeInAnim.SetTrigger("Start");

        ContinueText.alpha = 0;

        if (DataStore.Instance != null)
        {
            int currentLevel = DataStore.Instance.GetCurrentLevel();
            LoadSceneAsync(currentLevel + 1);
        }
    }

    private void Update()
    {
        if (AsyncLoadingOperation != null)
        {
            Debug.Log("Progress: " + AsyncLoadingOperation.progress);
            if (AsyncLoadingOperation.progress >= 0.9f)
            {
                AsyncOperationCompleted();
            }
        }
    }

    public void ActivateNewScene()
    {
        AsyncLoadingOperation.allowSceneActivation = true;
    }

    private void LoadSceneAsync(int SceneIndex)
    {
        Debug.Log("Loading new scene");


        AsyncLoadingOperation = SceneManager.LoadSceneAsync(SceneIndex);
        AsyncLoadingOperation.allowSceneActivation = false;
    }

    private void AsyncOperationCompleted()
    {
        bAsyncOperationComplete = true;
        ContinueText.alpha = 1;

        m_AnyButtonSubscription = InputSystem.onAnyButtonPress.CallOnce(_ => ActivateNewScene());

    }
}