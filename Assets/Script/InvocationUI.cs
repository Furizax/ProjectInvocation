using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvocationUI : MonoBehaviour
{
    public InvocationManager manager;

    [SerializeField] private TMP_Text[] slotText; 
    [SerializeField] private TMP_Text equippedText;

    private void Update()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        GameObject[] slots = manager.GetSlot();
        GameObject equipped = manager.GetEquipped();
        bool isActive = manager.GetCurrentInvocation() != null;

        for(int i  = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slotText[i].text = $"Slot {i + 1}: Empty";
                slotText[i].color = Color.white ;
            }
            else
            {
                slotText[i].text = $"Slot {i + 1}: {slots[i].name}";

                if (slots[i] == equipped && isActive)
                {
                    slotText[i].color = Color.green;
                }
                else
                {
                    slotText[i].color = Color.white;
                }
            }
        }
    }
}
