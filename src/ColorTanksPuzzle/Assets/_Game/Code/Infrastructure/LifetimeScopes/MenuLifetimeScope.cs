using System.Collections.Generic;
using _Game.Code.UI;
using _Game.Code.UI.View;
using _Game.Code.UI.Window;
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
        [SerializeField] private SettingsButton _settingsButton;
        [SerializeField] private SettingsWindow _settingsWindow;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_coinsView);
            builder.RegisterComponent(_settingsButton);
            builder.RegisterComponent(_settingsWindow);
            
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