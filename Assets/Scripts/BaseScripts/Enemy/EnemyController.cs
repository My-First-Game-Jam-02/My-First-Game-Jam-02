using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : NpcController
{
    
    protected CapsuleCollider2D capsuleCollider;
    protected SSPlayerHealth playerHealth;
    protected IState deadNpc;
    protected Vector3 originalPosition;

    public bool isPossessed;
    public enum EnemyType { GuardBot, DroneBot, RollerBot}
    public EnemyType enemyType;

    public Transform bulletSpawnPoint;
    public Bullet pfBullet;
    public float coolDownMinTime;
    public float coolDownMaxTime;
   

    public override void Awake()
    {
        base.Awake();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerHealth = FindObjectOfType<SSPlayerHealth>();
        deadNpc = new DeadNpc(this, animator, capsuleCollider);
        originalPosition = transform.position;
    }

    public void PlaceAtOriginalPosition()
    {
        transform.position = originalPosition;
    }

    public void ChangeStateToDead()
    {
        this.stateMachine.ChangeState(deadNpc);
    }




}
