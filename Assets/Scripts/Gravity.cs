using UnityEngine;

namespace Assets.Scripts
{
    public class Gravity : MonoBehaviour
    {
        public float Strenght;

        private GameController controller;

        void Start()
        {
            controller = GameController.controller;
        }

        private void FixedUpdate()
        {
            if (!controller.Gravity||controller.ActiveComet==null) return;
            var comet = controller.ActiveComet.Body;
            var line = transform.position - comet.transform.position;
            var distance = Vector2.Distance(transform.position, comet.transform.position);

            line.Normalize();
            comet.GetComponent<Rigidbody>().AddForce((line*(comet.GetComponent<Rigidbody>().mass*Strenght))/(distance*distance));
        }
    }
}
