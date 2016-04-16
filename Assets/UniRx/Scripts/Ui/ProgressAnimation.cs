using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.uGui.Animations
{
    public class ProgressAnimation : Tweener
    {
        [SerializeField]
        private Text _timer;
        [SerializeField]
        private Image _image;
        private float _startValue;
        private float _endValue;

        //=====public methods

        public virtual void StartAnimation(float startValue, float endValue,float time)
        {
            _startValue = startValue;
            _endValue = endValue;
            _animationTime = time;
            StopAnimation();
            _animationProgressTime = 0;
            _startAnimationUpdateTime = Time.timeSinceLevelLoad;
            _coroutine = StartCoroutine(AnimationCoroutine());
        }

        //=====private methods

        protected override void OnAnimationStop()
        {

        }

        protected override bool AnimationUpdate()
        {
            base.AnimationUpdate();
            var fillAmount = Mathf.Lerp(_startValue, _endValue, _animationProgressTime/_animationTime);
            if (_timer)
            {
                var timerValue = Math.Round(Mathf.Max(_animationTime - _animationProgressTime, 0f));
                _timer.text = timerValue > 0 ? string.Format("{0}", timerValue) : string.Empty;
            }
            _image.fillAmount = fillAmount;
            return Mathf.Approximately(fillAmount,_endValue);
        }

        protected override void Awake()
        {
            if(_image==null)
                _image = gameObject.GetComponent<Image>();
            _image.type = Image.Type.Filled;
            base.Awake();
        }
    }
}
