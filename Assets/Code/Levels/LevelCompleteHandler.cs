using Code.Events;
using Code.GameLoop;
using Code.InGameRewards;
using Code.Saves;
using UniRx;
using UnityEngine;

namespace Code.Levels
{
    public class LevelCompleteHandler
    {
        private readonly InGameEvents _events;
        private readonly LevelProgression _levelProgression;
        private readonly PlayerProfile _profile;
        private readonly DropRewards _dropRewards;

        public LevelCompleteHandler(InGameEvents events, LevelProgression levelProgression, PlayerProfile profile, DropRewards dropRewards)
        {
            _dropRewards = dropRewards;
            _events = events;
            _levelProgression = levelProgression;
            _profile = profile;
            _events.OnLevelEnd.Subscribe(ChangeLevelOnWin);
        }

        private void ChangeLevelOnWin(LevelEndResult result)
        {
            if (result == LevelEndResult.Win)
            {
                var levelIndex = _profile.GetCurrentLevel();
                var level = _levelProgression.GetLevel(levelIndex);
                var nextLevel = _levelProgression.GetNext(level);
                if (nextLevel < 0) return;
                _profile.SetCurrentLevel(nextLevel);
                if (_profile.IsLevelCompleted(levelIndex)) return;
                _dropRewards.DropCoins(level.CoinsReward, new Vector2(Screen.width/2f, Screen.height/2f));
                _profile.CompleteLevel(levelIndex);
            }
        }
    }
}