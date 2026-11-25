namespace _Game.Code.PersistenceProgress
{
    public interface IPlayerRepository
    {
        void SavePlayerData(PlayerData playerData);
        PlayerData LoadPlayerData();
    }
}