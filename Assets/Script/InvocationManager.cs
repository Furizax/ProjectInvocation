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
        SelectInvocation(0);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
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

        state = State.Active;
    }

    void DespawnInvocation()
    {
        if (currentInvocation != null)
        {
            Destroy(currentInvocation);
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
                invocationSlots[i] = newInvocation;
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

    public void OnInvocationDeath()
    {
        currentInvocation = null;
        state = State.Ready;
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
