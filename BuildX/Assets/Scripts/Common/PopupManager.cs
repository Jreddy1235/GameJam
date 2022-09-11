using System.Linq;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    [SerializeField] private BasePopup[] popups;

    private void Awake()
    {
        Instance = this;
        foreach (var popup in popups)
        {
            popup.Hide();
        }
    }

    public T Get<T>() where T : BasePopup
    {
        return popups.First(t => t is T) as T;
    }
}

public class BasePopup : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}