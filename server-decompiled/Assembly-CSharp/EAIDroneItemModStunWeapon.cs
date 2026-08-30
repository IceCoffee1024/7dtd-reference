using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000437 RID: 1079
public class EAIDroneItemModStunWeapon : EAIDroneItemTask
{
	// Token: 0x0600211B RID: 8475 RVA: 0x000C7984 File Offset: 0x000C5B84
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
	}

	// Token: 0x0600211C RID: 8476 RVA: 0x000C798D File Offset: 0x000C5B8D
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
	}

	// Token: 0x0600211D RID: 8477 RVA: 0x000C7CC0 File Offset: 0x000C5EC0
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

	// Token: 0x0600211E RID: 8478 RVA: 0x000C7DF8 File Offset: 0x000C5FF8
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

	// Token: 0x0600211F RID: 8479 RVA: 0x000C7C7D File Offset: 0x000C5E7D
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x06002120 RID: 8480 RVA: 0x000C7C85 File Offset: 0x000C5E85
	public override void Reset()
	{
		base.Reset();
		this.OnAttackExit();
	}

	// Token: 0x06002121 RID: 8481 RVA: 0x000C7C93 File Offset: 0x000C5E93
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackEnter()
	{
		base.OnAttackEnter();
	}

	// Token: 0x06002122 RID: 8482 RVA: 0x000C7C9B File Offset: 0x000C5E9B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackAction()
	{
		base.OnAttackAction();
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x000C7CA3 File Offset: 0x000C5EA3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackExit()
	{
		base.OnAttackExit();
	}
}
