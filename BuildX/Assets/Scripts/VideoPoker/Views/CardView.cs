using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [SerializeField] private HandView handView;
    [SerializeField] private Image imgCard;
    [SerializeField] private Image imgCardBack;
    [SerializeField] private GameObject goHold;

    public bool IsOnHold { get; private set; }
    public int Index { get; private set; }

    private void Awake()
    {
        goHold.SetActive(IsOnHold);
        imgCardBack.enabled = true;
        imgCard.enabled = false;
        handView.AddCard(this);
    }

    public void SetData()
    {
        IsOnHold = false;
        goHold.SetActive(IsOnHold);
        imgCardBack.enabled = false;
        imgCard.enabled = true;
    }

    public void SetData(int index, Sprite cardSprite)
    {
        Index = index;
        imgCard.sprite = cardSprite;
        SetData();
    }

    public void ToggleOnHold()
    {
        IsOnHold = !IsOnHold;
        goHold.SetActive(IsOnHold);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        handView.OnPointerEnter(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        handView.OnPointerClick(this);
    }
}