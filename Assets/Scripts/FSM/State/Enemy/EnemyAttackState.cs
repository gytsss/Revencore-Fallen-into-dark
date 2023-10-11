using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private const string attackAnimation = "MinionHit";
    public Action OnEnemyShoot;
    
    public EnemyAttackState(string name, State_Machine stateMachine, EnemyComponent enemy) : base(name, stateMachine,enemy)
    {
        
    }
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //enemy.target.GetComponent<PlayerComponent>().
        if (enemy.IsRangedEnemy)
        {
            DistanceAttack();
        }
    }

    private void DistanceAttack()
    {
        float _distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.target.transform.position);

        if (_distanceToPlayer > enemy.stopDistance)
        {
            Vector3 targetPosition = new Vector3(enemy.target.transform.position.x, enemy.transform.position.y, enemy.target.transform.position.z);
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPosition, enemy.speed * Time.deltaTime);
        }
        else if (_distanceToPlayer <= enemy.stopDistance && Mathf.Approximately(enemy.transform.position.z, enemy.target.transform.position.z))
        {
            Vector3 direction = enemy.target.transform.position - enemy.transform.position;
            direction.y = 0f;

            if (direction.x > 0)
                enemy.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            else
                enemy.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
             
            Quaternion rot = Quaternion.LookRotation(-enemy.transform.right);

            if (!enemy.IsAttacking)
            {
                OnEnemyShoot.Invoke();
            }

        }
        else if(!Mathf.Approximately(enemy.transform.position.x, enemy.target.transform.position.x))
        {
            Vector3 targetPosition = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.target.transform.position.z);
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPosition, enemy.speed * Time.deltaTime);
        }

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }
    
    private void playAttackAnimation()
    {
        // enemy.anim.Play(moveAnimation);
    }

    public override void AddStateTransitions(string transitionName, State transitionState)
    {
        base.AddStateTransitions(transitionName, transitionState);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
