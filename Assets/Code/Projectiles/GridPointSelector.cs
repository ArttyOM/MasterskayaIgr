using System;
using System.Collections;
using Code.DebugTools.Logger;
using UniRx;
using UnityEngine;

namespace Code.Projectiles
{
    public class GridPointSelector:IDisposable
    {

        public GridPointSelector(AutofireConfig autofireConfig, IObserver<(Vector2Int, Vector3)> onDestinationSelected, IObservable<int> eventsOnSessionStart)
        {
            _playZoneCollider = GameObject.FindObjectOfType<PlayZone>().GetCollider;
            _grid = GameObject.FindObjectOfType<Grid>();
            _aimCursor = GameObject.FindObjectOfType<AimCursor>();
            _camera = Camera.main;
            _autofirePeriod = autofireConfig.periodInSeconds;
            _onDestinationSelected = onDestinationSelected;
            _onGetMouseButtonDownSubscription = Observable.EveryUpdate().SkipUntil(eventsOnSessionStart)
                .Where(_ => IsLeftMouseButtonDown() || IsTouchBegan())
                .Subscribe(_ =>
                {
                    MainThreadDispatcher.StartUpdateMicroCoroutine(DisplayAimAndSendFireEvent());
                });
        }

        private static readonly Vector2Int _nonActivePosition = new Vector2Int(-100, -100);

        private readonly IObserver<(Vector2Int gridCoords, Vector3 worldCoords)> _onDestinationSelected;
        private readonly IDisposable _onGetMouseButtonDownSubscription;
        private readonly Camera _camera;
        
        private readonly Grid _grid;
        private readonly Vector2IntReactiveProperty _mouseGridPosition= new(_nonActivePosition);
        private readonly AimCursor _aimCursor;
        private readonly Collider2D _playZoneCollider;
        private readonly float _autofirePeriod;
        
        private bool _isCursorActive = false;

        public void Dispose()
        {
            _onGetMouseButtonDownSubscription?.Dispose();
        }

        private IEnumerator DisplayAimAndSendFireEvent()
        {
            SetCursorActive();
            float secondsToFire = _autofirePeriod;
            Vector3 worldPosition = GetWorldPosition(_mouseGridPosition.Value);
            
            Fire(_mouseGridPosition.Value, worldPosition);
            
            while (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                secondsToFire -= Time.deltaTime;
                yield return null;
                _mouseGridPosition.Value = GetPointerGridCoords();

                if (IsCursorActiveAndInGamezone())
                {
                    worldPosition = GetWorldPosition(_mouseGridPosition.Value);
                    _aimCursor.MoveToPosition(worldPosition);
                        
                    if (secondsToFire <= 0)
                    {
                        Fire(_mouseGridPosition.Value, worldPosition);
                        secondsToFire = _autofirePeriod;
                    }
                }
            }

            if (IsCursorActiveAndInGamezone())
            {
                worldPosition = GetWorldPosition(_mouseGridPosition.Value);
                
                SetCursorInactive();
            }
        }

        private bool IsCursorActiveAndInGamezone()
        {
            return (_isCursorActive && (_mouseGridPosition.Value != _nonActivePosition) &&
                    _playZoneCollider.OverlapPoint(_camera.ScreenToWorldPoint(Input.mousePosition)));
        }

        private bool IsLeftMouseButtonDown()
        {
            return Input.GetMouseButtonDown(0)&& _playZoneCollider.OverlapPoint(_camera.ScreenToWorldPoint(Input.mousePosition));
        }
        
        private bool IsTouchBegan()
        {
            foreach (var touch in Input.touches)
            {
                if ((touch.phase == TouchPhase.Began) && _playZoneCollider.OverlapPoint(_camera.ScreenToWorldPoint(touch.position)))
                {
                    return true;
                }
            }
            return false;
        }
        
        private void SetCursorActive()
        {
            _mouseGridPosition.Value = GetPointerGridCoords();
            var worldPosition = GetWorldPosition(_mouseGridPosition.Value);
            if (_playZoneCollider.OverlapPoint(worldPosition))
            {
                _isCursorActive = true;
                _aimCursor.ShowOnPosition(worldPosition);
            }
        }

        private void SetCursorInactive()
        {
            _isCursorActive = false;
            _aimCursor.Hide();
            _mouseGridPosition.Value = _nonActivePosition;
        }

        private void Fire(Vector2Int gridPosition, Vector3 worldPosition)
        {
            ">>Fire".Log();
            _onDestinationSelected.OnNext((gridPosition, worldPosition));
        }
        

        private Vector3 GetWorldPosition(Vector2Int coords)
        {
            Vector3 position = _grid.GetCellCenterWorld(new Vector3Int(coords.x, coords.y, 0));
            position.x -= 0.5f;
            position.y -= 0.3f;
            
            return position;
        }
        
        private Vector2Int GetPointerGridCoords()
        {
            Vector2 inputPosition = _nonActivePosition;

            if (Input.touchCount == 0)
            {
                inputPosition = Input.mousePosition;
            }
            else
            {
                foreach (var touch in Input.touches)
                {
                    inputPosition = touch.position;
                    break;
                }
            }

            Vector3 pointerPosition = _camera.ScreenToWorldPoint(inputPosition);
            
            pointerPosition.z = 0f;
            Vector3Int result = _grid.WorldToCell(pointerPosition);
            return new Vector2Int(result.x, result.y);
        }
        
    }
}