using UnityEngine;

namespace Assets.Scripts
{
    public class DestroyByContact : MonoBehaviour
    {
        private GameController controller;
        void Start()
        {
            controller = GameController.controller;
        }

        void OnTriggerEnter(Collider collider)
        {
            controller.ActiveComet.Kill();
        }
    }
}
