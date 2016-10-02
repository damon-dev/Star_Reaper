using UnityEngine;

namespace Assets.Chartboost.Scripts
{
    public class ChartboostSingleton : MonoBehaviour 
    {
        public static ChartboostSingleton ChartBoost;

        private void Awake()
        {
            if (ChartBoost == null)
            {
                DontDestroyOnLoad(gameObject);
                ChartBoost = this;
            }
            else if (ChartBoost != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
