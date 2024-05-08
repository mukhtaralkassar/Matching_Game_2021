using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeManagerUI : MonoBehaviour
{
    public Button PlayButton;
    public Toggle EasyToggle, MediumToggle, HardToggle;

    // Start is called before the first frame update
    void Start()
    {
        PlayButton.onClick.AddListener(PlayButton_Onclick);
        EasyToggle.onValueChanged.AddListener(EasyToggle_OnValueChanged);
        MediumToggle.onValueChanged.AddListener(MediumToggle_OnValueChanged);
        HardToggle.onValueChanged.AddListener(HardToggle_OnValueChanged);
        ChangeToggle(GameManager.ExperieenceLevel, true);

    }

    private void EasyToggle_OnValueChanged(bool isOn)
    {
        if(!isOn && GameManager.ExperieenceLevel==0) 
        {
            EasyToggle.isOn = true;
            return;
        }
        if (isOn)
        {
            var OlderExperieenceLevel = GameManager.ExperieenceLevel;
            GameManager.ExperieenceLevel = 0;
            if (OlderExperieenceLevel != GameManager.ExperieenceLevel)
                ChangeToggle(OlderExperieenceLevel, false);
        }

    }
    private void MediumToggle_OnValueChanged(bool isOn)
    {
        if (!isOn && GameManager.ExperieenceLevel == 1)
        {
            MediumToggle.isOn = true;
            return;
        }
        if(isOn)
        {
            var OlderExperieenceLevel = GameManager.ExperieenceLevel;
            GameManager.ExperieenceLevel = 1;
            if (OlderExperieenceLevel != GameManager.ExperieenceLevel)
                ChangeToggle(OlderExperieenceLevel, false);
            
        }


    }
    private void HardToggle_OnValueChanged(bool isOn)
    {
        if (!isOn && GameManager.ExperieenceLevel == 2)
        {
            HardToggle.isOn = true;
            return;
        }
        if (isOn)
        {
            var OlderExperieenceLevel = GameManager.ExperieenceLevel;
            GameManager.ExperieenceLevel = 2;
            if(OlderExperieenceLevel != GameManager.ExperieenceLevel)
                ChangeToggle(OlderExperieenceLevel, false);
        }

    }
    private void ChangeToggle(int ExperieenceLevel, bool isOn=true)
    {
        if(ExperieenceLevel==0)
        {
            EasyToggle.isOn = isOn;
        }
        else if(ExperieenceLevel==1)
        {
            MediumToggle.isOn = isOn;
        }
        else
            HardToggle.isOn = isOn;
    }

    private void PlayButton_Onclick()
    {
        GameManager.Instance.GameManagerUI.ResetGamePlayUI();
        GameManager.Instance.GameManagerUI.Show();
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        GameManager.Instance.AnimationToEnter(transform);
        gameObject.SetActive(true);
    }
}
