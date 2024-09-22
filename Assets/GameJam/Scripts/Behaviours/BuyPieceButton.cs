using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyPieceButton : MonoBehaviour
{
    [field:SerializeField] public Button Button { get; private set; }
    [field:SerializeField] public TMP_Text CostText { get; private set; }
    
    private void Awake()
    {
        // Button = GetComponent<Button>();
        // CostText = transform.GetChild(0).GetComponent<TMP_Text>();
    }
}
