using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulPickup : MonoBehaviour
{
   public GameObject invocationPrefab;
    InvocationManager invocationManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            invocationManager = collision.gameObject.GetComponent<InvocationManager>();
            Debug.Log(invocationManager);
            invocationManager.AddInvocation(invocationPrefab);
            
            Destroy(gameObject);
        }
    }
}
