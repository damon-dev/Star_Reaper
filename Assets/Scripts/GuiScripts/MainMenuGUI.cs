using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GuiScripts
{
    public class MainMenuGUI : MonoBehaviour
    {
        public GameObject Category;
        public GameObject Levels;
        public Image MuteImage;

        private List<Text> levelCounter;
        private List<Button> levelButtons;
        private bool soundState;
        private int category;

        void Start()
        {
            levelButtons=new List<Button>();
            levelCounter=new List<Text>();
            foreach (Transform t in Levels.transform)
            {
                levelButtons.Add(t.GetComponent<Button>());
            }
            foreach (Transform t in Category.transform)
            {
                levelCounter.Add(t.FindChild("Down").GetComponent<Text>());
            }
            UpdateCatStats();

            if(AudioSingleton.BackAudio!=null) Destroy(AudioSingleton.BackAudio.gameObject);

            MuteImage.sprite = Resources.Load<Sprite>(!AudioListener.pause ? "GuiSprites/audioOn" : "GuiSprites/audioOff");
            soundState = AudioListener.pause;
        }

        private void UpdateCatStats()
        {
            for (int j = 0; j < 4; j++)
            {
                int count = 0;
                for (int i = 0; i < 6; ++i)
                {
                    float s = 0;
                    int a = 0;
                    bool p = false, u = false;
                    DataManager.dataManager.ReportInfo(6*j+i+1, ref a, ref s, ref p, ref u);
                    if (p) count += 1;
                }
                levelCounter[j].text = count + "/6";
            }
        }

        void Update()
        {
            AudioListener.pause = soundState;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(Category.activeInHierarchy)
                    Application.Quit();
                else if (Levels.activeInHierarchy)
                {
                    Levels.SetActive(false);
                    Category.SetActive(true);
                    UpdateCatStats();
                }
            }
        }


        public void OnCatClick(int cat)
        {
            Category.SetActive(false);
            Levels.SetActive(true);

            category = cat;

            for (int i = 0; i < levelButtons.Count; ++i)
            {
                int attempts = 0;
                float score = 0;
                bool passed = false, unlocked = false;
                DataManager.dataManager.ReportInfo(cat + i + 1, ref attempts, ref score, ref passed, ref unlocked);

                if (passed)
                {
                    levelButtons[i].GetComponentInChildren<Text>().text =
                        string.Format((cat + i + 1) + "\nA:{0}\nS:{1,0:#0.0}", attempts, score);
                }
                else
                {
                    levelButtons[i].GetComponentInChildren<Text>().text =
                        (cat + i + 1).ToString(CultureInfo.InvariantCulture);
                }

                levelButtons[i].interactable = unlocked || cat + i + 1 == 1;
            }
        }

        public void OnLevelClick(int level)
        {
            Application.LoadLevel(category + level + 1);
        }

        public void OnMuteClick()
        {
            AudioListener.pause = !AudioListener.pause;
            MuteImage.sprite = Resources.Load<Sprite>(!AudioListener.pause ? "GuiSprites/audioOn" : "GuiSprites/audioOff");
            soundState = AudioListener.pause;
        }
    }
}
