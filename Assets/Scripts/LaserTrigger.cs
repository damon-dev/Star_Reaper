using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class LaserTrigger : MonoBehaviour
    {

        private LaserBeam laserBeam;
        private const float FadeSpeed = 20;
        private bool isInside;
        private Collider dummy;
		private Color originalColor;

        private void Start()
        {
            laserBeam = transform.parent.GetComponentInChildren<LaserBeam>();
			originalColor = GetComponent<Renderer>().material.GetColor("_TintColor");
        }

        void Update()
        {
            if (dummy == null)
            {
				if(isInside)
				{
	                isInside = false;
	                StartCoroutine(FadeIn());
				}
				GetComponent<Renderer>().material.SetColor("_TintColor",originalColor);
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            dummy = collider;
            isInside = true;
            StartCoroutine(FadeOut());
        }

        private void OnTriggerStay(Collider collider)
        {
            if (laserBeam.Active) return;
            var t = transform.position - collider.transform.position;
            var dot = Vector3.Dot(collider.GetComponent<Rigidbody>().velocity, t);
            var a = new Vector3(collider.GetComponent<Rigidbody>().velocity.x, collider.GetComponent<Rigidbody>().velocity.y, 0);
            var b = new Vector3(t.x, t.y, 0);
            var cross = Vector3.Cross(a, b);
            if (Mathf.Abs(dot) <= laserBeam.Precision)
            {
                laserBeam.BeginHarvesting(collider.gameObject, Mathf.Sign(cross.z));

				var c=new Color(0,0.255f,0.245f,0.40f);
				GetComponent<Renderer>().material.SetColor("_TintColor",c);
            }
        }


        private void OnTriggerExit(Collider collider)
        {
            isInside = false;
            laserBeam.StopHarvesting();
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeOut()
        {
			var c = GetComponent<Renderer>().material.GetColor("_TintColor");
            while (c.a > .1f && GetComponent<Renderer>().enabled)
            {
                var t = Time.deltaTime*FadeSpeed;
                c.a = Mathf.Lerp(c.a, 0f, t);
                GetComponent<Renderer>().material.color = c;
                yield return new WaitForEndOfFrame();
            }
            GetComponent<Renderer>().enabled = false;
        }

        IEnumerator FadeIn()
        {
			var c = GetComponent<Renderer>().material.GetColor("_TintColor");
            while (GetComponent<Renderer>().enabled || Time.time - laserBeam.TimeOfLastActive <= laserBeam.Cooldown)
            {
                yield return new WaitForEndOfFrame();
            }
            while (c.a < .9f && !GetComponent<Renderer>().enabled)
            {
                var t = Time.deltaTime * FadeSpeed;
                c.a = Mathf.Lerp(c.a, 1f, t);
                GetComponent<Renderer>().material.color = c;
                yield return new WaitForEndOfFrame();
            }
            GetComponent<Renderer>().enabled = true;
        }
    }
}
