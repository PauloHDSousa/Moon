using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] ParticleSystem bulletImpactFX;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.contacts.Length > 0)
        {
            var contact = collision.contacts[0];
            var bulletFX = Instantiate(bulletImpactFX, contact.point, Quaternion.identity);
            Destroy(bulletFX, 1f);
        }

        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
