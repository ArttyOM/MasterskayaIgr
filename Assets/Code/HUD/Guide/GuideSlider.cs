using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD.Guide
{
    public class GuideSlider : MonoBehaviour
    {
        [SerializeField] private Button _prev;
        [SerializeField] private Button _next;
        [SerializeField] private TMP_Text _pageCounter;
        
        private GuideSlide[] _slides;
        private int _activeSlide;

        private void Awake()
        {
            _prev.onClick.AddListener(PrevSlide);
            _next.onClick.AddListener(NextSlide);
            GetSlides();
            HideSlides();
            ActivateSlide(0);
        }

        private void GetSlides()
        {
            _slides = transform.GetComponentsInChildren<GuideSlide>(true);
        }

        private void HideSlides()
        {
            foreach (var slide in _slides)
            {
                slide.Hide();
            }
        }

        private void ActivateSlide(int i)
        {
            _slides[_activeSlide].Hide();
            _activeSlide = i;
            _slides[i].Show();
            _pageCounter.text = $"{i+1}/{_slides.Length}";
        }


        private void PrevSlide()
        {
            var slide =  (_slides.Length + _activeSlide - 1) % _slides.Length;
            ActivateSlide(slide);
        }

        private void NextSlide()
        {
            var slide =  (_activeSlide + 1) % _slides.Length;
            ActivateSlide(slide);
        }
    }
}