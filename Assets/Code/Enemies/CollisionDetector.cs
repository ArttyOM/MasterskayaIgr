using System;
using Code.DebugTools.Logger;
using MyBox;
using UniRx;
using UniRx.Triggers;
using UnityEngine;


namespace Code.Enemies
{
    public class CollisionDetector : MonoBehaviour
    {
        private void Awake()
        {
            var trigger = GetComponent<ObservableCollision2DTrigger>();
            trigger.OnCollisionEnter2DAsObservable()
                .Subscribe(x =>
                {
                     $"OnCollisionEnter2D -> {x.collider.name}".Colored(Colors.aqua).Log();

                });
        }
        
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            $"OnCollisionEnter2D -> {other.collider.name}".Colored(Colors.aqua).Log();
        }
    }
}