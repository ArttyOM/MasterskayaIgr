using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Code.HUD.DamageNumbers
{
    public class DamageNumber : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _distance;
        [SerializeField] private TMP_Text _text;
        
        public event Action<DamageNumber> OnComplete;


        public void Drop(int amount, Vector2 screenPosition)
        {
            _text.text = amount.ToString();
            transform.position = screenPosition;
            transform.DOMoveY(transform.position.y + _distance, _duration).OnComplete(Complete);
        }
        
        

        private void Complete()
        {
            OnComplete?.Invoke(this);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }
    }
}