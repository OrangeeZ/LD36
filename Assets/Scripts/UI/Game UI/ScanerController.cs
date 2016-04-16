using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.Game_UI
{
    public class ScanerController : MonoBehaviour
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
        private AudioSource _audioSource;

        private Character _character; 
        private List<Character> _enemies = new List<Character>();
        private float _lastActivationSoundTime;

        public void Initialize()
        {
            _character = GameplayController.Instance.PlayerSpawner.character;
            StartCoroutine(ScanerDetectCoroutine());
        }

        public void UpdateScanerData()
        {
            _enemies.Clear();
            var nearestEnemyDistance = float.MaxValue;
            for (int i = 0; i < Character.Instances.Count; i++)
            {
                var charcter = Character.Instances[i];
                if(charcter == null || !charcter.Pawn.isActiveAndEnabled || 
                    !charcter.IsEnemy())continue;
                //todo add visual part of scaner
                _enemies.Add(charcter);
                var distance = Vector3.Distance(charcter.Pawn.position, _character.Pawn.position);
                if (distance < nearestEnemyDistance)
                {
                    nearestEnemyDistance = distance;
                }
            }
            var delta = nearestEnemyDistance/_detectRange;
            var soundVolume = Mathf.Lerp(1, 0, delta);
            var delay = Mathf.Lerp(_minSoundDelay, _maxSoundDelay, delta);
            PlaySound(soundVolume,delay);
        }

        //====private methods
        private void Start()
        {
            Debug.Log("Start Scaner;");
            Initialize();
            
        }

        private void PlaySound(float volume,float delay)
        {
            var lastSoundTime = Time.timeSinceLevelLoad - _lastActivationSoundTime;
            if (lastSoundTime < delay) return;
            _lastActivationSoundTime = Time.timeSinceLevelLoad;
            _audioSource.volume = volume;
            _audioSource.Play();
        }

        private void Update()
        {
            if (_character == null)
            {
                Initialize();
                return;
            }
        }

        private IEnumerator ScanerDetectCoroutine()
        {
            while (true)
            {
                Debug.Log("ScanerDetectCoroutine");
                UpdateScanerData();
                yield return _coolDownTime > 0 ? 
                    new WaitForSeconds(_coolDownTime) : null;
            }
        }
    }
}
