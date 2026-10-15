using UnityEngine;

public class _InstakillObstacle : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("Touchinsta");
            other.gameObject.GetComponent<_PlayerHP>().Instakill();
        }
    }


}
