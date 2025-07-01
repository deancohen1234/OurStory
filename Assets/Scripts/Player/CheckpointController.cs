using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    private Checkpoint LastCheckpoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetCheckpoint(Checkpoint Checkpoint)
    {
        LastCheckpoint = Checkpoint;
    }

    public void ResetToLastCheckPoint()
    {
        LastCheckpoint.ReturnToCheckpoint(transform);
    }
}
