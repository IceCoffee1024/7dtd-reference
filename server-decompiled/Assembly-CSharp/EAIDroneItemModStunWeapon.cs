using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200043F RID: 1087
public class EAIDroneItemModStunWeapon : EAIDroneItemTask
{
	// Token: 0x0600212C RID: 8492 RVA: 0x000C85F0 File Offset: 0x000C67F0
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
	}

	// Token: 0x0600212D RID: 8493 RVA: 0x000C85F9 File Offset: 0x000C67F9
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
	}

	// Token: 0x0600212E RID: 8494 RVA: 0x000C892C File Offset: 0x000C6B2C
	public override bool CanExecute()
	{
		if (!base.CanExecute())
		{
			return false;
		}
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return false;
		}
		if (this.drone.IsOnTeleportCooldown())
		{
			if (this.currentPath.Count > 0)
			{
				this.OnAttackExit();
			}
			return false;
		}
		if (!this.drone.CanAttack)
		{
			return false;
		}
		if (!this.drone.CanInterruptFollow)
		{
			return false;
		}
		EntityAlive owner = this.drone.Owner;
		if (owner && (this.drone.IsAttachedToVehicle(owner) || LockManager.Instance.IsLockedServer(this.drone, 0)))
		{
			return false;
		}
		if (this.drone.IsOwnerSneaking())
		{
			return false;
		}
		EntityAlive nearestEnemyInRange = this.drone.GetNearestEnemyInRange(owner ? owner.position : this.drone.position);
		if (!nearestEnemyInRange)
		{
			return false;
		}
		DroneWeapons.Weapon installedWeapon = this.drone.GetInstalledWeapon(this.ItemKey);
		if (installedWeapon.canFire())
		{
			this.drone.SetAttackTarget(nearestEnemyInRange, 1200);
			if (this.drone.GetAttackTarget())
			{
				this.drone.SetActiveWeapon(installedWeapon);
				this.drone.SetState(EntityDrone.State.Attack, true);
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600212F RID: 8495 RVA: 0x000C8A64 File Offset: 0x000C6C64
	public override bool Continue()
	{
		if (!this.drone)
		{
			return false;
		}
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return false;
		}
		EntityAlive attackTarget = this.drone.GetAttackTarget();
		if (!attackTarget || attackTarget.IsDead())
		{
			this.OnAttackExit();
			return false;
		}
		if (this.drone.WakeupAnimTime > 0f)
		{
			return false;
		}
		if (!this.drone.CanInterruptFollow)
		{
			return false;
		}
		EntityAlive owner = this.drone.Owner;
		if (owner && (this.drone.IsAttachedToVehicle(owner) || LockManager.Instance.IsLockedServer(this.drone, 0)))
		{
			this.OnAttackExit();
			return false;
		}
		DroneWeapons.Weapon installedWeapon = this.drone.GetInstalledWeapon(this.ItemKey);
		if (installedWeapon != null && this.attackEnterTimer <= 0f)
		{
			Vector3 position = this.drone.position;
			Transform transform = this.drone.transform;
			Utils.DrawCircleLinesHorzontal(position - Origin.position, this.drone.EnemyDetectionRadius, Color.white, Color.yellow, 24, 0.05f);
			float num = 0.1f;
			float num2 = 10f;
			if (!this.weaponDischarged)
			{
				Vector3 chestPosition = attackTarget.getChestPosition();
				if (base.DoMoveIntoAtkPos(attackTarget, installedWeapon.Range, transform.forward, num, true, num2) && installedWeapon.canFire())
				{
					Utils.DrawCircleLinesHorzontal(position - Origin.position, installedWeapon.Range, Color.white, Color.red, 24, 5f);
					Utils.DrawLine(position - Origin.position, chestPosition - Origin.position, Color.red, Color.red, 1, 5f);
					installedWeapon.Fire(attackTarget);
					this.weaponDischarged = true;
					base.ClearPath();
				}
			}
			else if (this.actionTimer <= 0f && base.DoFleeFromTargetEntity(attackTarget, 1.414f, num, true, num2))
			{
				this.OnAttackExit();
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002130 RID: 8496 RVA: 0x000C88E9 File Offset: 0x000C6AE9
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x06002131 RID: 8497 RVA: 0x000C88F1 File Offset: 0x000C6AF1
	public override void Reset()
	{
		base.Reset();
		this.OnAttackExit();
	}

	// Token: 0x06002132 RID: 8498 RVA: 0x000C88FF File Offset: 0x000C6AFF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackEnter()
	{
		base.OnAttackEnter();
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x000C8907 File Offset: 0x000C6B07
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackAction()
	{
		base.OnAttackAction();
	}

	// Token: 0x06002134 RID: 8500 RVA: 0x000C890F File Offset: 0x000C6B0F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackExit()
	{
		base.OnAttackExit();
	}
}
