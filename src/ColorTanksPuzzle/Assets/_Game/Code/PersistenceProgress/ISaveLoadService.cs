namespace _Game.Code.PersistenceProgress
{
    public interface ISaveLoadService
    {
        void SavePlayerData(PlayerData playerData);
        PlayerData LoadPlayerData();
    }
}