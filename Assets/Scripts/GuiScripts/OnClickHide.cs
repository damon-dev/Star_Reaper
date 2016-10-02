using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GuiScripts
{
    public class OnClickHide : MonoBehaviour
    {
        private void Update()
        {
            if (GameController.controller.ActiveComet!=null)
                gameObject.SetActive(false);
        }
    }
}
