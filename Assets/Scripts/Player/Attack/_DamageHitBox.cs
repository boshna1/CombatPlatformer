using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _DamageHitBox : MonoBehaviour
{
    [SerializeField] List<string> _ignoreTags;
    [SerializeField] public float _damage;

    [SerializeField] float knockbackScaleX;
    [SerializeField] float knockbackScaleY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(TryGetComponent<_PlayerBaseClass>(out _PlayerBaseClass comp))
        {
            _ignoreTags.Add("Player");
        }
        else
        {
            _ignoreTags.Add("Enemy");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy") && !_ignoreTags.Contains("Enemy"))
        {
            _EnemyBase temp = other.GetComponent<_EnemyBase>();
            temp._currentHP = Damage(temp._currentHP);
            temp.GetComponent<Rigidbody>().AddForce(Knockback(temp.transform), ForceMode.Impulse);
            temp.TakeDamage();
        }
    }

    public float Damage(float _currentHP)
    {
        return _currentHP -= _damage;
    }

    public Vector3 Knockback(Transform target)
    {
        return new Vector3(((target.position - transform.position).normalized).x * knockbackScaleX,knockbackScaleY,0);
    }
}
