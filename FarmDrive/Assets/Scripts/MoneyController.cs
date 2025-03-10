using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MoneyController : MonoBehaviour
{
    public static MoneyController Instance { get; private set; }

    [SerializeField] private int money;
    [SerializeField] private int dailyMoney = 100;

    private const string MoneyKey = "PlayerMoney";
    private const string LastRewardDateKey = "LastRewardDate";
    private const string DailyMoneyKey = "DailyMoney";

    private List<TextMeshProUGUI> moneyTextFields = new List<TextMeshProUGUI>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // Додаємо обробник для оновлення текстів при зміні сцени
    }

    private void Start()
    {
        LoadMoney();
        FindMoneyTextFields();
        CheckDailyReward();
        UpdateMoneyTexts();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindMoneyTextFields();  // Шукаємо текстові поля після зміни сцени
        UpdateMoneyTexts();     // Оновлюємо їх
    }

    public void AddMoney(int amount)
    {
        money += amount;
        SaveMoney();
        UpdateMoneyTexts();
    }

    public bool SubtractMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            SaveMoney();
            UpdateMoneyTexts();
            return true;
        }
        return false;
    }

    private void UpdateMoneyTexts()
    {
        FindMoneyTextFields(); // Переконаємося, що текстові поля оновлені після повернення на сцену
        string formattedMoney = FormatMoney(money);

        foreach (var textField in moneyTextFields)
        {
            if (textField != null)
            {
                textField.text = formattedMoney;
            }
        }
    }

    private string FormatMoney(int amount)
    {
        return amount >= 1000 ? (amount / 1000) + "K" : amount.ToString();
    }

    public int GetMoney()
    {
        return money;
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt(MoneyKey, money);
        PlayerPrefs.Save();
    }

    private void LoadMoney()
    {
        money = PlayerPrefs.GetInt(MoneyKey, 0);
    }

    private void CheckDailyReward()
    {
        string lastRewardDateStr = PlayerPrefs.GetString(LastRewardDateKey, "");
        DateTime lastRewardDate;

        if (DateTime.TryParse(lastRewardDateStr, out lastRewardDate))
        {
            if (lastRewardDate.Date < DateTime.Now.Date)
            {
                AddMoney(dailyMoney);
                PlayerPrefs.SetString(LastRewardDateKey, DateTime.Now.ToString());
                PlayerPrefs.Save();
            }
        }
        else
        {
            PlayerPrefs.SetString(LastRewardDateKey, DateTime.Now.ToString());
            AddMoney(dailyMoney);
            PlayerPrefs.Save();
        }
    }

    public void SetDailyMoney(int amount)
    {
        dailyMoney = amount;
        PlayerPrefs.SetInt(DailyMoneyKey, dailyMoney);
        PlayerPrefs.Save();
    }

    public int GetDailyMoney()
    {
        return dailyMoney;
    }

    private void FindMoneyTextFields()
    {
        MoneyTextContainer container = FindObjectOfType<MoneyTextContainer>();
        if (container != null)
        {
            moneyTextFields = container.GetMoneyTextFields();
        }
    }
}
