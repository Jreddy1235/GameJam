using UnityEngine;

namespace BrilliantBingo.Code.Infrastructure.Models
{
    public class NumberAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public void PlayAudioClip(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.volume = 1;
            audioSource.PlayOneShot(audioClip);
        }
    }
}