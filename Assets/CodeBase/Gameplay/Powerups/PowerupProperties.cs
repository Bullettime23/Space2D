using UnityEngine;
using Common;
using System;

namespace SpaceShooter
{
    public enum PowerupType
    {
        Speedboost,
    }

    [CreateAssetMenu(fileName = "PowerupProperties", menuName = "Scriptable objects/Powerup properties")]
    public class PowerupProperties : ScriptableObject
    {
        [SerializeField] private float m_BonusSpeed;
        public float BonusSpeed => m_BonusSpeed;

        [SerializeField] private float m_Duration;
        public float Duration => m_Duration;

        [SerializeField] private PowerupType m_PowerupType;
        public PowerupType PowerupType => m_PowerupType;

        [SerializeField] private AudioClip m_AudioStart;
        public AudioClip AudioStart => m_AudioStart;

        [SerializeField] private AudioClip m_AudioEnd;
        public AudioClip AudioEnd => m_AudioEnd;

        [HideInInspector]
        public Timer Timer;
    }
}