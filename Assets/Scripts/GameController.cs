using System.Collections.Generic;
using Assets.Scripts.GuiScripts;
using ChartboostSDK;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        public static GameController controller;

        //Inspector Values
        public float AimTime; //opimal time per probe for aiming
        public GameObject CometPrefab;

        public LaserBeam ActiveBeam { get; set; }
        public bool Gravity { get; set; }
        public Comet ActiveComet { get; private set; }
        public int ProbeChargedCounter { get; private set; }
        public float Score { get; private set; }
        public int Attempts { get; set; }
		public bool LevelPassed { get; private set; }

        public bool GamePaused { get; private set; }

        private Transform spawnPoint;
        private List<GameObject> probeList;
        private bool[] probesCharged;
		private GameGUI gameGui;
        private bool soundState;

        private void Awake()
        {
            controller = this;
        }

        private void Start()
        {
            spawnPoint = transform.GetChild(0);
			LevelPassed = false;

			gameGui = GameObject.FindGameObjectWithTag ("GUI").GetComponent<GameGUI> ();

            var probes = GameObject.FindGameObjectsWithTag("Probe");
            probeList = new List<GameObject>();
            foreach (var probe in probes)
            {
                probeList.Add(probe);
            }
            probesCharged=new bool[probeList.Count];

            Attempts = 0;
            Score = AimTime * (probeList.Count-1);

            soundState = AudioListener.pause;
        }

        void Update()
        {
            AudioListener.pause = soundState;

            if (LevelPassed) return;

			if (ActiveComet == null) 
			{
				if(ProbeChargedCounter == probeList.Count)
				{
					LevelPassed=true;
                    ShowWinScreen();
                    DataManager.dataManager.Save();
				}
			}
            else
	            if (ProbeChargedCounter != probeList.Count) 
				{
					Score = ActiveComet.Life;
				}
        }

        public void ShowWinScreen()
        {
            AdManager.ShowChartboostAd(CBLocation.LevelComplete);
            gameGui.ShowWinScreen();
        }

        public void ScreenClicked()
        {
			if (ActiveComet == null && !LevelPassed)
				Initialise ();
		}

        private void Initialise()
        {
            ProbeChargedCounter = 0;
			for (int i = 0; i < probeList.Count; ++i) 
			{
				probesCharged [i] = false;
			}
            Score = AimTime * (probeList.Count-1);
            Attempts += 1;
            Gravity = true;

            SpawnComet();
        }

        private void SpawnComet()
        {
            var comet = (GameObject)Instantiate(CometPrefab, spawnPoint.position, spawnPoint.rotation);
            ActiveComet = comet.GetComponent<Comet>();
        }

        public void CountProbe(GameObject probe)
        {
            var index = probeList.IndexOf(probe);
            if (probesCharged[index] == false)
            {
                ProbeChargedCounter += 1;
            }
            probesCharged[index] = true;
        }

		public int GetNumberOfProbes()
		{
			return probeList.Count;
		}

        public void PauseGame()
        {
            GamePaused = true;
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            GamePaused = false;
            Time.timeScale = 1;
        }

        public bool TogleSound()
        {
            AudioListener.pause = !AudioListener.pause;
            soundState = AudioListener.pause;
            return !AudioListener.pause;
        }

        
        void OnApplicationPause(bool _bool)
        {
            if (Application.loadedLevel == 0) return;
            if (_bool)
            {
                gameGui.OnMenuClick();
            }
        }
    }
}
