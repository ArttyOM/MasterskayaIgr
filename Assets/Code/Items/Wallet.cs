using System;

namespace Code.Items
{
    public class Wallet
    {
        private int _coins;
        public event Action BalanceChanged;

        public Wallet(int coins)
        {
            _coins = coins;
        }

        public int Balance => _coins;

        public bool TrySpend(int amount)
        {
            if (_coins < amount) return false;
            _coins -= amount;
            BalanceChanged?.Invoke();
            return true;
        }

        public void Add(int amount)
        {
            _coins += amount;
            BalanceChanged?.Invoke();
        }
    }
}