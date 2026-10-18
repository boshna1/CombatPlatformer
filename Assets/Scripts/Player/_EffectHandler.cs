using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using System.Collections.Generic;

public class _EffectHandler : MonoBehaviour
{
    [SerializeField] VisualEffect effect;
    _PlayerMovement pm;

    private void Start()
    {
        pm = GetComponent<_PlayerMovement>();   
    }

    public void Effect(bool state)
    {
        if (state == true && pm.isGrounded)
        {
            
            effect.Play();
        }
        else
        {
            effect.Stop();
        }
        effect.enabled = state;
    }
    public void Effect(bool state, _PlayerMovement.DirX scaleDir)
    {
        effect.enabled = state;
        int direction;
        if (scaleDir == _PlayerMovement.DirX.Left)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        effect.gameObject.transform.localScale = new Vector3(direction,1,1);
        effect.gameObject.transform.localPosition = new Vector3(-0.7f*direction,-1.11f,0);
        if (state == true && pm.isGrounded)
        {
            effect.Play();
        }
        else
        {
            effect.Stop();
        }
    }
}
