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
            _wallet.Render(services.Profile.GetWallet(), services.DropRewardsService);
            var levelIndex = services.Profile.GetCurrentLevel();
            var currentLevel = services.LevelProgression.GetLevel(levelIndex);
            _reward.SetActive(false);
            //@todo: Need to Force VictoryScreen to use somewhat of a reward system
            //_rewardAmount.text = level.CoinsReward.ToString();
            _restartButton.Init(services.Events.OnLevelRestart, currentLevel.BuildIndex);
            _menuButton.Init(services.Events.OnMenu, currentLevel.BuildIndex);
            Debug.Log($"Change Level to {levelIndex} :{currentLevel.BuildIndex} {currentLevel.SceneName}");
        }
    }
}