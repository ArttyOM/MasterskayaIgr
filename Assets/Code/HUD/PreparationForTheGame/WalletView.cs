using Code.InGameRewards;
using Code.Items;
using TMPro;
using UnityEngine;

namespace Code.HUD
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _balance;
        private Wallet _wallet;

        public void Render(Wallet wallet, DropRewards dropRewards)
        {
            UnSubscribe();

            dropRewards.SetDestination(transform.position);
            _wallet = wallet;
            UpdateBalance();
            Subscribe();
        }

        private void Subscribe()
        {
            if (_wallet != null)
            {
                _wallet.BalanceChanged += UpdateBalance;
            }
        }

        private void UnSubscribe()
        {
            if (_wallet != null)
            {
                _wallet.BalanceChanged -= UpdateBalance;
            }
        }

        private void UpdateBalance()
        {
            _balance.text = _wallet == null ? "" : _wallet.Balance.ToString();
        }

        private void OnDestroy()
        {
            UnSubscribe();
        }
    }
}