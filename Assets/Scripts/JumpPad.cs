using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 200;
    private float _jumpPadTopSurface = 180;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                float angle = Vector3.Angle(contact.normal, Vector3.up);
                if (angle == _jumpPadTopSurface && collision.gameObject.TryGetComponent(out Rigidbody rb))
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                    rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                    return;
                }
            }
        }
    }
}