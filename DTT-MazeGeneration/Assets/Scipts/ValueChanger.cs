using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class ValueChanger : MonoBehaviour
{
    [SerializeField] private float minValue = 10;
    [SerializeField] private float maxValue = 250;
    [SerializeField] private float changeAmount = 1;
    [Space]
    [SerializeField] private bool showDecimals = false;
    [SerializeField] private int amountDecimals = 2;
    [Space]
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    

    public float CurrentValue { get; private set; }

    private void Awake()
    {
        CurrentValue = minValue;

        plusButton.onClick.AddListener(() => AddValue(changeAmount));
        minusButton.onClick.AddListener(() => SubtractValue(changeAmount));
    }

    private void UpdateText()
    {
        valueText.text = showDecimals ? CurrentValue.ToString("0.00") : CurrentValue.ToString();
    }

    private void AddValue(float i)
    {
        CurrentValue = (float)Math.Round(CurrentValue, amountDecimals);
        
        if(CurrentValue + i <= maxValue)
            CurrentValue+= i;

        UpdateText();
    }

    private void SubtractValue(float i)
    {
        CurrentValue = (float)Math.Round(CurrentValue, amountDecimals);
        
        if(CurrentValue - i >= minValue)
            CurrentValue-= i;

        UpdateText();
    }
    
    public void SetChangeAmount(float newAmount)
    {
        changeAmount = newAmount;
    }
    
    public void SetButtonListeners(UnityAction newAction)
    {
        plusButton.onClick.AddListener(newAction);
        minusButton.onClick.AddListener(newAction);        
    }
}
