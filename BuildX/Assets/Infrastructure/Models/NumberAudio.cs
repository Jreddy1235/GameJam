using UniRx;
using UnityEngine;

namespace BrilliantBingo.Code.Infrastructure.Models
{
    public class NumberAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public void PlayAudioClip(AudioClip audioClip)
        {
            Observable.ReturnUnit()
                .DelayFrame(1)
                .Subscribe(_ =>
                {
                    audioSource.clip = audioClip;
                    audioSource.volume = 1;
                    audioSource.Play();
                });
        }
    }
}