using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashView : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text txtSlider;
    [SerializeField] private float duration;
    
    private void Awake()
    {
        Camera.main.aspect = 0.5625f;
        Application.targetFrameRate = 60;

        LeanTween.value(gameObject, x =>
        {
            slider.value = x;
            txtSlider.text = (int) (x * 100) + "%";
        }, 0, 1, duration).setOnComplete(() =>
        {
            SceneManager.LoadScene("Main");
        });
    }
}