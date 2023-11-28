using JetBrains.Annotations;
using UnityEngine;

namespace Components.CharacterControllers
{
    public class PlayerAudioAnimationEvents : MonoBehaviour
    {
        public CharacterController CharacterController;
        
        [Header("Audio")]
        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
        
        [UsedImplicitly]
        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], CharacterController.transform.TransformPoint(CharacterController.center), FootstepAudioVolume);
                }
            }
        }
        
        [UsedImplicitly]
        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, CharacterController.transform.TransformPoint(CharacterController.center), FootstepAudioVolume);
            }
        }
    }
}