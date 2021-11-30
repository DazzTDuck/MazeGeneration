using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

public class ValueChanger : MonoBehaviour
{
    [SerializeField] private int minValue = 10;
    [SerializeField] private int maxValue = 250;
    [Space]
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    
    private int changeAmount = 1;

    public int CurrentValue { get; private set; }

    private void Start()
    {
        CurrentValue = minValue;

        plusButton.onClick.AddListener(() => AddValue(changeAmount));
        minusButton.onClick.AddListener(() => SubtractValue(changeAmount));
    }

    private void UpdateText()
    {
        valueText.text = CurrentValue < 10 ? $"0{CurrentValue}" : CurrentValue.ToString();
    }

    private void AddValue(int i)
    {
        if(CurrentValue + i <= maxValue)
            CurrentValue+= i;

        UpdateText();
    }

    private void SubtractValue(int i)
    {
        if(CurrentValue - i >= minValue)
            CurrentValue-= i;

        UpdateText();
    }
    
    public void SetChangeAmount(int newAmount)
    {
        changeAmount = newAmount;
    }
}
