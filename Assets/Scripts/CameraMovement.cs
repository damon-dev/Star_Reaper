using System.Globalization;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraMovement : MonoBehaviour
    {
        public float smooth;
        public float BorderPadding; //number of units from borders after which the camera starts following
        public float WaitTime; //amount of seconds to wait for comet to enter
        
        private float enterTime;  //time at wich the comet entered the scene
        private bool moveX, moveY; //axis of camera movement
        private Vector3 offset;    //offset distance from comet
        private Vector3 upperCorner, downCorner;  //camera corners to worldpoint position
        private Vector3 smoothVelocity = Vector3.zero;  //needed for camera smooth movement
        private GameController controller;
        private GameObject background;

        private void Start()
        {
            controller = GameController.controller;
            upperCorner = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1, 1, GetComponent<Camera>().nearClipPlane));
            downCorner = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0, 0, GetComponent<Camera>().nearClipPlane));
            AdjustBackground();
        }

        private void AdjustBackground()
        {
            background = GetComponent<Camera>().transform.GetChild(0).gameObject;
            var ratio = (float)Screen.width / Screen.height;
            background.transform.localScale = new Vector3(GetComponent<Camera>().orthographicSize * 2 * ratio, GetComponent<Camera>().orthographicSize * 2 * ratio, 1f);
            var backTexture = Application.loadedLevel%6;
            if (backTexture == 0) backTexture = 6;
            background.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>("Textures/"+backTexture.ToString(CultureInfo.InvariantCulture));
        }

        private void LateUpdate()
        {
            if (controller.ActiveComet == null)
            {
                enterTime = float.PositiveInfinity;
            }
            else if (controller.ActiveComet.GetComponent<Renderer>().isVisible &&
                     float.IsInfinity(enterTime))
            {
                enterTime = Time.time;
            }

            if (Time.time - enterTime > WaitTime)
            {
                CheckIfCameraShouldMove();

                var targetPosition = transform.position;

                if (moveX) targetPosition.x = controller.ActiveComet.transform.position.x + offset.x;
                if (moveY) targetPosition.y = controller.ActiveComet.transform.position.y + offset.y;

                if ((moveX || moveY) == false)
                {
                    targetPosition = new Vector3(0, 0, -10);
                }
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity,
                    smooth);
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(0, 0, -10), ref smoothVelocity,
                    smooth);
            }
        }

        private void CheckIfCameraShouldMove()
        {

            if (controller.ActiveComet.transform.position.x > upperCorner.x - BorderPadding ||
                controller.ActiveComet.transform.position.x < downCorner.x + BorderPadding)
            {
                if (!moveX)
                {
                    moveX = true;
                    offset.x = transform.position.x - controller.ActiveComet.transform.position.x;
                }
            }
            else moveX = false;

            if (controller.ActiveComet.transform.position.y > upperCorner.y - BorderPadding ||
                controller.ActiveComet.transform.position.y < downCorner.y + BorderPadding)
            {
                if (!moveY)
                {
                    moveY = true;
                    offset.y = transform.position.y - controller.ActiveComet.transform.position.y;
                }
            }
            else moveY = false;

            if (IsInMainArea(controller.ActiveComet.Body)) moveX = moveY = false;
        }

        private bool IsInMainArea(GameObject obj) //checks if an object is in the original area at a distance from camera borders
        {
            var x = obj.transform.position.x;
            var y = obj.transform.position.y;

            return x < upperCorner.x - BorderPadding &&
                   y < upperCorner.y - BorderPadding &&
                   x > downCorner.x + BorderPadding &&
                   y > downCorner.y + BorderPadding;
        }
    }
}