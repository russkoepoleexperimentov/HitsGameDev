using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.General
{

    public static class Saver
    {
        private const string SAVE_VAR_NAME = "currentLevel";

        // здесь пусть будут все уровни (!)в порядке прохождения игры (названия сцен)
        private static List<string> _gameLevels = new()
        {
            "first_level",
            "code_level",
            "third_level",
            "laser_level",
        };

        public static void OnNewGame()
        {
            if(PlayerPrefs.HasKey(SAVE_VAR_NAME)) 
                PlayerPrefs.DeleteKey(SAVE_VAR_NAME);
        }

        public static bool UpdateSave(string level)
        {
            if(_gameLevels.Contains(level))
            {
                if (!PlayerPrefs.HasKey(SAVE_VAR_NAME))
                {
                    PlayerPrefs.SetString(SAVE_VAR_NAME, level);
                    return true;
                }

                var lastName = PlayerPrefs.GetString(SAVE_VAR_NAME);

                if(_gameLevels.IndexOf(lastName) < _gameLevels.IndexOf(level))
                {
                    PlayerPrefs.SetString(SAVE_VAR_NAME, level);
                    return true;
                }
            }

            return false;
        }

        public static string GetSavedLevel() => PlayerPrefs.GetString(SAVE_VAR_NAME, _gameLevels[0]);

        public static bool HasSave() => PlayerPrefs.HasKey(SAVE_VAR_NAME);
    }
}
