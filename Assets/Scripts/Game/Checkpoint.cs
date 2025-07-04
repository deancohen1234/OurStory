using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform SpawnLocation;

    public void ReturnToCheckpoint(Transform Player)
    {
        Player.SetPositionAndRotation(SpawnLocation.position, SpawnLocation.rotation);
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        Debug.Log("Triggering Checkpoint");


        if (other)
        {
            CheckpointController controller = other.GetComponent<CheckpointController>();
            if (controller)
            {
                controller.SetCheckpoint(this);
                Debug.Log("Setting Checkpoint");
            }
        }
    }
}
