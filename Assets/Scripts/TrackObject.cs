using UnityEngine;

namespace Assets.Scripts
{
    public class TrackObject : MonoBehaviour
    {
        public float Speed = 30;

        private GameController controller;

        void Start()
        {
            controller = GameController.controller;
        }

        void LateUpdate()
        {
            if (controller.ActiveComet != null)
            {
                var rotation = Quaternion.LookRotation
                    (controller.ActiveComet.transform.position - transform.position, transform.TransformDirection(Vector3.up));
                if (controller.ActiveComet.IsConsumed)
                {
                    var targetRotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * Speed *10);
                    transform.rotation = new Quaternion(0, 0, targetRotation.z, targetRotation.w);
                }
                else
                {
                    var targetRotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * Speed);
                    transform.rotation = new Quaternion(0, 0, targetRotation.z, targetRotation.w);
                }
            
            }
            else
            {
                var rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime*Speed/2);
                transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            }
        }
    }
}
