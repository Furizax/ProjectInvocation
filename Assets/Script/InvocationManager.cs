using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvocationManager : MonoBehaviour
{
    [SerializeField] private InvocationUI ui;

    [SerializeField] GameObject baseInvocation;
    [SerializeField] public Transform spawnPoint;
    private GameObject currentInvocation;

    [SerializeField] GameObject[] invocationSlots;
    private float[] invocationHealth;

    public GameObject equippedInvocation;

    private enum State
    {
        Ready,
        Active,
    }

    private State state = State.Ready;

    private void Start()
    {
        invocationSlots[0] = baseInvocation;

        invocationHealth = new float[invocationSlots.Length];

        for (int i = 0; i < invocationSlots.Length; i++)
        {
            if (invocationSlots[i] != null)
            {
                InvocationStats stats = invocationSlots[i].GetComponent<InvocationStats>();

                invocationHealth[i] = stats.maxHealth;
            }
        }
        SelectInvocation(0);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        RegenerateInvocations();
    }

    void HandleInput()
    {
        //Selection des invocations
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectInvocation(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectInvocation(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectInvocation(2);

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (state == State.Ready)
                SpawnInvocation();

            else if (state == State.Active)
                DespawnInvocation();
        }
    }

    void SpawnInvocation()
    {
        if (state != State.Ready)
            return;

        if (equippedInvocation == null)
        {
            Debug.LogWarning("No invocation equipped");
            return;
        }

        currentInvocation = Instantiate(
            equippedInvocation,
            spawnPoint.position,
            spawnPoint.rotation
        );

        InvocationHealth health = currentInvocation.GetComponent<InvocationHealth>();
        int slotIndex = GetEquippedSlotIndex();

        health.SetHealth(invocationHealth[slotIndex]); //Donner à l'invocation sa vie sauvegardé

        health.SetHealtBar(ui.GetInvocationHealthBar(slotIndex));

        state = State.Active;
    }

    void DespawnInvocation()
    {
        if (currentInvocation != null)
        {
            InvocationHealth health = currentInvocation.GetComponent<InvocationHealth>(); //Obtenir le script des pv de l'invocation

            int slotIndex = GetEquippedSlotIndex();

            //Sauvegarder la vie actuelle 
            invocationHealth[slotIndex] = health.GetCurrentHealth();

            Destroy(currentInvocation); //Retirer l'invocation
            currentInvocation = null;
        }

        state = State.Ready; 
    }

    public void AddInvocation(GameObject newInvocation)
    {
        for (int i = 0; i < invocationSlots.Length; i++)
        {
            if (invocationSlots[i] == null)
            {
                invocationSlots[i] = newInvocation; //Ajouter la nouvelle invocation dans le tableau
                InvocationStats stats = newInvocation.GetComponent<InvocationStats>(); // assigner les stats à la nouvelle invocation

                invocationHealth[i] = stats.maxHealth; 

                Debug.Log("invocation has been added: " + invocationSlots[i].name);

                ui.UpdateTextUI();
                return;
            }
        }
        Debug.Log("No free slots");
    }

    void SelectInvocation(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= invocationSlots.Length)
            return;

        if (invocationSlots[slotIndex] == null)
        {
            equippedInvocation = null;

            Debug.Log("Slot is empty");

            ui.UpdateTextUI();
            return;
        }

        equippedInvocation = invocationSlots[slotIndex];

        Debug.Log($"Equipped: {equippedInvocation.name}");

        ui.UpdateTextUI();
    }

    void RegenerateInvocations()
    {
        for (int i = 0; i < invocationSlots.Length; i++)
        {
            if (invocationSlots[i] == null)
                continue;
            if (state == State.Active && i == GetEquippedSlotIndex())
                continue;

            InvocationStats stats = invocationSlots[i].GetComponent<InvocationStats>();

            invocationHealth[i] += stats.regenerateRate * Time.deltaTime;

            if (invocationHealth[i] > stats.maxHealth)
                invocationHealth[i] = stats.maxHealth;

            ui.UpdateInvocationHealth(i, invocationHealth[i], stats.maxHealth);
        }
    }

    public void OnInvocationDeath()
    {
        currentInvocation = null;
        state = State.Ready;
    }

    public int GetEquippedSlotIndex()
    {
        for (int i = 0; i < invocationSlots.Length; i++)
        {
            if (invocationSlots[i] == equippedInvocation)
            {
                return i;
            }
        }
        return -1;
    }
    public bool hasFreeSlot()
    {
        foreach (GameObject slot in invocationSlots)
        {
            if (slot == null)
            {
                return true;
            }
        }

        return false;
    }

    public GameObject GetEquipped()
    {
        return equippedInvocation;
    }

    public GameObject GetCurrentInvocation()
    {
        return currentInvocation;
    }

    public GameObject[] GetSlots()
    {
        return invocationSlots;
    }

}
