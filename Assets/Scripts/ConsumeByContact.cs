using UnityEngine;

namespace Assets.Scripts
{
    public class ConsumeByContact : MonoBehaviour
    {
		public GameObject VanishEffect;

        private GameController controller;
     
		void Start()
		{
		    controller = GameController.controller;
		}

        void OnTriggerEnter(Collider collider)
        {
            Vanish();
        }

		private void Vanish()
		{
			var woosh = (GameObject) Instantiate(VanishEffect, transform.position, Quaternion.LookRotation(transform.right));
			Destroy(woosh,1f);
            controller.CountProbe(gameObject);
			controller.ActiveComet.Kill ();
            GetComponent<AudioSource>().Play();
		}
    }
}
