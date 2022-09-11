using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class ProfilePopup : BasePopup
    {
        [SerializeField] private Image imgUser;
        [SerializeField] private TMP_Text txtLevel;
        [SerializeField] private TMP_Text txtUserName;
        [SerializeField] private GameObject goSoundsOn;
        [SerializeField] private GameObject goSoundsOff;
        [SerializeField] private GameObject goMusicOn;
        [SerializeField] private GameObject goMusicOff;

        public void OnSoundsOnClick()
        {
            goSoundsOff.SetActive(true);
            goSoundsOn.SetActive(false);
        }
        
        public void OnSoundsOffClick()
        {
            goSoundsOff.SetActive(false);
            goSoundsOn.SetActive(true);
        }
        
        public void OnMusicOnClick()
        {
            goMusicOff.SetActive(true);
            goMusicOn.SetActive(false);
        }
        
        public void OnMusicOffClick()
        {
            goMusicOff.SetActive(false);
            goMusicOn.SetActive(true);
        }
    }
}