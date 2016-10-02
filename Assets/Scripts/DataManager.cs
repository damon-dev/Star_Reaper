using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager dataManager;
        private const string SavesName = "/comet_saves.sav";
        private DataContainer container;
        private const int TotalLevels = 25; //Change this on each added level

        void Awake()
        {
            if (dataManager == null)
            {
                DontDestroyOnLoad(gameObject);
                dataManager = this;
            }
            else if (dataManager != this)
            {
                Destroy(gameObject);
            }

            container = new DataContainer();

            if (Load()) return;

            container.levels = new LevelInfo[TotalLevels];
        }

        public void Save()
        {
            var bf = new BinaryFormatter();
            FileStream file;
            if (File.Exists(Application.persistentDataPath + SavesName))
                file = File.Open(Application.persistentDataPath + SavesName, FileMode.Open);
            else
                file = File.Create(Application.persistentDataPath + SavesName);
            var c = GameController.controller;

            Actualize(Application.loadedLevel, c.Attempts, c.Score, true, true);
            container.levels[Application.loadedLevel+1].Unlocked = true;

            bf.Serialize(file, container);
            file.Close();
        }

        private bool Load()
        {
            if (File.Exists(Application.persistentDataPath + SavesName))
            {
                var bf = new BinaryFormatter();
                var file = File.Open(Application.persistentDataPath + SavesName, FileMode.Open);

                container = (DataContainer) bf.Deserialize(file);
                file.Close();
                return true;
            }
            return false;
        }

        public void ReportInfo(int level, ref int a, ref float s, ref bool p, ref bool u)
        {
            a = container.levels[level].Attemts;
            s = container.levels[level].Score;
            p = container.levels[level].Passed;
            u = container.levels[level].Unlocked;
        }

        public void ReportInfo(int level, ref int a, ref float s)
        {
            a = container.levels[level].Attemts;
            s = container.levels[level].Score;
        }

        private void Actualize(int level, int a, float s, bool p, bool u)
        {
            if (container.levels[level].Score < s)
            {
                container.levels[level].Attemts = a;
                container.levels[level].Score = s;
            }
            else if (container.levels[level].Attemts > a && Math.Abs(container.levels[level].Score - s) < 0.1f)
            {
                container.levels[level].Attemts = a;
            }

            container.levels[level].Passed = p;
            container.levels[level].Unlocked = u;

        }
    }

    [Serializable]
    internal class DataContainer
    {
        public LevelInfo[] levels;
    }

    [Serializable]
    internal struct LevelInfo
    {
        public int Attemts;
        public float Score;
        public bool Passed;
        public bool Unlocked;
    }
}
