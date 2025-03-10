using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyTextContainer : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> moneyTextFields;

    public List<TextMeshProUGUI> GetMoneyTextFields() => moneyTextFields;
}
