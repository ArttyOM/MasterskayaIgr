using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Code.DebugTools.Logger;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening.Plugins;
using GameAnalyticsSDK.Setup;
using UniRx;
using UniRx.Diagnostics;
using UnityEditor.ShaderKeywordFilter;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Projectiles
{
    public class GridPointSelector:IDisposable
    {
        private const string TOUCH = "Fire1";
        
        public GridPointSelector(IObserver<(Vector2Int,Vector3)> onDestinationSelected)
        {
            _playZoneCollider = GameObject.FindObjectOfType<PlayZone>().GetCollider;
            _camera = Camera.main;

            _onDestinationSelected = onDestinationSelected;
            
            _grid = GameObject.FindObjectOfType<Grid>();
            _aimCursor = GameObject.FindObjectOfType<AimCursor>();

            var onMouseButtonPressed = Observable.EveryUpdate()
                .Where(_ => IsLeftMouseButtonDown() || IsTouchBegan());
            _onGetMouseButtonDownSubscription = onMouseButtonPressed
                .Subscribe(_ => SetCursorActive());

            _onGetMouseButtonPressedSubscription = Observable.EveryUpdate()
                .Where(_ => Input.touchCount>0 || Input.GetMouseButton(0))
                .Subscribe(x =>
                {
                    _mouseGridPosition.Value = GetPointerGridCoords();
                });

            _mouseGridPosition.Where(x => _isCursorActive && x!= _nonActivePosition)
                .Subscribe(x => _aimCursor.MoveToPosition(GetWorldPosition(x)));

            // Observable.EveryUpdate()
            //     .Where(_ => Input.GetButtonUp(TOUCH) ||IsTouchPhaseEnded())
            //     .Subscribe(_ =>
            //     {
            //         //"OnButtonUp".Log();
            //         SetCursorInactive();
            //     });
            
            // _mouseGridPosition.Where(x => _isCursorActive && x!= _nonActivePosition)
            //     .Buffer(_mouseGridPosition.Throttle(TimeSpan.FromMilliseconds(500f)))
            //     .Where(x => x.Count>=1)
            //     .Subscribe(x =>
            //     {
            //         SetCursorInactive();
            //     });

            "<<new GridPointSelector".Colored(Color.red).Log();
        }

        private static readonly Vector2Int _nonActivePosition = new Vector2Int(-100, -100);

        private readonly IObserver<(Vector2Int gridCoords, Vector3 worldCoords)> _onDestinationSelected; 

        private readonly IDisposable _onGetMouseButtonUpSubscription;
        private readonly IDisposable _onGetMouseButtonDownSubscription;
        private readonly IDisposable _onGetMouseButtonPressedSubscription;
        private readonly Grid _grid;

        private readonly Vector2IntReactiveProperty _mouseGridPosition= new(_nonActivePosition);
        
        private readonly AimCursor _aimCursor;

        private readonly Collider2D _playZoneCollider;

        private bool _isCursorActive = false;

        private readonly Camera _camera;

        public void Dispose()
        {
           _onGetMouseButtonUpSubscription?.Dispose();
           _onGetMouseButtonPressedSubscription?.Dispose();
           _onGetMouseButtonDownSubscription?.Dispose();
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

        private bool IsTouchMoved()
        {
            foreach (var touch in Input.touches)
            {
                if (touch.phase==TouchPhase.Moved) return true;
                    //if (_touch.phase != TouchPhase.Moved) return false;
                    //if (!_playZoneCollider.OverlapPoint(_camera.ScreenToWorldPoint(_touch.position))) return false;
            }
            return false;
        }
        
        

        private void SetCursorActive()
        {
            ">>SetCursorActive".Log();
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
            if (_mouseGridPosition.Value != _nonActivePosition && 
                _playZoneCollider.OverlapPoint(_camera.ScreenToWorldPoint(Input.mousePosition)))
            {
                
                var worldPosition = GetWorldPosition(_mouseGridPosition.Value);
                _onDestinationSelected.OnNext((_mouseGridPosition.Value, worldPosition));
                $"_mouseGridPosition.Value = {_mouseGridPosition.Value}, GetWorldPosition(_mouseGridPosition.Value))= {worldPosition}".Colored(Color.cyan).Log();

            }
 
            _mouseGridPosition.Value = _nonActivePosition;
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