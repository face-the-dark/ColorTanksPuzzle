using System.Collections.Generic;
using _Game.Code.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Game.Code.Infrastructure.LifetimeScopes
{
    public class MenuLifetimeScope : LifetimeScope
    {
        [SerializeField] private List<DifficultyButton> _levelButtons;
        [SerializeField] private CoinsView _coinsView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_coinsView);
            
            builder.RegisterBuildCallback(container =>
            {
                foreach (DifficultyButton button in _levelButtons)
                    container.Inject(button);
            });
        }
    }
}