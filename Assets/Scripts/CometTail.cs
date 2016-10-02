using UnityEngine;

namespace Assets.Scripts
{
    public class CometTail : MonoBehaviour
    {
        public float ParticleLifetime = 1.5f;

        public void DetachParticles()
        {
            transform.parent = null;
            GetComponent<ParticleSystem>().emissionRate = 0;
            Destroy(gameObject, ParticleLifetime);
        }
    }
}