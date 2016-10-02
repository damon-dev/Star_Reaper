using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserBeam : MonoBehaviour
    {
        public float Precision;
        public float Cooldown;
        public float Power;

        public bool Active { get; private set; }
        public float TimeOfLastActive { get; private set; }


        private LineRenderer beamRenderer;
        private float backupSpeed;
        private GameObject pray;
        private GameController controller;
        private float orientation;
        private float laserOffsetValue;

        void Start()
        {
            controller = GameController.controller;
            beamRenderer = GetComponent<LineRenderer>();
        }

        void LateUpdate()
        {
            if (!Active) return;
            if ((Input.GetMouseButtonDown(0)&&!controller.GamePaused)||controller.ActiveComet == null)
            {
                StopHarvesting();
                return;
            }

            var direction = transform.parent.position - pray.transform.position;
            var t = 2*Mathf.PI*direction.magnitude/backupSpeed;

            pray.transform.RotateAround(transform.parent.position, orientation * transform.forward, Time.deltaTime * 360 / t);

            controller.ActiveComet.Life -= Power*(controller.ProbeChargedCounter == controller.GetNumberOfProbes()
                ? 5
                : 1)*Time.deltaTime;

            laserOffsetValue -= Time.deltaTime*0.5f;
            beamRenderer.SetPosition(1, transform.parent.position);
            beamRenderer.SetPosition(0, pray.transform.position);
            beamRenderer.materials[0].SetTextureOffset("_MainTex", new Vector2(laserOffsetValue, 0));
        }

        public void BeginHarvesting(GameObject intruder, float sign)
        {
            if (Active||Time.time-TimeOfLastActive<=Cooldown) return;
            controller.ActiveBeam = this;
            Active = true;
            orientation = sign;
            pray = intruder;
            backupSpeed = pray.GetComponent<Rigidbody>().velocity.magnitude;
            pray.GetComponent<Rigidbody>().velocity = Vector2.zero;
            controller.Gravity = false;
            controller.ActiveComet.IsConsumed = true;
            controller.CountProbe(transform.parent.gameObject);

            ShootBeam();
        }

        public void StopHarvesting()
        {
            if (!Active) return;
            Active = false;
            TimeOfLastActive = Time.time;
            if (pray != null)
            {
                var direction = transform.parent.position - pray.transform.position;
                direction.Normalize();
                var newVelocity = new Vector2(orientation*direction.y, -orientation*direction.x)*backupSpeed;
                pray.GetComponent<Rigidbody>().velocity = newVelocity;
            }
            controller.ActiveComet.IsConsumed = false;
            controller.Gravity = true;

            StopShooting();
        }

        void ShootBeam()
        {
            beamRenderer.enabled = true;
            GetComponent<AudioSource>().Play();
        }

        void StopShooting()
        {
            beamRenderer.enabled = false;
            StartCoroutine(AudioShutdown());
        }

        private IEnumerator AudioShutdown()
        {
            var backubVal = GetComponent<AudioSource>().volume;
            while (GetComponent<AudioSource>().volume > 0.01f)
            {
                GetComponent<AudioSource>().volume -= 0.1f;
                yield return new WaitForSeconds(0.001f);
            }
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().volume = backubVal;
        }
    }
}
