using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UniRx.Scripts.Ui
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField]
        private Text _text;

        private float _previousHealth;
        private float _currentHealth;
        Character _character;

        public void Initialize(Character character)
        {
            _character = character;
        }

        private void SetHealth(float healthValue)
        {
            if (Mathf.Approximately(healthValue,_currentHealth))
                return;
            _previousHealth = _currentHealth;
            _currentHealth = healthValue;
            var heathProc = Math.Round(healthValue/ _character.Info.statusInfo.MaxHealth * 100);
            _text.text = string.Format("{0}%", heathProc);
        }

        private void Update()
        {
            if (_character == null) return;
            SetHealth(_character.Health.Value);
        }
    }
}
