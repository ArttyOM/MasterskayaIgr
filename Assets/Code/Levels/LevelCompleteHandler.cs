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
            //Ignore
        }
    }
}