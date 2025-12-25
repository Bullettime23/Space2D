using UnityEngine;

namespace SpaceShooter
{
    [CreateAssetMenu(fileName = "LevelProperties", menuName = "Scriptable objects/LevelProperties")]
    public class LevelProperties : ScriptableObject
    {
        [SerializeField] string m_Title;
        [SerializeField] string m_SceneName;
        [TextAreaAttribute(10,10)]
        [SerializeField] string m_SceneDescription;
        [SerializeField] Sprite m_PreviewImage;
        [SerializeField] LevelProperties m_NextLevel;
        [SerializeField] AudioClip m_BackgroundMusic;

        public string Title => m_Title;
        public string SceneName => m_SceneName;
        public string SceneDescription => m_SceneDescription;
        public Sprite PrevieImage => m_PreviewImage;
        public LevelProperties NextLevel => m_NextLevel;
        public AudioClip BackgroundMusic => m_BackgroundMusic;
    }
}