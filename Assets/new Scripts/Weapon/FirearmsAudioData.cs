using UnityEngine;

namespace new_Scripts.Weapon
{
    [CreateAssetMenu(menuName = "FPS/Firearms Audio Data")]
    public class FirearmsAudioData : ScriptableObject
    {
        public AudioClip ShootingAudioClip;
        public AudioClip ReloadOutOf;
        public AudioClip AimIn;
    }
}