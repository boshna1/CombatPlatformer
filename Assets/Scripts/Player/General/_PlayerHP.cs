using UnityEngine;

public class _PlayerHP : MonoBehaviour
{
    [Range(0f, 100f)]
    public float currentHP;
    [Range(0f, 100f)]
    public float maxHP;
    // Start is called before the first frame update
    _CheckpointManager cm;

    void Start()
    {
        cm = _GlobalValues.cm;
    }

    public void Instakill()
    {
        Debug.Log("ToLast");
        currentHP = 0;
        cm.ToLastCheckpoint();
    }
}
