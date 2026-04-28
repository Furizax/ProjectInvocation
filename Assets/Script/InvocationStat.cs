using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvocationStat : MonoBehaviour
{
    [SerializeField] public int maxHealth;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float damage;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackCooldown;
    [SerializeField] public float detectionRange;
    [SerializeField] public float Recharge; //Temps de recharge avant de pouvoir être réinvoqué

}
