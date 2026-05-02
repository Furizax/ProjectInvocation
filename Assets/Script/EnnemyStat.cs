using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyStat : MonoBehaviour
{
    [SerializeField] public int maxHealth;
    [SerializeField] public int damage;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float detectionRange;
    [SerializeField] public float chaseDistance;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackDelay; 
}
