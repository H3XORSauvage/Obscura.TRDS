using UnityEngine;

namespace Obscura.TRDS
{
    internal class Fly : MonoBehaviour
    {
        private Rigidbody rb;
        private float flySpeed = 10f;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
                rb = gameObject.AddComponent<Rigidbody>();

            rb.useGravity = !Main.flyEnabled;
        }

        void Update()
        {
            // Si script désactivé via Main, ne fait rien
            if (!enabled)
                return;

            Vector3 move = Vector3.zero;

            if (Input.GetKey(KeyCode.Space))
                move += Vector3.up;
            if (Input.GetKey(KeyCode.LeftShift))
                move += Vector3.down;

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            move += transform.forward * v;
            move += transform.right * h;

            rb.velocity = move.normalized * flySpeed;
        }
    }
}
