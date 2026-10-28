using UnityEngine;

public class _EnemySpawn : _BaseTrigger
{
    [SerializeField] _EnemyBase enemyToSpawn;

    public _EnemyBase _eref;
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
        if (_eref == null)
        {
            _eref = Instantiate(enemyToSpawn, transform.position, Quaternion.identity, transform);
        }
        else
        {
            Destroy(_eref);
            _eref = Instantiate(enemyToSpawn, transform.position, Quaternion.identity, transform);
        }
    }
}
