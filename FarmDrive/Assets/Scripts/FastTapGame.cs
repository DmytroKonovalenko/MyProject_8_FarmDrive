using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastTapGame : MonoBehaviour
{
    public Button[] buttons;
    public Button startButton;
    public GameObject winPopup;
    public GameObject losePopup;
    public Button restartWinButton;
    public Button restartLoseButton;
    public Button[] closeButtons;

    private List<int> availableIndices = new List<int>();
    private int currentRound = 0;
    private int activeButtonIndex = -1;
    private Coroutine roundCoroutine;
    private float reactionTime = 0.75f;
    public WhimsicalWindowAnimator windowAnimator;

    void Start()
    {
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
            button.interactable = false;
            int index = System.Array.IndexOf(buttons, button);
            button.onClick.AddListener(() => OnButtonClick(index));
        }

        startButton.onClick.AddListener(StartGame);
        restartWinButton.onClick.AddListener(StartGame);
        restartLoseButton.onClick.AddListener(StartGame);

        foreach (var closeButton in closeButtons)
        {
            closeButton.onClick.AddListener(() => {
                windowAnimator.AnimateClose();
                winPopup.SetActive(false);
                losePopup.SetActive(false);
                startButton.gameObject.SetActive(true);
            });
        }

        startButton.gameObject.SetActive(true);
        winPopup.SetActive(false);
        losePopup.SetActive(false);
    }

    void StartGame()
    {
        startButton.gameObject.SetActive(false);
        winPopup.SetActive(false);
        losePopup.SetActive(false);
        currentRound = 0;
        availableIndices.Clear();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
            buttons[i].interactable = false;
            availableIndices.Add(i);
        }

        StartNewRound();
    }

    void StartNewRound()
    {
        if (availableIndices.Count == 0)
        {
            winPopup.SetActive(true);
            MoneyController.Instance.AddMoney(100);
            return;
        }

        int randomIndex = Random.Range(0, availableIndices.Count);
        activeButtonIndex = availableIndices[randomIndex];
        availableIndices.RemoveAt(randomIndex);

        buttons[activeButtonIndex].gameObject.SetActive(true);
        buttons[activeButtonIndex].interactable = true;
        roundCoroutine = StartCoroutine(WaitForReaction());
    }

    IEnumerator WaitForReaction()
    {
        yield return new WaitForSeconds(reactionTime);
        GameOver();
    }

    void OnButtonClick(int buttonIndex)
    {
        if (buttonIndex != activeButtonIndex) return;

        StopCoroutine(roundCoroutine);
        buttons[activeButtonIndex].interactable = false;
        StartNewRound();
    }

    void GameOver()
    {
        if (roundCoroutine != null)
            StopCoroutine(roundCoroutine);

        losePopup.SetActive(true);
    }
}
