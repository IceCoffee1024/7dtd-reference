using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200043D RID: 1085
public class EAIDroneItemModStunWeapon : EAIDroneItemTask
{
	// Token: 0x06002127 RID: 8487 RVA: 0x000C8244 File Offset: 0x000C6444
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
	}

	// Token: 0x06002128 RID: 8488 RVA: 0x000C824D File Offset: 0x000C644D
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
	}

	// Token: 0x06002129 RID: 8489 RVA: 0x000C8580 File Offset: 0x000C6780
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

	// Token: 0x0600212A RID: 8490 RVA: 0x000C86B8 File Offset: 0x000C68B8
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

	// Token: 0x0600212B RID: 8491 RVA: 0x000C853D File Offset: 0x000C673D
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x0600212C RID: 8492 RVA: 0x000C8545 File Offset: 0x000C6745
	public override void Reset()
	{
		base.Reset();
		this.OnAttackExit();
	}

	// Token: 0x0600212D RID: 8493 RVA: 0x000C8553 File Offset: 0x000C6753
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackEnter()
	{
		base.OnAttackEnter();
	}

	// Token: 0x0600212E RID: 8494 RVA: 0x000C855B File Offset: 0x000C675B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackAction()
	{
		base.OnAttackAction();
	}

	// Token: 0x0600212F RID: 8495 RVA: 0x000C8563 File Offset: 0x000C6763
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackExit()
	{
		base.OnAttackExit();
	}
}
