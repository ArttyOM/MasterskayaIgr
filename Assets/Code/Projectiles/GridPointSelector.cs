using System;
using System.Collections.Generic;
using Code.DebugTools.Logger;
using UniRx;
using UnityEngine;

namespace Code.Projectiles
{
    public class GridPointSelector:IDisposable
    {
        public GridPointSelector()
        {
            _grid = GameObject.FindObjectOfType<Grid>();
            _aimCursor = GameObject.FindObjectOfType<AimCursor>();

            var onMouseButtonUpEveryUpdate = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonUp(0));
            _onGetMouseButtonUpSubscription = onMouseButtonUpEveryUpdate
                .Subscribe(_ =>
                {
                    " _aimCursor.Hide()".Log();
                    _aimCursor.Hide();
                });

            var onMouseButtonPressedEveryUpdate = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButton(0))
                .Select(x =>
                {
                    var coords = GetGridCoords();
                    ShowPreview(coords);
                    return (coords, x);
                });

            _onGetMouseButtonPressedSubscription = onMouseButtonPressedEveryUpdate
                 .Buffer( onMouseButtonPressedEveryUpdate.Throttle(TimeSpan.FromMilliseconds(500f)))
            .Subscribe(onNext: x =>
            {
                if (IsGridCoordsSame(x))
                {
                    _aimCursor.Hide();
                    "timeout прицеливания".Colored(Color.yellow).Log();
                }
            });
        }

        private readonly IDisposable _onGetMouseButtonUpSubscription;
        private readonly IDisposable _onGetMouseButtonPressedSubscription;
        private readonly Grid _grid;

        private readonly AimCursor _aimCursor;
        
        public void Dispose()
        {
           _onGetMouseButtonUpSubscription?.Dispose();
           _onGetMouseButtonPressedSubscription?.Dispose();
        }

        private bool IsGridCoordsSame(IList<(Vector3Int, long)> coords)
        {
            if (coords is null || coords.Count <= 0) return false;
            int maxIndex = coords.Count - 1;
            long framesLeft = coords[maxIndex].Item2 - coords[0].Item2;
             $"framesLeft ={framesLeft}, enumLength = {maxIndex}".Log();
            if (framesLeft > maxIndex) return false;
            // $"coords.Count ={coords.Count}".Log();
            // $"coords[0].Item2 ={coords[0].Item2}".Log();
            // $"coords[coords.Count-1].Item2 ={(coords[coords.Count-1].Item2)}".Log();
            // $"dif ={ (coords[coords.Count-1].Item2)-coords[0].Item2}".Log();
            
            Vector3Int firstCoord = coords[0].Item1;
            foreach (var coord in coords)
            {
                if (coord.Item1 != firstCoord) return false;
            }

            return true;
        }
        
        private void ShowPreview(Vector3Int coords)
        {
            Vector3 position = _grid.GetCellCenterWorld(coords);
            position.x -= 0.5f;
            position.y -= 0.3f;

            _aimCursor.ShowOnPosition(position);
        }
        
        private Vector3Int GetGridCoords()
        {
            Vector3 pointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pointerPosition.z = 0f;
            //$"Позиция на сетке: {_grid.WorldToCell(pointerPosition).ToString()}".Colored(Color.cyan).Log();
            
            return _grid.WorldToCell(pointerPosition);
        }
        
    }
}