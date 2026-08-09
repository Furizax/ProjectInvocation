using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InvocationUI : MonoBehaviour
{
    [SerializeField] private InvocationManager manager;

    [SerializeField] private TMP_Text[] slotText; 
    [SerializeField] private TMP_Text equippedText;

    [SerializeField] private Image invocationHealthBar;
    [SerializeField] private GameObject[] invocationHealthBars;

    private void Update()
    {
        UpdateTextUI();
        UpdateInvocationHpBar();
    }

    //Update le text 
    public void UpdateTextUI()
    {
        GameObject[] slots = manager.GetSlots();

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slotText[i].text = $"Slot {i + 1}: Empty";
                slotText[i].color = Color.white;
            }
            else
            {
                slotText[i].text = $"Slot {i + 1}: {slots[i].name}";
                slotText[i].color = Color.white;
            }
        }

        UpdateInvocationHpBar();
    }

    public void UpdateInvocationHpBar()
    {
        GameObject[] slots = manager.GetSlots();

        for (int i = 0; i < invocationHealthBars.Length; i++)
        {
            if (slots[i] != null)
            {
                invocationHealthBars[i].SetActive(true);
            }
            else
            {
                invocationHealthBars[i].SetActive(false);
            }
        }
    }
}
