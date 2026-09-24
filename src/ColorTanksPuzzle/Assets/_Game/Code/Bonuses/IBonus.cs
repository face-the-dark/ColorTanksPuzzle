namespace _Game.Code.Bonuses
{
    public interface IBonus
    {
        Bonus Bonus { get; }
        void Activate();
    }
}