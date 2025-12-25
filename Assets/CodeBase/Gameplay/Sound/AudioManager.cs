using UnityEngine;
using Infrastructure;

namespace SpaceShooter
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {

        [SerializeField] private AudioClip m_MenuMusic;

        public void PlayMenuMusic()
        {
            GetComponent<AudioSource>().resource = m_MenuMusic;
            GetComponent<AudioSource>().Play();
        }

        public void PlayLevelMusic(LevelProperties props)
        {
            GetComponent<AudioSource>().resource = props.BackgroundMusic;
            GetComponent<AudioSource>().Play();
        }

    }
}