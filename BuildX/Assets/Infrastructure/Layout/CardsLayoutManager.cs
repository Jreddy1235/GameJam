using System.Collections.Generic;
using BrilliantBingo.Code.Infrastructure.Layout.Interfaces;
using UnityEngine;

namespace BrilliantBingo.Code.Infrastructure.Layout
{
    public class CardsLayoutManager : MonoBehaviour, ICardsLayoutManager
    {
        #region Fields

        [SerializeField] private GameObject _rightArrowObj;
        [SerializeField] private GameObject _LeftArrowObj;
        
        [SerializeField]
        private RectTransform _singleCardLayout;

        [SerializeField]
        private RectTransform _twoCardsLayout;

        [SerializeField]
        private RectTransform _threeCardsLayout;

        [SerializeField]
        private RectTransform _fourCardsLayout;

        private List<GameObject> _cardList = new List<GameObject>();

        #endregion

        #region Methods

        public void Awake()
        {
            SetVisibilityOfCards(false);
        }

        public void SetVisibilityOfCards(bool flag)
        {
            _rightArrowObj.SetActive(flag);
            _LeftArrowObj.SetActive(flag);
            _singleCardLayout.gameObject.SetActive(flag);
            _twoCardsLayout.gameObject.SetActive(flag);
            _threeCardsLayout.gameObject.SetActive(flag);
            _fourCardsLayout.gameObject.SetActive(flag);
        }
        
        #endregion

        #region ICardsLayoutManager

        #region Properties

        public Vector2 SingleCardLayoutPosition
        {
            get { return _singleCardLayout.anchoredPosition; }
        }

        public Vector2 TwoCardsLayoutPosition
        {
            get { return _twoCardsLayout.anchoredPosition; }
        }

        public Vector2 ThreeCardsLayoutPosition
        {
            get { return _threeCardsLayout.anchoredPosition; }
        }

        public Vector2 FourCardsLayoutPosition
        {
            get { return _fourCardsLayout.anchoredPosition; }
        }

        #endregion

        #region Methods

        public void LayoutOneCard(GameObject firstCard)
        {
            _cardList.Add(firstCard);
            
            _rightArrowObj.SetActive(false);
            _singleCardLayout.gameObject.SetActive(true);

            firstCard.GetComponent<RectTransform>().SetParent(_singleCardLayout, false);
            firstCard.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        }

        public void LayoutTwoCards(GameObject firstCard, GameObject secondCard)
        {
            _cardList.Add(firstCard);
            _cardList.Add(secondCard);
            _rightArrowObj.SetActive(false);
            _twoCardsLayout.gameObject.SetActive(true);
            firstCard.GetComponent<RectTransform>().SetParent(_twoCardsLayout, false);
            firstCard.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            secondCard.GetComponent<RectTransform>().SetParent(_twoCardsLayout, false);
            secondCard.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        }

        public void LayoutThreeCards(GameObject firstCard, GameObject secondCard, 
            GameObject thirdCard)
        {
            _cardList.Add(firstCard);
            _cardList.Add(secondCard);
            _cardList.Add(thirdCard);
            
            _rightArrowObj.SetActive(true);
            _threeCardsLayout.gameObject.SetActive(true);
            
            firstCard.GetComponent<RectTransform>().SetParent(_threeCardsLayout, false);
            firstCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            secondCard.GetComponent<RectTransform>().SetParent(_threeCardsLayout, false);
            secondCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            thirdCard.GetComponent<RectTransform>().SetParent(_threeCardsLayout, false);
            thirdCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            
            thirdCard.SetActive(false);
        }

        public void LayoutFourCards(GameObject firstCard, GameObject secondCard, 
            GameObject thirdCard, GameObject fourthCard)
        {
            _cardList.Add(firstCard);
            _cardList.Add(secondCard);
            _cardList.Add(thirdCard);
            _cardList.Add(fourthCard);
            
            _rightArrowObj.SetActive(true);
            _fourCardsLayout.gameObject.SetActive(true);
            
            firstCard.GetComponent<RectTransform>().SetParent(_fourCardsLayout, false);
            firstCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            secondCard.GetComponent<RectTransform>().SetParent(_fourCardsLayout, false);
            secondCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            thirdCard.GetComponent<RectTransform>().SetParent(_fourCardsLayout, false);
            thirdCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            fourthCard.GetComponent<RectTransform>().SetParent(_fourCardsLayout, false);
            fourthCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            
            thirdCard.SetActive(false);
            fourthCard.SetActive(false);
        }

        public void ClearCards()
        {
            foreach (var card in _cardList)
            {
                Destroy(card);
            }
            _cardList.Clear();
            
        }

        public void OnRightArrowClick()
        {
            _rightArrowObj.SetActive(false);
            _LeftArrowObj.SetActive(true);
            SetArrowVisibility(false);
        }
        
        public void OnLeftArrowClick()
        {
            _rightArrowObj.SetActive(true);
            _LeftArrowObj.SetActive(false);
            SetArrowVisibility(true);
        }

        private void SetArrowVisibility(bool flag)
        {
            if (_cardList.Count > 2 || _cardList.Count > 3)
            {
                _cardList[0].SetActive(flag);
                _cardList[1].SetActive(flag);
                _cardList[2].SetActive(!flag);
                
                if(_cardList.Count > 3)
                    _cardList[3].SetActive(!flag);
            }
        }
        
        #endregion

        #endregion
    }
}