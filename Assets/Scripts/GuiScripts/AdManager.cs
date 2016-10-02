using ChartboostSDK;
using UnityEngine;

namespace Assets.Scripts.GuiScripts
{
    public class AdManager : MonoBehaviour
    {
        private const string ADCOLONY_VERSION = "version:1.0.0.2,store:google";
        private const string ADCOLONY_APP_ID = "appb8eb92c52e664ab09a";
        private const string ADCOLONY_ZONE_ID = "vz767345b267184ddea7";
        private static int adsRequested;

        void OnEnable()
        {
            AdColony.Configure(ADCOLONY_VERSION, ADCOLONY_APP_ID, ADCOLONY_ZONE_ID);
            ChartboostSDK.Chartboost.didDisplayInterstitial += Chartboost_didDisplayInterstitial;
        }

        void OnDisable()
        {
            ChartboostSDK.Chartboost.didDisplayInterstitial -= Chartboost_didDisplayInterstitial; 
        }

        void Chartboost_didDisplayInterstitial(CBLocation obj)
        {
            adsRequested = 0;
        }

        void OnLevelWasLoaded(int level)
        {
            ChartboostSDK.Chartboost.cacheInterstitial(CBLocation.LevelComplete);
        }

        public static void ShowChartboostAd(CBLocation location)
        {
            adsRequested += 1;
            if(adsRequested>=3)
                ChartboostSDK.Chartboost.showInterstitial(location);
        }

        public static void ShowAdColonyVideo()
        {
            AdColony.ShowVideoAd(ADCOLONY_ZONE_ID);
        }

        public static bool IsVideoAvailable()
        {
            return AdColony.IsVideoAvailable(ADCOLONY_ZONE_ID);
        }
    }
}
