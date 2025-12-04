using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
