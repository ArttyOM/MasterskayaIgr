using System;
using UnityEngine;

namespace Code.InGameRewards
{
    public class TestDropCoins : MonoBehaviour
    {
        [SerializeField] private DropRewards _rewardsDrop;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                _rewardsDrop.DropCoins(5, Input.mousePosition);
            }
        }
    }
}