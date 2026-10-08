using UnityEngine;

namespace _Game.Code.PersistenceProgress
{
    public class PrefsSaveLoadService : ISaveLoadService
    {
        public void SavePlayerData(PlayerData playerData)
        {
            PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(playerData));
        }

        public PlayerData LoadPlayerData()
        {
            string json = PlayerPrefs.GetString("PlayerData");

            return JsonUtility.FromJson<PlayerData>(json) ?? new PlayerData();
        }
    }
}