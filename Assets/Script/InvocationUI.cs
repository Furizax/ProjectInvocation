using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvocationUI : MonoBehaviour
{
    [SerializeField] private InvocationManager manager;

    [SerializeField] private TMP_Text[] slotText; 
    [SerializeField] private TMP_Text equippedText;

    [SerializeField] private Image[] slotHpFill;

    private void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        GameObject[] slots = manager.GetSlots();
        GameObject equipped = manager.GetEquipped();    
        bool isActive = manager.GetCurrentInvocation() != null;

        Debug.Log($"Slots: {slots.Length} | UI: {slotText.Length}");

        for (int i = 0; i < slots.Length; i++)
        {
            //Text Color 
            if (slots[i] == null)
            {
                slotText[i].text = $"Slot {i + 1}: Empty";
                slotText[i].color = Color.white ;
                continue;
            }

            slotText[i].text = $"Slot {i + 1}: {slots[i].name}";

            if (slots[i] == equipped && isActive)
                slotText[i].color = Color.green;
            else
                slotText[i].color = Color.white;

            //Hp bar 
            var health = slots[i].GetComponent<InvocationHealth>();

            if(health != null)
            {
                slotHpFill[i].fillAmount = health.GetHealthPercent();
            }
        }
    }
}
