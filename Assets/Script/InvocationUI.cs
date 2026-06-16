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
    [SerializeField] private GameObject invocationHealthBarGo;

    private void Update()
    {
        UpdateTextUI();
        UpdateInvocationHpBar();
    }

    //Update le text 
    public void UpdateTextUI()
    {
        GameObject[] slots  = manager.GetSlots();
        GameObject equipped = manager.GetEquipped();
        bool isActive = manager.GetCurrentInvocation() != null;

        for(int i =0; i<slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slotText[i].text = $"Slot {i + 1}: Empty";
                slotText[i].color = Color.white ;   
            }
            else
            {
                slotText[i].text = $"Slot {i + 1}: {slots[i].name}";

                if (slots[i] == equipped && isActive) { slotText[i].color = Color.green; } 
                else { slotText[i].color = Color.white; }
            }
        }
    }

    public void UpdateInvocationHpBar()
    {
        GameObject currentInvocation = manager.GetCurrentInvocation();

        if(currentInvocation == null)
        {
            Debug.Log(invocationHealthBar);
            invocationHealthBar.fillAmount = 0 ;
            return; 
        }

        InvocationHealth health = currentInvocation.GetComponent<InvocationHealth>();

        float hpPercent = (float)health.currentHealth / health.GetMaxHealth();

        invocationHealthBar.fillAmount = hpPercent;
    }

    public void ShowInvocationBar()
    {
        invocationHealthBarGo.SetActive(true);
    }

    public void HideInvocationBar()
    {
        invocationHealthBarGo.SetActive(false);
    }
}
