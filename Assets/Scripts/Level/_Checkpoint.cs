using UnityEngine;

public class _Checkpoint : MonoBehaviour
{
    [SerializeField] _CheckpointManager cm;

    void Start()
    {
        cm = _GlobalValues.cm;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (cm.checkpointList.IndexOf(this) > cm.checkpointList.IndexOf(cm.currentCheckpoint))
            {
                cm.SetCheckpoint(this);
            }

        }
    }
}
