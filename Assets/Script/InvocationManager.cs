using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvocationManager : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] public Transform spawnPoint;
    private GameObject currentInvocation;

    private float rechargeCooldown = 10f;
    private float cooldownTimer;

    [SerializeField] GameObject[] invocationSlots = new GameObject[3];

    private enum State
    {
        Ready,
        Active,
        Cooldown
    }

    private State state = State.Ready;

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleCooldown();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (state == State.Ready)
                SpawnInvocation();
            else if(state == State.Active)
                DespawnInvocation();
        }

    }

    void SpawnInvocation()
    {
        currentInvocation = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        state = State.Active;

        currentInvocation.GetComponent<InvocationHealth>().SetManager(this);
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
        Debug.Log(newInvocation.name);
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

        if( cooldownTimer <= 0 )
            state = State.Ready;
    }

    private bool HasActiveInvocation()
    {
        return state == State.Active;
    }
}
