using UnityEngine;

public class _InstakillObstacle : MonoBehaviour
{
    [SerializeField] bool _allContacts;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("Touchinsta");
            other.gameObject.GetComponent<_PlayerHP>().Instakill();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Touchinsta");
            collision.gameObject.GetComponent<_PlayerHP>().Instakill();
        }
    }


}
