using System;
using UnityEngine;

namespace PointClear.Combat
{
    /// <summary>
    /// Concrete health/damage component shared by the player and enemies.
    /// No interface — only two consumers exist and both use it identically.
    /// </summary>
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private float maxHealth = 100f;

        public float CurrentHealth { get; private set; }
        public float MaxHealth => maxHealth;
        public bool IsDead => CurrentHealth <= 0f;

        public event Action Died;
        public event Action<float> Damaged;

        private void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (IsDead || amount <= 0f)
            {
                return;
            }

            CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
            Damaged?.Invoke(amount);

            if (CurrentHealth <= 0f)
            {
                Died?.Invoke();
            }
        }

        public void ResetHealth()
        {
            CurrentHealth = maxHealth;
        }
    }
}
