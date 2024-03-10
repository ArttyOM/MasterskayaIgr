using System;
using System.Threading.Tasks;
using Code.Main;
using TMPro;
using UnityEngine;

namespace Code.HUD.Gameplay
{
    public class VictoryScreen : MonoBehaviour
    {
        [SerializeField] private RestartButton _restartButton;
        [SerializeField] private ToMenuButton _menuButton;
        [SerializeField] private TMP_Text _rewardAmount;
        [SerializeField] private GameObject _reward;
        [SerializeField] private WalletView _wallet;
        
        private async void OnEnable()
        {
            while (ServiceLocator.Instance == null)
            {
                await Task.Yield();
            }
            var services = ServiceLocator.Instance;
            var level = services.LevelProgression.GetLevel(services.Profile.GetCurrentLevel());
            _wallet.Render(services.Profile.GetWallet(), services.DropRewardsService);
            _restartButton.Init(services.Events.OnLevelRestart, level.BuildIndex);
            _menuButton.Init(services.Events.OnMenu, level.BuildIndex);
            _reward.SetActive(services.Profile.IsLevelCompleted(services.Profile.GetCurrentLevel()) == false);
            _rewardAmount.text = level.CoinsReward.ToString();
        }
    }
}