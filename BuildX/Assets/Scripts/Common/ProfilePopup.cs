using System;
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

        private void Awake()
        {
            OnSoundsOffClick();
        }

        public void OnSoundsOnClick()
        {
            goSoundsOff.SetActive(true);
            goSoundsOn.SetActive(false);
            goMusicOff.SetActive(true);
            goMusicOn.SetActive(false);
            AudioListener.pause = true;
        }
        
        public void OnSoundsOffClick()
        {
            goSoundsOff.SetActive(false);
            goSoundsOn.SetActive(true);
            goMusicOff.SetActive(false);
            goMusicOn.SetActive(true);
            AudioListener.pause = false;
        }
        
        public void OnMusicOnClick()
        {
            goSoundsOff.SetActive(true);
            goSoundsOn.SetActive(false);
            goMusicOff.SetActive(true);
            goMusicOn.SetActive(false);
            AudioListener.pause = true;
        }
        
        public void OnMusicOffClick()
        {
            goSoundsOff.SetActive(false);
            goSoundsOn.SetActive(true);
            goMusicOff.SetActive(false);
            goMusicOn.SetActive(true);
            AudioListener.pause = false;
        }
    }
}