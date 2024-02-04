using System;
using System.Collections.Generic;
using Code.DebugTools.Logger;
using GameAnalyticsSDK.Setup;
using UniRx;
using UnityEngine;

namespace Code.Projectiles
{
    public class GridPointSelector:IDisposable
    {
        public GridPointSelector(IObserver<(Vector2Int,Vector3)> onCursorInactive)
        {
            _playZoneCollider = GameObject.FindObjectOfType<PlayZone>().GetCollider;
            _camera = Camera.main;

            _onCursorInactive = onCursorInactive;
            
            _grid = GameObject.FindObjectOfType<Grid>();
            _aimCursor = GameObject.FindObjectOfType<AimCursor>();
            
            _onGetMouseButtonDownSubscription = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0) && _playZoneCollider.OverlapPoint(_camera.ScreenToWorldPoint(Input.mousePosition)))
                .Subscribe(_ => SetCursorActive());

            _onGetMouseButtonPressedSubscription = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButton(0))
                .Select(x =>
                {
                    _mouseGridPosition.Value = GetMouseGridCoords();
                    return (_mouseGridPosition.Value);
                })
                .Subscribe();

            _mouseGridPosition.Where(x => _isCursorActive && x!= _nonActivePosition)
                .Subscribe(x => _aimCursor.MoveToPosition(GetWorldPosition(x)));

            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonUp(0))
                .Subscribe(_ => SetCursorInactive());
            
            _mouseGridPosition.Where(x => _isCursorActive && x!= _nonActivePosition)
                .Buffer(_mouseGridPosition.Throttle(TimeSpan.FromMilliseconds(500f)))
                .Where(x => x.Count>=1)
                .Subscribe(x =>
                {
                    SetCursorInactive();
                });

        }

        private static readonly Vector2Int _nonActivePosition = new Vector2Int(-100, -100);

        private readonly IObserver<(Vector2Int gridCoords, Vector3 worldCoords)> _onCursorInactive; 

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

        private void SetCursorActive()
        {
            _isCursorActive = true;
            _mouseGridPosition.Value = GetMouseGridCoords();
            var worldPosition = GetWorldPosition(_mouseGridPosition.Value);
            _aimCursor.ShowOnPosition(worldPosition);
        }

        private void SetCursorInactive()
        {
            _isCursorActive = false;
            _aimCursor.Hide();
            if (_mouseGridPosition.Value != _nonActivePosition && 
                _playZoneCollider.OverlapPoint(_camera.ScreenToWorldPoint(Input.mousePosition)))
            {
                
                var worldPosition = GetWorldPosition(_mouseGridPosition.Value);
                _onCursorInactive.OnNext((_mouseGridPosition.Value, worldPosition));
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
        
        private Vector2Int GetMouseGridCoords()
        {
            Vector3 pointerPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            pointerPosition.z = 0f;
            //$"Позиция на сетке: {_grid.WorldToCell(pointerPosition).ToString()}".Colored(Color.cyan).Log();
            Vector3Int result = _grid.WorldToCell(pointerPosition);
            return new Vector2Int(result.x, result.y);
        }
        
    }
}