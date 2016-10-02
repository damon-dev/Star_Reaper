using ChartboostSDK;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GuiScripts
{
	public class WinMenu : MonoBehaviour 
	{
		public Text Score;
		public Text Attempts;

		private GameController controller;
        private const string TwitterAddress = "http://twitter.com/intent/tweet";
        private const string TweetLanguage = "en";
        private const string DefaultText = @"With only {0} attempts I managed to beat Star Reaper's level {1} with a score of {2,0:#0.0}! 
#brag #StarReaper https://play.google.com/store/apps/details?id=com.Sora.StarReaper"; 

		void Awake()
		{
		    controller = GameController.controller;
		}

		public void OnNextClick()
		{
			Application.LoadLevel (Application.loadedLevel + 1);
		}

	    public void OnRetryClick()
	    {
	        Application.LoadLevel(Application.loadedLevel);
	    }

	    public void OnShareClick()
	    {
	        Application.OpenURL(TwitterAddress +
	                            "?text=" +
	                            WWW.EscapeURL(string.Format(DefaultText, controller.Attempts, Application.loadedLevel,
	                                controller.Score)) +
	                            "&amp;lang=" + WWW.EscapeURL(TweetLanguage));
	    }

	    public void UpdateWinStats()
	    {
	        float oldScore=0; int oldAttempts=0;
		    DataManager.dataManager.ReportInfo(Application.loadedLevel, ref oldAttempts, ref oldScore);
		    Score.text = "Score: " + controller.Score.ToString("#0.0") +
                         string.Format(" ({0,0:+#0.00;-#0.00;0})", controller.Score - oldScore);
		    Attempts.text = "Attempts: " + controller.Attempts + string.Format(" ({0,0:+#;-#;0})", controller.Attempts - oldAttempts);
	    }
	}
}
