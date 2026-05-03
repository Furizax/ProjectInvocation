using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvocationManager : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] public Transform spawnPoint;
    GameObject currentInvocation;

   
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(currentInvocation != null)
                DespawnInvocation();
            else 
                SpawnInvocation();
        }
       
    }

    void SpawnInvocation()
    {
        currentInvocation = Instantiate(prefab,spawnPoint.position,spawnPoint.rotation);
    }

    void DespawnInvocation()
    {
        if(currentInvocation != null)
        {
            Destroy(currentInvocation);
            currentInvocation = null;
        }
    }
}
