using UnityEngine;

namespace Assets.Scripts
{
    public class DestroyOnEscape : MonoBehaviour
    {
        private GameController controller;
        void Start()
        {
            controller = GameController.controller;
        }
        void OnTriggerExit(Collider collider)
        {
            controller.ActiveComet.Kill();
        }
    }
}
