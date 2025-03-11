using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGame : MonoBehaviour
{
    public Button[] buttons;
    public Button startButton;
    public GameObject winPopup;
    public GameObject losePopup;
    public Button restartWinButton;
    public Button restartLoseButton;
    public Button[] closeButtons;

    private List<int> sequence = new List<int>();
    private int playerIndex = 0;
    private bool isPlayerTurn = false;
    public WhimsicalWindowAnimator windowAnimator;
    void Start()
    {
        foreach (var button in buttons)
        {
            int index = System.Array.IndexOf(buttons, button);
            button.onClick.AddListener(() => OnButtonClick(index));
        }

        startButton.onClick.AddListener(StartGame);
        restartWinButton.onClick.AddListener(StartNewRound);
        restartLoseButton.onClick.AddListener(StartNewRound);

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
        StartNewRound();
    }

    void StartNewRound()
    {
        winPopup.SetActive(false);
        losePopup.SetActive(false);
        sequence.Clear();
        playerIndex = 0;
        isPlayerTurn = false;
        GenerateSequence();
        StartCoroutine(ShowSequence());
    }

    void GenerateSequence()
    {
        for (int i = 0; i < 4; i++)
        {
            sequence.Add(Random.Range(0, buttons.Length));
        }
    }

    IEnumerator ShowSequence()
    {
        foreach (int index in sequence)
        {
            buttons[index].image.color = Color.green;
            yield return new WaitForSeconds(0.5f);
            buttons[index].image.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
        isPlayerTurn = true;
    }

    void OnButtonClick(int buttonIndex)
    {
        if (!isPlayerTurn) return;

        if (buttonIndex == sequence[playerIndex])
        {
            playerIndex++;
            if (playerIndex >= sequence.Count)
            {
                Debug.Log("Правильно! Нова послідовність");
                MoneyController.Instance.AddMoney(100);
                winPopup.SetActive(true);
                isPlayerTurn = false;
            }
        }
        else
        {
            Debug.Log("Неправильно! Починаємо знову");
            losePopup.SetActive(true);
            isPlayerTurn = false;
        }
    }
}
