using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillUpgradeManager : MonoBehaviour
{
    public static SkillUpgradeManager Instance { get; private set; }

    [SerializeField] private int playerLevel = 0;
    [SerializeField] private int currentLeague = 1;

    [System.Serializable]
    public class Skill
    {
        public int price;
        public TextMeshProUGUI priceText;
        public Button upgradeButton;
        public GameObject purchasedIndicator;
        public int leaguePurchased = 0;
    }

    [System.Serializable]
    public class League
    {
        public int leagueNumber;
        public string leagueName;
        public List<Skill> skills;

        public Button leagueButton;
        public Image leagueButtonImage;
        public TextMeshProUGUI leagueButtonText;
        public Sprite currentLeagueSprite;
        public Sprite lockedLeagueSprite;
        public Sprite completedLeagueSprite;
        public Sprite availableLeagueSprite;

      
        public Sprite leagueLogo; 
    }

    [SerializeField] private List<League> leagues;
    [SerializeField] private List<TextMeshProUGUI> levelTexts;
    [SerializeField] private Image leagueImage;
    [SerializeField] private Image tractorImage;

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
        }
    }

    private void Start()
    {
        LoadProgress();
        InitializeSkills();
        UpdateLeagueButtons();
        UpdatePlayerLevelTexts();
        UpdateTractorLogo();
    }

    private void InitializeSkills()
    {
        League currentLeagueData = leagues.Find(league => league.leagueNumber == currentLeague);
        if (currentLeagueData == null)
        {
            Debug.LogError("Ліга не знайдена: " + currentLeague);
            return;
        }

        foreach (var skill in currentLeagueData.skills)
        {
            skill.priceText.text = skill.price.ToString();

            skill.upgradeButton.onClick.RemoveAllListeners();
            skill.upgradeButton.onClick.AddListener(() => UpgradeSkill(skill));

            skill.purchasedIndicator.SetActive(skill.leaguePurchased >= currentLeague);
            skill.upgradeButton.interactable = skill.leaguePurchased < currentLeague;
        }
    }

    private void UpgradeSkill(Skill skill)
    {
        if (MoneyController.Instance.SubtractMoney(skill.price))
        {
            playerLevel++;
            skill.leaguePurchased = currentLeague;
            skill.purchasedIndicator.SetActive(true);
            skill.upgradeButton.interactable = false;
            SaveProgress();
            UpdatePlayerLevelTexts();
            UpdateLeagueButtons();
        }
        else
        {
            Debug.Log("Недостатньо коштів для прокачки!");
        }
    }

    private bool AreAllSkillsUpgradedInCurrentLeague()
    {
        League currentLeagueData = leagues.Find(league => league.leagueNumber == currentLeague);
        if (currentLeagueData == null) return false;

        foreach (var skill in currentLeagueData.skills)
        {
            if (skill.leaguePurchased < currentLeague)
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateLeagueButtons()
    {
        foreach (var league in leagues)
        {
            if (league.leagueButton == null || league.leagueButtonImage == null || league.leagueButtonText == null)
            {
                Debug.LogError("Кнопка, іконка або текст для ліги не задані!");
                continue;
            }

            if (league.leagueNumber < currentLeague)
            {
                league.leagueButton.interactable = false;
                league.leagueButtonImage.sprite = league.completedLeagueSprite;
                league.leagueButtonText.text = "PURCHASED";
            }
            else if (league.leagueNumber == currentLeague)
            {
                league.leagueButton.interactable = true;
                league.leagueButtonImage.sprite = league.currentLeagueSprite;
                league.leagueButtonText.text = "PURCHASED";
                SaveLeagueLogo(league.leagueLogo); 
            }
            else if (league.leagueNumber == currentLeague + 1 && AreAllSkillsUpgradedInCurrentLeague())
            {
                league.leagueButton.interactable = true;
                league.leagueButtonImage.sprite = league.availableLeagueSprite;
                league.leagueButtonText.text = "UPGRADE";
            }
            else
            {
                league.leagueButton.interactable = false;
                league.leagueButtonImage.sprite = league.lockedLeagueSprite;
                league.leagueButtonText.text = "LOCKED";
            }
        }
    }

    private void UpdatePlayerLevelTexts()
    {
        foreach (var levelText in levelTexts)
        {
            if (levelText != null)
            {
                levelText.text = playerLevel.ToString();
            }
        }
    }

    private void UpdateTractorLogo()
    {
      
        Sprite savedLogo = LoadLeagueLogo();
        if (savedLogo != null)
        {
            leagueImage.sprite = savedLogo;
            tractorImage.sprite = savedLogo;
        }
    }
    public void UpdateUI()
    {
        UpdateTractorLogo();
        UpdatePlayerLevelTexts();
    }

    private void SaveLeagueLogo(Sprite logoSprite)
    {
       
        string logoName = logoSprite.name;
        PlayerPrefs.SetString("CurrentLeagueLogo", logoName);
    }

    private Sprite LoadLeagueLogo()
    {
       
        string logoName = PlayerPrefs.GetString("CurrentLeagueLogo", "");
        foreach (var league in leagues)
        {
            if (league.leagueLogo.name == logoName)
            {
                return league.leagueLogo;
            }
        }
        return null;
    }

    public void AdvanceToNextLeague()
    {
        if (AreAllSkillsUpgradedInCurrentLeague())
        {
            if (currentLeague < leagues.Count)
            {
                currentLeague++;
                InitializeSkills();
                UpdateLeagueButtons();
                SaveProgress();
            }
            else
            {
                Debug.Log("Гравець вже у найвищій лізі!");
            }
        }
        else
        {
            Debug.Log("Всі скіли поточної ліги мають бути прокачані перед переходом!");
        }
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("PlayerLevel", playerLevel);
        PlayerPrefs.SetInt("CurrentLeague", currentLeague);

        for (int i = 0; i < leagues.Count; i++)
        {
            League league = leagues[i];
            for (int j = 0; j < league.skills.Count; j++)
            {
                PlayerPrefs.SetInt($"SkillPurchased_{i}_{j}", league.skills[j].leaguePurchased);
            }
        }
    }

    public void LoadProgress()
    {
        playerLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        currentLeague = PlayerPrefs.GetInt("CurrentLeague", 1);

        for (int i = 0; i < leagues.Count; i++)
        {
            League league = leagues[i];
            for (int j = 0; j < league.skills.Count; j++)
            {
                league.skills[j].leaguePurchased = PlayerPrefs.GetInt($"SkillPurchased_{i}_{j}", 0);
            }
        }
    }
}
