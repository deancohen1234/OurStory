using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other)
        {
            CheckpointController controller = other.GetComponent<CheckpointController>();
            if (controller)
            {
                Debug.Log("resetting Checkpoint");

                controller.ResetToLastCheckPoint();
            }
        }
    }
}
