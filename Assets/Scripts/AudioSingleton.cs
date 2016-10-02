using UnityEngine;

namespace Assets.Scripts
{
    public class AudioSingleton : MonoBehaviour
    {
        public static AudioSingleton BackAudio;

        private void Awake()
        {
            if (BackAudio == null)
            {
                DontDestroyOnLoad(gameObject);
                BackAudio = this;
            }
            else if (BackAudio != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
