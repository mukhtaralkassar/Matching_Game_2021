using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipCard : MonoBehaviour
{
    private string _ID;
    public string ID
    {
        get
        { 
            return _ID;
        }
    }
    public Button CardButton;
    public Image CardImage, CardBackgroundImage;
    public Vector3 TargetRotation, CloseRotation;
    public float DurationRotation;
    public AudioSource FlipAudioSource;
    public GameObject Parent;
     
    // Start is called before the first frame update
    void Start()
    {
        CardButton.onClick.AddListener(CardButton_Onclick);
    }

    private void CardButton_Onclick()
    {
        CardButton.interactable = false;
        Sequence seq = DOTween.Sequence();
        FlipAudioSource.Play();
        seq.AppendCallback(() =>
        {
            CardBackgroundImage.transform.DORotate(TargetRotation, DurationRotation,RotateMode.FastBeyond360).SetUpdate(true);
        }).OnComplete(() =>
        {
            GameManager.Instance.CheckIfCardsMatching(this);
            CardBackgroundImage.gameObject.SetActive(false);
        }).AppendInterval(DurationRotation).SetUpdate(true);
    }

    public void CloseCard()
    {
        
        Sequence seq = DOTween.Sequence();
        CardBackgroundImage.gameObject.SetActive(true);
        FlipAudioSource.Play();
        seq.AppendCallback(() =>
        {
            CardBackgroundImage.transform.DORotate(CloseRotation, DurationRotation, RotateMode.FastBeyond360).SetUpdate(true);
        }).AppendInterval(DurationRotation).OnComplete(() => 
        {
            
            CardButton.interactable = true; 
        }).SetUpdate(true);
    }

     public void Init(CardsData cardsData, int CountOfCardLevel)
     {
        CardImage.sprite = cardsData.MatchImage;
        this._ID = cardsData.ID;
        transform.SetSiblingIndex(UnityEngine.Random.Range(0, CountOfCardLevel - 1));
        Show();
     }

    internal void Hide()
    {
        CardBackgroundImage.transform.DORotate(CloseRotation, 0, RotateMode.FastBeyond360).SetUpdate(true);
        Parent.SetActive(false);
    }
    internal void Show()
    {
        gameObject.SetActive(true);
        Parent.SetActive(true);
        CardBackgroundImage.gameObject.SetActive(false);
    }
}
