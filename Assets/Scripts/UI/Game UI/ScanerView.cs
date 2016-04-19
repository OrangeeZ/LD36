using System;
using System.Collections.Generic;
using Packages.EventSystem;
using UI.uGui.Animations;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI.Game_UI
{
    public enum ScanerState
    {
        Warning,
        Clear,
        NoSignal,
        Ready,
        Disable,
        Active
    }
    public class ScanerView : MonoBehaviour
    {
        [SerializeField]
        private float _detectRange = 10f;
        [SerializeField]
        private float _coolDownTime = 1;
        [SerializeField]
        private float _minSoundDelay = 0.01f;
        [SerializeField]
        private float _maxSoundDelay = 3f;
        [SerializeField]
        private float _duration = 10f;
        [SerializeField]
        private float _maxDistance = 10f;
        [SerializeField]
        private float _reloadDuration = 10f;
        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private ProgressAnimation _animation;
        [SerializeField]
        private List<GameObject> _enemiesMarkers = new List<GameObject>(); 

        private Character _character;
        private List<Character> _enemies = new List<Character>();
        private float _lastActivationSoundTime;
        private float _lasActivateTime;
        private const string _warningState = "Warning";
        private const string _disableState = "Disable";
        private const string _enableState = "Ready";
        private const string _noSignalState = "NoSignal";
        private const string _clearState = "Clear";
        private ScanerState _scanerState = ScanerState.Ready;

        public void Initialize(Character character)
        {
            _character = character;
            _lasActivateTime = -_reloadDuration;
            _animation.AnimaitonFinished.AddListener(OnCooldownPassed);
            EventSystem.Events.SubscribeOfType<Room.EveryoneDied>(OnEveryoneDieInRoom);
            SetState(ScanerState.Ready);
        }

        public void Activate()
        {
            Debug.Log("Try Activate Scaner");
            if (_scanerState == ScanerState.Active || _scanerState == ScanerState.NoSignal) return;
            var time = Time.timeSinceLevelLoad - _lasActivateTime;
            if (_reloadDuration > time) return;
            Debug.Log("Launch Scaner");
            _lasActivateTime = Time.timeSinceLevelLoad;
            SetState(ScanerState.Active);
        }

        public void UpdateScanerData()
        {
            if(_scanerState == ScanerState.NoSignal)return;
            _enemies.Clear();
            var nearestEnemyDistance = float.MaxValue;
            for (int i = 0; i < Character.Instances.Count; i++)
            {
                var charcter = Character.Instances[i];
                if (charcter == null || !charcter.IsEnemy()) continue;
                //todo add visual part of scaner
                
                var distance = Vector3.Distance(charcter.Pawn.position, _character.Pawn.position);
                if (distance < _maxDistance)
                {
                    if(distance < nearestEnemyDistance)
                        nearestEnemyDistance = distance;
                    _enemies.Add(charcter);
                }
            }
            if (_enemies.Count > 0)
            {
                SetState(ScanerState.Warning);
                SetEnemyMarkersState(_enemies.Count, true);
            }
            else
            {
                SetState(ScanerState.Clear);
            }
            var delta = nearestEnemyDistance / _detectRange;
            var soundVolume = Mathf.Lerp(1, 0, delta);
            var delay = Mathf.Lerp(_minSoundDelay, _maxSoundDelay, delta);
            PlaySound(soundVolume, delay);
        }

        //====private methods

        private void SetEnemyMarkersState(int count,bool enable)
        {
            for (int i = 0; i < Math.Min(count,_enemiesMarkers.Count); i++)
            {
                _enemiesMarkers[i].SetActive(enable);
            }
        }

        private void PlaySound(float volume, float delay)
        {
            var lastSoundTime = Time.timeSinceLevelLoad - _lastActivationSoundTime;
            if (lastSoundTime < delay) return;
            _lastActivationSoundTime = Time.timeSinceLevelLoad;
            _audioSource.volume = volume;
            _audioSource.Play();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Activate();
            }
            if (_scanerState != ScanerState.NoSignal 
                && _scanerState != ScanerState.Disable 
                && _scanerState != ScanerState.Ready)
            {
                var time = Time.timeSinceLevelLoad - _lasActivateTime;
                var state = _duration > time;
                if (!state)
                {
                    SetState(ScanerState.Disable);
                    return;
                }
                UpdateScanerData();
            }
        }

        private void SetState(ScanerState state)
        {
            if (_scanerState == state || _scanerState == ScanerState.NoSignal) return;
            _scanerState = state;
            switch (_scanerState)
            {
                case ScanerState.Active:

                    break;
                case ScanerState.Ready:
                    _animator.SetTrigger(_enableState);
                    break;
                case ScanerState.Clear:
                    SetEnemyMarkersState(_enemies.Count, false);
                    _animator.SetTrigger(_clearState);
                    break;
                case ScanerState.NoSignal:
                    _animator.SetTrigger(_noSignalState);
                    break;
                case ScanerState.Warning: 
                    _animator.SetTrigger(_warningState);
                    break;
                case ScanerState.Disable:
                    _animation.StartAnimation(0, 1, _reloadDuration);
                    _animator.SetTrigger(_disableState);
                    break;
            };
               
        }

        private void OnCooldownPassed()
        {
	        if ( _scanerState == ScanerState.Active ) {

				SetState( ScanerState.Disable );
	        } else if ( _scanerState == ScanerState.Disable ) {
		        
				SetState( ScanerState.Ready );
	        }
        }

        private void OnEveryoneDieInRoom(Room.EveryoneDied everyoneDiedEvent)
        {
            var room = everyoneDiedEvent.Room;
            if (room.GetRoomType() != Room.RoomType.SecurityRoom) return;
            SetState(ScanerState.NoSignal);
        }
    }
}
