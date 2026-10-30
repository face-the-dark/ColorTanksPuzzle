using System.Collections.Generic;
using _Game.Code.UI;
using _Game.Code.UI.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Game.Code.Infrastructure.LifetimeScopes
{
    public class MenuLifetimeScope : LifetimeScope
    {
        [SerializeField] private List<DifficultyButton> _difficultyButtons;
        [SerializeField] private CoinsView _coinsView;
        [SerializeField] private List<LevelButton> _levelButtons;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_coinsView);
            
            builder.RegisterBuildCallback(container =>
            {
                foreach (DifficultyButton button in _difficultyButtons)
                    container.Inject(button);
                
                foreach (LevelButton button in _levelButtons)
                    container.Inject(button);
            });
        }
    }
}