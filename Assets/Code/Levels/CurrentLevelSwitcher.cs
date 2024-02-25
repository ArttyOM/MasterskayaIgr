using Code.Events;
using Code.GameLoop;
using Code.Saves;
using UniRx;

namespace Code.Levels
{
    public class CurrentLevelSwitcher
    {
        private readonly InGameEvents _events;
        private readonly LevelProgression _levelProgression;
        private readonly PlayerProfile _profile;

        public CurrentLevelSwitcher(InGameEvents events, LevelProgression levelProgression, PlayerProfile profile)
        {
            _events = events;
            _levelProgression = levelProgression;
            _profile = profile;
            _events.OnLevelEnd.Subscribe(ChangeLevelOnWin);
        }

        private void ChangeLevelOnWin(LevelEndResult result)
        {
            if (result == LevelEndResult.Win)
            {
                var level = _levelProgression.GetLevel(_profile.GetCurrentLevel());
                var nextLevel = _levelProgression.GetNext(level);
                if (nextLevel < 0) return;
                _profile.SetCurrentLevel(nextLevel);
            }
        }
    }
}