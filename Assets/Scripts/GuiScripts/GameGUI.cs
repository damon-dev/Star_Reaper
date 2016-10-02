using System.Globalization;
using ChartboostSDK;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GuiScripts
{
    public class GameGUI : MonoBehaviour
    {
        public Text ScoreField;
        public Text AttemptsField;
        public Text LevelField;
        public GameObject MenuPanel;
		public GameObject WinPanel;
        public GameObject ScreenButton;
        public Image MuteImage;

        private GameController controller;
        private GameObject WinScreen;

        void Start()
        {
            controller = GameController.controller;
            WinScreen = WinPanel.transform.FindChild("WinScreen").gameObject;
            LevelField.text = "Level " + Application.loadedLevel;
        }

        void Update()
        {
            ScoreField.text = controller.Score > 0 ? controller.Score.ToString("#0.0") : "0";
            AttemptsField.text = controller.Attempts.ToString(CultureInfo.InvariantCulture);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(MenuPanel.activeInHierarchy)
                    OnResumeClick();
                else
                    OnMenuClick();
            }
        }

		public void ShowWinScreen()
		{
			WinPanel.SetActive (true);
            WinScreen.SetActive(true);
		    var menu = WinScreen.GetComponent<WinMenu>();
            menu.UpdateWinStats();
            
		}

        public void OnBoomClick()
        {
            if (controller.ActiveComet != null)
				controller.ActiveComet.Explode ();
			else
				controller.ScreenClicked ();
        }

        public void OnScreenClick()
        {
            controller.ScreenClicked();
        }

        public void OnMenuClick()
        {
            if (controller.LevelPassed) return;
            controller.PauseGame();
            MenuPanel.SetActive(true);

            MuteImage.sprite = Resources.Load<Sprite>(!AudioListener.pause ? "GuiSprites/audioOn" : "GuiSprites/audioOff");

            var adButton = MenuPanel.transform.FindChild("VideoAd").gameObject;
            adButton.SetActive(AdManager.IsVideoAvailable());
        }

        public void OnResumeClick()
        {
            MenuPanel.SetActive(false);
            controller.ResumeGame();
        }

        public void OnMainMenuClick()
        {
            controller.ResumeGame();
            Application.LoadLevel("MainMenu");
        }

        public void OnMuteClick()
        {
            MuteImage.sprite = Resources.Load<Sprite>(controller.TogleSound() ? "GuiSprites/audioOn" : "GuiSprites/audioOff");
        }

        public void OnShowVideoClick()
        {
                AdManager.ShowAdColonyVideo();
        }
    }
}
