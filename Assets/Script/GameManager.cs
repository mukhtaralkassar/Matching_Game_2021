using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static int ExperieenceLevel
    {
        get
        {
            return PlayerPrefs.GetInt("ExperieenceLevel", 1);
        }
        set
        {
            PlayerPrefs.SetInt("ExperieenceLevel", value);
        }
    }

    public int CountOfEasyLevel = 8, ConstraintCountEasy = 2;
    public Vector2 CellSizeForEasy, SpacingForEasy;
    public int CountOfMediumeLevel = 16, ConstraintCountMediume = 2;
    public Vector2 CellSizeForMediume, SpacingForMediume;
    public int CountOfHardLevel = 32, ConstraintCountHard = 2;
    public Vector2 CellSizeForHard, SpacingForHard;
    [NonSerialized]
    public int CurrentCountOfLevel;
    private FlipCard FirstFlipCard;
    public AudioClip WinLevelSound, IncreaseMatchesSound;
    public AudioSource AudioSource;
    public GameManagerUI GameManagerUI;
    public HomeManagerUI HomeManagerUI;
    public Transform StartPosition, TargetPosition;
    public float DurationMove=0.5f;
    public bool IsStartGame
    {
        get
        {
            return _IsStartGame;
        }
        set
        {
            _IsStartGame=value;
        }
    }
    private bool _IsStartGame=false;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        HomeManagerUI.Show();
    }
    public void AnimationToEnter(Transform transform,Action action=null)
    {
        transform.DOMove(StartPosition.position, 0);
        transform.DOMove(TargetPosition.position, DurationMove).SetUpdate(true).SetEase(Ease.InOutElastic).OnComplete(() => 
        {
            action?.Invoke();
        });

    }
    internal void CheckIfCardsMatching(FlipCard FlipCard)
    {
        if(!IsStartGame) 
            return;
        GameManagerUI.Instance.TurnsCounter++;


        if (FirstFlipCard != null)
        {
            if (FirstFlipCard.ID.Equals( FlipCard.ID))
            {
                AudioSource.clip = IncreaseMatchesSound;
                AudioSource.Play();
                GameManagerUI.Instance.MatchesCounter++;
                StartCoroutine(HideOrCloseAfterCardAfteWait(FirstFlipCard, FlipCard,true));                            
            }
            else
            {
                
                StartCoroutine(HideOrCloseAfterCardAfteWait(FirstFlipCard, FlipCard, false));

            }
            FirstFlipCard = null;
            if((CurrentCountOfLevel/2) == GameManagerUI.Instance.MatchesCounter)
            {
                StartCoroutine(ShowWinGameObjectAfterWWait());
            }
        }
        else
            FirstFlipCard = FlipCard;

    }
    IEnumerator ShowWinGameObjectAfterWWait()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForSecondsRealtime(0.5f);
        GameManagerUI.Instance.HideGameLevelAndShowWin(true);
        AudioSource.clip = WinLevelSound;
        AudioSource.Play();
    }
    IEnumerator HideOrCloseAfterCardAfteWait(FlipCard flipCard ,FlipCard flipCard1,bool IsHide)
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForSecondsRealtime(0.5f);
        if (IsHide)
        {
            flipCard.Hide();
            flipCard1.Hide();
        }
        else
        {
            flipCard.CloseCard();
            flipCard1.CloseCard();
        }
    }
    
}
