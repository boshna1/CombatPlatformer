using UnityEngine;

public class _EnemySpawn : _BaseTrigger
{
    [SerializeField] _EnemyBase enemyToSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Trigger()
    {
        Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
    }
}
