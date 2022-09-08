using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayView : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollGamePlay;
    [SerializeField] private float scrollTime;

    private bool _isTop = true;
    private bool _inTransition;

    [Button]
    public void GoToTop()
    {
        if (_isTop || _inTransition) return;
        _inTransition = true;
        LeanTween.value(gameObject, (x) => scrollGamePlay.value = x, 0, 1, scrollTime)
            .setOnComplete(_ =>
            {
                _isTop = true;
                _inTransition = false;
            });
    }

    [Button]
    public void GoToBottom()
    {
        if (!_isTop || _inTransition) return;
        _inTransition = true;
        LeanTween.value(gameObject, (x) => scrollGamePlay.value = x, 1, 0, scrollTime).setOnComplete(_ =>
        {
            _isTop = false;
            _inTransition = false;
        });
    }
}