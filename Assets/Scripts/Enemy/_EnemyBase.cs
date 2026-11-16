using System.Collections;
using UnityEngine;

public class _EnemyBase : MonoBehaviour
{
    [SerializeField] public float _maxHP;
    public float _currentHP;

    [SerializeField] protected float _damage;

    [SerializeField] protected float _moveSpeed;
    [SerializeField] public MeshRenderer _meshRenderer;
    [SerializeField] Material _damageMat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentHP = _maxHP;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        StartCoroutine(BufferMat());
        if (_currentHP <= 0)
        {
            gameObject.SetActive(false);
        }
        
    }

    IEnumerator BufferMat()
    {
        Material tempMat = _meshRenderer.material;
        _meshRenderer.material = _damageMat;
        yield return new WaitForSeconds(0.2f);
        _meshRenderer.material = tempMat;
        
    }
    
}
