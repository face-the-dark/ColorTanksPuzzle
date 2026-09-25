using System;

namespace _Game.Code.Bonuses
{
    public interface IBonus
    {
        event Action<IBonus> Activated;
        
        Bonus Bonus { get; }
        void Activate();
        void Block();
        void Unblock();
    }
}