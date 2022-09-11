    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class SoundButton : MonoBehaviour
    {
        [SerializeField] private AudioType _audioType;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Play);
        }

        private void Play()
        {
            SoundManager.Play(_audioType);
        }
    }
