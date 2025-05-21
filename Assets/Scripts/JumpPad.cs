using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 200;
    private float _jumpPadTopSurface = 180;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("if 1");
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.Log("foreach");
                float angle = Vector3.Angle(contact.normal, Vector3.up);
                Debug.Log(angle);
                if (angle == _jumpPadTopSurface)
                {
                    Debug.Log("if 2");
                    if (collision.gameObject.TryGetComponent(out Rigidbody rb))
                    {
                        Debug.Log("if 3");
                        rb.velocity = new Vector3(rb.velocity.x,0f, rb.velocity.z);
                        rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                    }
                    return;
                }
            }
        }
    }
}