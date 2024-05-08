using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour
{
    public static GameManagerUI Instance;
    public List<CardsData> CardsData;
    public TMP_Text MatchesText, TurnsText,WinText;
    public Button HomeButton, ResartButton;
    public GameObject MatchesGameObject ,WinGameObject;
    private List<FlipCard> _FlipCards;
    public FlipCard FlipCardTemplate;
    private int _ExperieenceLevel, CountOfCardLevel;
    private int _MatchesCounter;
    public Transform StartResartPosition;
    public float WaitToCloseCards=2, DurationWinTextScale=0.3f, DurationMoveButtonToTarget=0.2f;
    public GridLayoutGroup GridLayoutGroup;
    public Transform TargetResartPosition;

    public int MatchesCounter 
    {
        get 
        {
            return _MatchesCounter;
        }
        set 
        {
            _MatchesCounter = value;
            MatchesText.text=_MatchesCounter.ToString();

        }
    }
    private int _TurnsCounter;
    public int TurnsCounter
    {
        get
        {
            return _TurnsCounter;
        }
        set
        {
            _TurnsCounter = value;
            TurnsText.text = _TurnsCounter.ToString();

        }
    }

    private void Start()
    {
        Instance=this;
        ResartButton.onClick.AddListener(ResartButton_OnClick);
        HomeButton.onClick.AddListener(HomeButton_OnClick);
    }

    private void HomeButton_OnClick()
    {
        GameManager.Instance.HomeManagerUI.Show();
        
        Hide();
    }

    private void ResartButton_OnClick()
    {
        ResetGamePlayUI();
        HideGameLevelAndShowWin(false);
        Show();
    }

    public void ResetGamePlayUI()
    {
        MatchesCounter=0; 
        TurnsCounter=0;
        _ExperieenceLevel =GameManager.ExperieenceLevel;

        if (_ExperieenceLevel == 0)
        {
            GridLayoutGroup.spacing = GameManager.Instance.SpacingForEasy;
            GridLayoutGroup.cellSize = GameManager.Instance.CellSizeForEasy;
            GridLayoutGroup.constraintCount = GameManager.Instance.ConstraintCountEasy;
            CountOfCardLevel = GameManager.Instance.CountOfEasyLevel;
        }

        else if (_ExperieenceLevel == 1)
        {
            GridLayoutGroup.spacing = GameManager.Instance.SpacingForMediume;
            GridLayoutGroup.cellSize = GameManager.Instance.CellSizeForMediume;
            GridLayoutGroup.constraintCount = GameManager.Instance.ConstraintCountMediume;
            CountOfCardLevel = GameManager.Instance.CountOfMediumeLevel;
        }
        else
        {
            GridLayoutGroup.spacing = GameManager.Instance.SpacingForHard;
            GridLayoutGroup.cellSize = GameManager.Instance.CellSizeForHard;
            GridLayoutGroup.constraintCount = GameManager.Instance.ConstraintCountHard;
            CountOfCardLevel = GameManager.Instance.CountOfHardLevel;
        }
        GameManager.Instance.CurrentCountOfLevel = CountOfCardLevel;
        CheckIfFlipCardEnoughToPlay();
        var cloneCardsData = GetCloneFromCardsData();
        for(int i= 0;i< CountOfCardLevel / 2; i++)
        {
            int RandomIndex = UnityEngine.Random.Range(0, cloneCardsData.Count-1);
            _FlipCards[i].Init(cloneCardsData[RandomIndex], CountOfCardLevel);
            _FlipCards[CountOfCardLevel - i - 1].Init(cloneCardsData[RandomIndex], CountOfCardLevel);
            cloneCardsData.RemoveAt(RandomIndex);
        }
    }
    IEnumerator CloseAllCardAfterWait()
    {
        yield return new WaitForSecondsRealtime(WaitToCloseCards);
        for (int i = 0; i < CountOfCardLevel; i++)
        {

            _FlipCards[i].CloseCard();
        }
        GameManager.Instance.IsStartGame=true;
    }
    private void CheckIfFlipCardEnoughToPlay()
    {
        if (_FlipCards == null || _FlipCards.Count == 0)
        {
            _FlipCards = new List<FlipCard>();
        }
        for(int i= 0; i<_FlipCards.Count;i++)
            _FlipCards[i].gameObject.SetActive(false);
        int needOfCount = CountOfCardLevel - _FlipCards.Count;
        for (int i = 0; i < needOfCount; i++)
        {
            _FlipCards.Add(Instantiate(FlipCardTemplate, GridLayoutGroup.transform));
            _FlipCards[i].name = "Card " + i;
        }



    }
    public void HideGameLevelAndShowWin(bool showWinGameObject)
    {
        if (showWinGameObject)
        {
            GameManager.Instance.IsStartGame = false;
            ResartButton.transform.DOMove(StartResartPosition.position,0);
            WinText.transform.DOScale(2, 0);
            StartAnimationToWin();
        }
        MatchesGameObject.SetActive(!showWinGameObject);
        WinGameObject.SetActive(showWinGameObject);

    }

    private void StartAnimationToWin()
    {
        DG.Tweening.Sequence seq = DOTween.Sequence();
        seq.Append(WinText.transform.DOScale(1,DurationWinTextScale).SetEase(Ease.InOutElastic))
            .Append(ResartButton.transform.DOMove(TargetResartPosition.transform.position,DurationMoveButtonToTarget).SetEase(Ease.InOutElastic)).SetUpdate(true);
        
    }

    private List<CardsData> GetCloneFromCardsData()
    {
        return new List<CardsData>( CardsData);
    }

    internal void Show()
    {
        MatchesGameObject.SetActive(true);
        WinGameObject.SetActive(false);
        gameObject.SetActive(true);
        GameManager.Instance.AnimationToEnter(transform, () => 
        {
            StartCoroutine(CloseAllCardAfterWait());
        });
        
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
}

