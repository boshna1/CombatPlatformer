using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class _EffectHandler : MonoBehaviour
{
    [SerializeField] VisualEffect effect;
    _PlayerMovement pm;

    public float _timeTillDisable;

    float direction;

    private void Start()
    {
        pm = GetComponent<_PlayerMovement>();   
    }

    public void Effect(bool state)
    {
        effect.gameObject.SetActive(state);
        if (state == true && pm.isGrounded)
        {
            
            effect.Play();
        }
        else
        {
            StartCoroutine(Wait(_timeTillDisable));
        }
        
    }
    public void Effect(bool state, _PlayerMovement.DirX scaleDir)
    {
        effect.gameObject.SetActive(state);
        if (scaleDir == _PlayerMovement.DirX.Left)
        {
            direction = 1;
        }
        else if (scaleDir == _PlayerMovement.DirX.Right)
        {
            direction = -1;
        }
        effect.gameObject.transform.localScale = new Vector3(direction,1,1);
        effect.gameObject.transform.localPosition = new Vector3(-0.7f*direction,-1.11f,0);
        if (state == true && pm.isGrounded)
        {
            effect.Play();
        }
        else
        {
            StartCoroutine(Wait(_timeTillDisable));    
        }
    }

    public void Transform(_PlayerMovement.DirX scaleDir)
    {
        if (scaleDir == _PlayerMovement.DirX.Left)
        {
            direction = 1;
        }
        else if (scaleDir == _PlayerMovement.DirX.Right)
        {
            direction = -1;
        }
        effect.gameObject.transform.localScale = new Vector3(direction, 1, 1);
        effect.gameObject.transform.localPosition = new Vector3(-0.7f * direction, -1.11f, 0);
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        if (effect.gameObject.activeSelf)
        {
            effect.Stop();
        }
    }
}
