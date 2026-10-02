using UnityEngine;

public class _SoundRelay : MonoBehaviour
{
    [SerializeField] float vol;
    public void SendSound(string name)
    {
        _AudioManager.Instance.PlaySoundAmbient(name, vol);
    }
}
