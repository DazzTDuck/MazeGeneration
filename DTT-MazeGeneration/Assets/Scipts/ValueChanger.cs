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
    
    //you can get the current value but never set it outside of this class
    public float CurrentValue { get; private set; }

    private void Awake()
    {
        CurrentValue = minValue;

        //automatically adds actions to the buttons
        plusButton.onClick.AddListener(() => AddValue(changeAmount));
        minusButton.onClick.AddListener(() => SubtractValue(changeAmount));
    }

    /// <summary>
    /// Updates the text that tells the value
    /// </summary>
    private void UpdateText()
    {
        valueText.text = showDecimals ? CurrentValue.ToString("0.00") : CurrentValue.ToString();
    }


    /// <summary>
    /// Adds to the current value
    /// </summary>
    /// <param name="i">Value to add</param>
    private void AddValue(float i)
    {
        CurrentValue = (float)Math.Round(CurrentValue, amountDecimals);
        
        if(CurrentValue + i <= maxValue)
            CurrentValue += i;

        UpdateText();
    }

    /// <summary>
    /// Subtracts to the current value
    /// </summary>
    /// <param name="i">Value to Subtract</param>
    private void SubtractValue(float i)
    {
        CurrentValue = (float)Math.Round(CurrentValue, amountDecimals);
        
        if(CurrentValue - i >= minValue)
            CurrentValue -= i;

        UpdateText();
    }
    
    /// <summary>
    /// Changes how much the value gets changed by each step
    /// </summary>
    /// <param name="newAmount">the new amount</param>
    public void SetChangeAmount(float newAmount)
    {
        changeAmount = newAmount;
    }
    
    /// <summary>
    /// Adds extra listeners for the buttons to give them other actions 
    /// </summary>
    /// <param name="newAction">actions what the button needs to do</param>
    public void SetButtonListeners(UnityAction newAction)
    {
        plusButton.onClick.AddListener(newAction);
        minusButton.onClick.AddListener(newAction);        
    }
}
