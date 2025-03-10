using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DailyRewardStan : MonoBehaviour
{
    [Header("UI Elements")]
    public Image[] dayImages; 
    public Button claimButton; 

    [Header("Sprites")]
    public Sprite rewardAvailableSprite; 
    public Sprite rewardCollectedSprite; 
    public Sprite rewardLockedSprite; 

    [Header("Rewards")]
    public int[] dailyRewards; 

    private int currentDay; 
    private string lastClaimKey = "LastClaimDate"; 
    private string currentDayKey = "CurrentDay";


    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    private void Start()
    {
        LoadProgress();
        UpdateCalendarUI();
        claimButton.onClick.AddListener(ClaimReward);
    }

    private void LoadProgress()
    {
        currentDay = PlayerPrefs.GetInt(currentDayKey, 0);
        string lastClaimDate = PlayerPrefs.GetString(lastClaimKey, "");
        if (!string.IsNullOrEmpty(lastClaimDate))
        {
            DateTime lastClaimDateTime = DateTime.Parse(lastClaimDate);
            if (DateTime.Now.Date > lastClaimDateTime.Date)
            {
                claimButton.interactable = true;
            }
            else
            {
                claimButton.interactable = false;
            }
        }
        else
        {
            claimButton.interactable = true;
        }
    }

    private void UpdateCalendarUI()
    {
        for (int i = 0; i < dayImages.Length; i++)
        {
            if (i < currentDay)
            {
                dayImages[i].sprite = rewardCollectedSprite;
            }
            else if (i == currentDay)
            {
                dayImages[i].sprite = rewardAvailableSprite;
            }
            else
            {
                dayImages[i].sprite = rewardLockedSprite;
            }
        }
    }

    public void ClaimReward()
    {
        if (currentDay < dailyRewards.Length)
        {
            MoneyController.Instance.AddMoney(dailyRewards[currentDay]);    
            PlayerPrefs.SetString(lastClaimKey, DateTime.Now.ToString());       
            currentDay++;
            if (currentDay >= dailyRewards.Length)
            {
             
                currentDay = 0;
            }
            PlayerPrefs.SetInt(currentDayKey, currentDay);
            PlayerPrefs.Save();        
            UpdateCalendarUI();         
            claimButton.interactable = false;

            Debug.Log("Нагорода отримана! Поточний баланс: " + MoneyController.Instance.GetMoney());
        }
        else
        {
            Debug.LogWarning("Усі нагороди вже отримані.");
        }
    }
}
