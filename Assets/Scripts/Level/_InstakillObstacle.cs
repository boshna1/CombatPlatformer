using UnityEngine;

public class _InstakillObstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Touchinsta");
            collision.gameObject.GetComponent<_PlayerHP>().Instakill();
        }
    }


}
