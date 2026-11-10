using UnityEngine;

public class _EnemyBase : MonoBehaviour
{
    [SerializeField] float _maxHP;
    public float _currentHP;

    [SerializeField] protected float _damage;

    [SerializeField] protected float _moveSpeed;
    MeshRenderer _renderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentHP = _maxHP;
        _renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        Color tempColor = _renderer.material.color;
        _renderer.material.color = Color.red;
        if (_currentHP <= 0)
        {
            this.enabled = false;
        }
        _renderer.material.color = tempColor;
    }

    
}
