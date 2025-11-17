using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreMemory : MonoBehaviour
{
    public string m_MemoryName;
    public string m_GrabText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (TextManager.Singleton != null)
        {
            TextManager.OnDisplayMessageFinished OnMessageFinishedCallback = OnDisplayLineFinished;
            TextManager.Singleton.DisplayMessage(m_GrabText, OnDisplayLineFinished);
        }
    }

    private void OnDisplayLineFinished()
    {
        SceneManager.LoadScene(0);
    }
}
