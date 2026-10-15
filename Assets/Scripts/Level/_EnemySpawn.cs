using UnityEngine;

public class _EnemySpawn : MonoBehaviour
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

    public void Spawn()
    {
        Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
    }
}
