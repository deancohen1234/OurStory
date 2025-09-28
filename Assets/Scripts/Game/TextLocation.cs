using UnityEngine;

public class TextLocation : MonoBehaviour
{
    public string Text;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (TextManager.Singleton != null)
        {
            TextManager.Singleton.DisplayMessage(Text);
        }
    }


}
