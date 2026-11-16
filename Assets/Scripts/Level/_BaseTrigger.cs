using System.Collections;
using UnityEngine;

public class _BaseTrigger : MonoBehaviour
{
    public virtual void Trigger()
    { 
    }

    public virtual IEnumerator WaitRespawn()
    {
        yield return new WaitForSeconds(0);
    }

    public virtual IEnumerator HitGround()
    {
        yield return new WaitForSeconds(0);
    }
}
