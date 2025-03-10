using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LeagueInfoSlide : MonoBehaviour
{
    #region Public Variables
    public Image leagueImage;
    public TextMeshProUGUI leagueNameText;
    public TextMeshProUGUI leagueDescriptionText;
    public TextMeshProUGUI leaguePriceText;
    public TextMeshProUGUI leagueLevelText;

    public Button nextButton;
    public Button previousButton;

    public float animationDuration = 0.5f;

    public List<string> leagueNames = new List<string>();
    public List<Sprite> leagueSprites = new List<Sprite>();
    public List<string> leagueDescriptions = new List<string>();
    public List<string> leaguePrices = new List<string>();
    public List<string> leagueLevels = new List<string>();

    public List<GameObject> gameObjectsToToggle = new List<GameObject>();
    #endregion

    #region Private Variables
    private int currentLeagueIndex = 0;
    #endregion

    #region Unity Methods
    private void Start()
    {
        UpdateUI();
        nextButton.onClick.AddListener(MoveToNextLeague);
        previousButton.onClick.AddListener(MoveToPreviousLeague);
        UpdateButtonStates();  
    }
    #endregion

    #region League Transition Methods
    private void MoveToNextLeague()
    {
        FalseObject();
        if (currentLeagueIndex < leagueNames.Count - 1)
        {
            currentLeagueIndex++;
            AnimateLeagueTransition(1);
            SwitchToNextObject();
        }
        UpdateButtonStates(); 
    }

    private void MoveToPreviousLeague()
    {
        FalseObject();
        if (currentLeagueIndex > 0)
        {
            currentLeagueIndex--;
            AnimateLeagueTransition(-1);
            SwitchToPreviousObject();
        }
        UpdateButtonStates();  
    }

    private void AnimateLeagueTransition(int direction)
    {
        Vector3 startPosition = leagueImage.rectTransform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(-direction * 500f, 0, 0);

        leagueImage.rectTransform.DOLocalMove(endPosition, animationDuration).OnComplete(() =>
        {
            UpdateUI();
            leagueImage.rectTransform.localPosition = startPosition - new Vector3(direction * 500f, 0, 0);
            leagueImage.rectTransform.DOLocalMove(startPosition, animationDuration);
        });
    }
    #endregion

    #region UI Update Method
    private void UpdateUI()
    {
        leagueImage.sprite = leagueSprites[currentLeagueIndex];
        leagueNameText.text = leagueNames[currentLeagueIndex];
        leagueDescriptionText.text = leagueDescriptions[currentLeagueIndex];
        leaguePriceText.text = leaguePrices[currentLeagueIndex];
        leagueLevelText.text = leagueLevels[currentLeagueIndex];
    }
    #endregion

    #region Object Switching Methods
    private void SwitchToNextObject()
    {
        if (gameObjectsToToggle.Count == 0) return;     
        gameObjectsToToggle[currentLeagueIndex].SetActive(true);
    }

    private void SwitchToPreviousObject()
    {
        if (gameObjectsToToggle.Count == 0) return;
        gameObjectsToToggle[currentLeagueIndex].SetActive(true);
    }

    public void FalseObject()
    {
        if (gameObjectsToToggle.Count > 0)
        {
            gameObjectsToToggle[currentLeagueIndex].SetActive(false);
        }
    }
    #endregion

    #region Button State Management
    private void UpdateButtonStates()
    {
      
        if (currentLeagueIndex == leagueNames.Count - 1)
        {
            nextButton.interactable = false;
        }
        else
        {
            nextButton.interactable = true;
        }

        
        if (currentLeagueIndex == 0)
        {
            previousButton.interactable = false;
        }
        else
        {
            previousButton.interactable = true;
        }
    }
    #endregion
}
