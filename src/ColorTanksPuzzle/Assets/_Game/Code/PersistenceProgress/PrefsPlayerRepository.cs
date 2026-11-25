using UnityEngine;

namespace _Game.Code.PersistenceProgress
{
    public class PrefsPlayerRepository : IPlayerRepository
    {
        public void SavePlayerData(PlayerData playerData)
        {
            PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(playerData));
            PlayerPrefs.Save();
        }

        public PlayerData LoadPlayerData()
        {
            string json = PlayerPrefs.GetString("PlayerData");

            return JsonUtility.FromJson<PlayerData>(json) ?? new PlayerData();
        }
    }
}