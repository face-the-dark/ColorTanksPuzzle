using UnityEditor;
using UnityEngine;

namespace _Game.Code.Editor
{
    public class PlayerPrefsWindow : EditorWindow
    {
        [MenuItem("Tools/PlayerPrefs Cleaner")]
        private static void OpenWindow()
        {
            GetWindow<PlayerPrefsWindow>("Prefs Cleaner");
        }

        private void OnGUI()
        {
            GUILayout.Label("PlayerPrefs Cleaner", EditorStyles.boldLabel);
            GUILayout.Space(10);

            if (GUILayout.Button("Clear All PlayerPrefs"))
            {
                if (EditorUtility.DisplayDialog("Подтверждение", "Удалить все PlayerPrefs?", "Удалить", "Отмена"))
                {
                    PlayerPrefs.DeleteAll();
                    PlayerPrefs.Save();
                    
                    Debug.Log("PlayerPrefs очищены!");
                }
            }
        }
    }
}