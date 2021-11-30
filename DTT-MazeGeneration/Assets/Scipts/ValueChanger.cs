using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

public class ValueChanger : MonoBehaviour
{
    [SerializeField] private float minValue = 10;
    [SerializeField] private float maxValue = 250;
    [SerializeField] private float changeAmount = 1;
    [SerializeField] private bool showDecimals = false;
    [Space]
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    

    public float CurrentValue { get; private set; }

    private void Start()
    {
        CurrentValue = minValue;

        plusButton.onClick.AddListener(() => AddValue(changeAmount));
        minusButton.onClick.AddListener(() => SubtractValue(changeAmount));
    }

    private void UpdateText()
    {
        valueText.text = showDecimals ? CurrentValue.ToString("0.0") : CurrentValue.ToString();
    }

    private void AddValue(float i)
    {
        CurrentValue = (float)Math.Round(CurrentValue, 1);
        
        if(CurrentValue + i <= maxValue)
            CurrentValue+= i;

        UpdateText();
    }

    private void SubtractValue(float i)
    {
        CurrentValue = (float)Math.Round(CurrentValue, 1);
        
        if(CurrentValue - i >= minValue)
            CurrentValue-= i;

        UpdateText();
    }
    
    public void SetChangeAmount(float newAmount)
    {
        changeAmount = newAmount;
    }
}
