using UnityEngine;

namespace Assets.Scripts
{
    public class Comet : MonoBehaviour
    {
        public float StartSpeed;
        public GameObject ExplosionEffect;
        public GameObject FlashEffect;
        public GameObject Body { get; private set; }
        public bool IsConsumed { get; set; }
        public float Life { get; set; }

        private GameController controller;
        private float initScale, initLife;
        private float initLight;
        private Light lightSource;
        private CometTail tail;

        private void Start()
        {
            Body = gameObject;

            controller = GameController.controller;
            lightSource = GetComponentInChildren<Light>();
            tail = GetComponentInChildren<CometTail>();

            initScale = 0.4f;
            initLife = (controller.GetNumberOfProbes()-1)*controller.AimTime;
            initLight = lightSource.range;
            Life = initLife;

            GetComponent<Rigidbody>().velocity = transform.right*StartSpeed;
        }

        private void Update()
        {
            if (IsConsumed)
            {
                var val = initScale*Life/initLife;
                transform.localScale = new Vector3(val, val, val);
                lightSource.range = initLight*Life/initLife;
                if (Life <= 0)
                {
                    Life = 0;
                    Flash();
                }
            }
        }

        private void FixedUpdate()
        {
            if (IsConsumed) return;
            transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
        }

        public void Explode()
        {
            var boom = (GameObject) Instantiate(ExplosionEffect, transform.position, transform.rotation);
            Destroy(boom, 4f);
            Kill();
        }


        internal void Flash()
        {
            var pow = (GameObject) Instantiate(FlashEffect, transform.position, transform.rotation);
            Destroy(pow, 1f);
            Kill();
        }

        internal void Kill()
        {
            tail.DetachParticles();
            Destroy(Body);
        }
    }
}