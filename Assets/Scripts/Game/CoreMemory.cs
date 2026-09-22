using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CoreMemory : MonoBehaviour
{
    public ParticleSystem CompleteSystem;

    public string m_MemoryName;
    public string m_GrabText;

    private const int TRANSITION_SCENE_INDEX = 7;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (TextManager.Singleton != null)
        {
            TextManager.OnDisplayMessageFinished OnMessageFinishedCallback = OnDisplayLineFinished;
            TextManager.Singleton.DisplayMessage(m_GrabText, OnDisplayLineFinished);
        }

        if (DataPersistanceManager.Instance != null)
        {
            DataPersistanceManager.Instance.SaveGame();
        }

        CompleteSystem.Play();
    }

    private void Update()
    {
        if (Keyboard.current.mKey.IsPressed())
        {
            OnTriggerEnter2D(null);
        }
    }

    private void OnDisplayLineFinished()
    {
        SceneManager.LoadScene(TRANSITION_SCENE_INDEX);
    }
}
