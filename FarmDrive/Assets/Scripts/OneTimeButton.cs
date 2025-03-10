using UnityEngine;
using UnityEngine.UI;

public class OneTimeButton : MonoBehaviour
{
    [SerializeField] private Button targetButton; 
    private string buttonKey; 

    private void Awake()
    {
        if (targetButton == null)
        {
            targetButton = GetComponent<Button>();
        }

      
        buttonKey = $"{gameObject.name}_Pressed";

        
        if (PlayerPrefs.GetInt(buttonKey, 0) == 1)
        {
            targetButton.interactable = false; 
        }
        else
        {
            targetButton.interactable = true; 
            targetButton.onClick.AddListener(OnButtonPressed);
        }
    }

    private void OnButtonPressed()
    {
        PlayerPrefs.SetInt(buttonKey, 1);
        PlayerPrefs.Save();
        targetButton.interactable = false;
        Debug.Log($"{gameObject.name} натиснута!");
    }
}
