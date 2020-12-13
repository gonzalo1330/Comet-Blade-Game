using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirAttackState : PlayerAbilityState
{
    private bool gotInput, isFirstAttack;
    private float AttackInputStartTime;

    public PlayerAirAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
        player.Anim.SetBool("attack2", true);
        player.Anim.SetBool("firstAttack", false);
        player.Anim.SetBool("isAttacking", true);
        player.PS.Launch();
        isAbilityDone = true;
        
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
