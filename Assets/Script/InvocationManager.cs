using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvocationManager : MonoBehaviour
{
    [SerializeField] GameObject baseInvocation;
    [SerializeField] public Transform spawnPoint;
    private GameObject currentInvocation;

    private float rechargeCooldown = 10f;
    private float cooldownTimer;

    [SerializeField] GameObject[] invocationSlots;
    public GameObject equippedInvocation;

    private enum State
    {
        Ready,
        Active,
        Cooldown
    }

    private State state = State.Ready;

    private void Start()
    {
        Debug.Log("=== DEBUG INVOCATION MANAGER ===");
        Debug.Log("Object: " + gameObject.name);

        Debug.Log("BaseInvocation: " + baseInvocation);

        if (invocationSlots == null)
            Debug.Log("Slots ARRAY = NULL");
        else
            Debug.Log("Slots SIZE = " + invocationSlots.Length);

        if (invocationSlots.Length > 0)
            Debug.Log("Slot0 BEFORE: " + invocationSlots[0]);

        invocationSlots[0] = baseInvocation;

        Debug.Log("Slot0 AFTER: " + invocationSlots[0]);

        SelectInvocation(0);
    }

    private void Awake()
    {
        Debug.Log("Manager on: " + gameObject.name);
        Debug.Log("Is prefab instance? " + gameObject.scene.name);
        Debug.Log("baseInvocation: " + baseInvocation);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleCooldown();
    }

    void HandleInput()
    {
        //Selection des invocations
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectInvocation(0);
        if(Input.GetKeyDown(KeyCode.Alpha2)) SelectInvocation(1);
        if( Input.GetKeyDown(KeyCode.Alpha3)) SelectInvocation(2);

        // Touche Alpha1 dédiée à l'action de Spawn / Despawn de l'élément équipé
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
        if (state != State.Ready) return;

        if(equippedInvocation == null)
        {
            Debug.LogWarning("No invocation equipped");
            return;
        }

        currentInvocation = Instantiate(equippedInvocation, spawnPoint.position, spawnPoint.rotation);
        state = State.Active;

        Debug.Log($"Spawned: {currentInvocation.name}");
        currentInvocation
           .GetComponent<InvocationHealth>()
           .SetManager(this);
    }

    void DespawnInvocation()
    {
        if (currentInvocation != null)
        {
            Destroy(currentInvocation);
            currentInvocation = null;
        }
        //Empêche de considérer l'invocation comme morte et mettre le cooldown
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
            Debug.Log("Empty slot");
            return;
        }

        equippedInvocation = invocationSlots[slotIndex];
        Debug.Log($"Equipped: {equippedInvocation.name}");
    }

    public void OnInvocationDeath()
    {
        currentInvocation = null;
        state = State.Cooldown;
        cooldownTimer = rechargeCooldown;
    }

    private void HandleCooldown()
    {
        if (state != State.Cooldown)
            return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0)
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
}
