using UnityEngine;
using Unity.Cinemachine;

public class _CameraManager : MonoBehaviour
{
    CinemachineCamera cam;
    bool blended;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponentInChildren<CinemachineCamera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player pass");
            Flip();
        }
    }

    void Flip()
    {
        blended = !blended;
        if (blended)
            cam.Priority.Value = 1;
        else
            cam.Priority.Value = -1;
    }
}
