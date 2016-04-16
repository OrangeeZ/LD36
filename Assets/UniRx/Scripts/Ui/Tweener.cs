using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UI.uGui.Animations
{
    public enum TweenMode
    {
        PingPong,
        Forward
    }

    public class Tweener : MonoBehaviour, ITweener
    {
        //====priavte properties
        [SerializeField]
        protected float _animationTime;
        [SerializeField]
        protected float _animationUpdatePeriod;
        [SerializeField]
        private bool _loop = true;
        [SerializeField]
        private bool _startOnAwake = true;

        protected float _animationProgressTime;
        protected float _startAnimationUpdateTime;
        protected Coroutine _coroutine;

        //===events
        public UnityEvent AnimaitonFinished = new UnityEvent();

        //=====public methods
        public virtual void StartAnimation()
        {
            StopAnimation();
            Initialize();
            _coroutine = StartCoroutine(AnimationCoroutine());
        }

        public virtual void StopAnimation()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            OnAnimationStop();
            AnimaitonFinished.Invoke();
        }

        // Update is called once per frame
        protected virtual IEnumerator AnimationCoroutine()
        {
            while (true)
            {
                while (true)
                {
                    var stopAnimation = AnimationUpdate();
                    if (stopAnimation)
                    {
                        OnAnimationFinished();
                        if (!_loop)
                            yield break;
                        Initialize();
                    }
                    yield return _animationUpdatePeriod <= 0 ?
                        null : new WaitForSeconds(_animationUpdatePeriod);
                }
            }
        }


        protected virtual bool AnimationUpdate()
        {
            _animationProgressTime = Time.timeSinceLevelLoad - _startAnimationUpdateTime;
            return true;
        }

        protected virtual void OnAnimationFinished()
        {
            OnAnimationStop();
        }

        protected virtual void Initialize()
        {
            _animationProgressTime = 0;
            _startAnimationUpdateTime = Time.timeSinceLevelLoad;
        }

        protected virtual void OnAnimationStop()
        {

        }

        protected virtual void OnEnable()
        {
            if (_startOnAwake)
            {
               StartAnimation(); 
            }
        }

        protected virtual void Awake()
        {

        }

    }
}
