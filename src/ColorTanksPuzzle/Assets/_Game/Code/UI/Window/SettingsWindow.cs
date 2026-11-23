using System;
using _Game.Code.Infrastructure.SceneManagment;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI.Window
{
    public class SettingsWindow : UserInterfaceWindow
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Slider _soundsSlider;

        private SceneLoader _sceneLoader;
        
        private SettingsButton _settingsButton;

        public event Action<float> SoundsVolumeChanged;
        
        [Inject]
        public void Construct(SceneLoader sceneLoader, SettingsButton settingsButton)
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _settingsButton = settingsButton ?? throw new ArgumentNullException(nameof(settingsButton));

            _settingsButton.Clicked += Open;
        }

        private void OnDestroy()
        {
            _settingsButton.Clicked -= Open;
        }

        private void Open()
        {
            _closeButton.onClick.AddListener(Close);
            
            if (_menuButton != null)
                _menuButton.onClick.AddListener(BackToMenu);
            
            _soundsSlider.onValueChanged.AddListener(OnSoundsVolumeChanged);
            
            InvokeOpen();
        }

        private void Close()
        {
            _closeButton.onClick.RemoveListener(Close);
            
            if (_menuButton != null)
                _menuButton.onClick.RemoveListener(BackToMenu);
            
            _soundsSlider.onValueChanged.RemoveListener(OnSoundsVolumeChanged);
            
            InvokeClose();
        }

        private void BackToMenu()
        {
            Close();
            
            _sceneLoader.LoadMenuScene();
        }

        private void OnSoundsVolumeChanged(float value)
        {
            SoundsVolumeChanged?.Invoke(value);
            
            Debug.Log($"Sounds changed to {value}");
        }
    }
}