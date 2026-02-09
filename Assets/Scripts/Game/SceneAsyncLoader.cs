using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;


public class SceneAsyncLoader : MonoBehaviour
{
    public TextMeshProUGUI ContinueText;

    private AsyncOperation AsyncLoadingOperation;
    private bool bAsyncOperationComplete = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            if (AsyncLoadingOperation.progress >= 0.9f)
            {
                AsyncOperationCompleted();
            }
        }

        if (bAsyncOperationComplete && Input.anyKeyDown)
        {
            ActivateNewScene();
        }
    }

    public void ActivateNewScene()
    {
        AsyncLoadingOperation.allowSceneActivation = true;
    }

    private void LoadSceneAsync(int SceneIndex)
    {
        AsyncLoadingOperation = SceneManager.LoadSceneAsync(SceneIndex);
        AsyncLoadingOperation.allowSceneActivation = false;
    }

    private void AsyncOperationCompleted()
    {
        bAsyncOperationComplete = true;
        ContinueText.alpha = 1;
    }
}