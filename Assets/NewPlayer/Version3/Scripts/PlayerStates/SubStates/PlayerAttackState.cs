using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerGroundedState
{
    private bool gotInput, isFirstAttack;
    private float AttackInputStartTime;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseAttackInput();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        AttackInputStartTime = Time.time;
        player.Anim.SetBool("canAttack", true);
        
        if(player.CheckIfGrounded()) {
            // set animatinos
            isFirstAttack = true;
            player.Anim.SetBool("attack1", true);
        }
        player.Anim.SetBool("firstAttack", isFirstAttack);
        player.Anim.SetBool("isAttacking", true);
    }

    public bool CanAttack()
    {
        return Time.time >= AttackInputStartTime + 0.5f;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
