using UnityEngine;

public class TextLocation : MonoBehaviour
{
    public string Text;
    public bool bCanRetrigger = false;

    private bool bHasTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (TextManager.Singleton != null && !bHasTriggered)
        {
            if (!bCanRetrigger)
            {
                bHasTriggered = true;
            }
            TextManager.Singleton.DisplayMessage(Text);
        }
    }


}
