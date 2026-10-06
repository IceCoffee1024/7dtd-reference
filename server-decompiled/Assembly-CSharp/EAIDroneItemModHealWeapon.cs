using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200043C RID: 1084
public class EAIDroneItemModHealWeapon : EAIDroneItemTask
{
	// Token: 0x0600211D RID: 8477 RVA: 0x000C8244 File Offset: 0x000C6444
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
	}

	// Token: 0x0600211E RID: 8478 RVA: 0x000C824D File Offset: 0x000C644D
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
	}

	// Token: 0x0600211F RID: 8479 RVA: 0x000C8258 File Offset: 0x000C6458
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
		if (this.drone.Owner && LockManager.Instance.IsLockedServer(this.drone, 0))
		{
			return false;
		}
		EntityAlive nearestHealTargetInRange = this.drone.GetNearestHealTargetInRange(this.healTargetRange);
		if (!nearestHealTargetInRange)
		{
			return false;
		}
		if (!this.drone.TargetCanBeHealed(nearestHealTargetInRange))
		{
			return false;
		}
		if (!this.drone.IsTargetInNeedOfMedical(nearestHealTargetInRange))
		{
			return false;
		}
		DroneWeapons.Weapon installedWeapon = this.drone.GetInstalledWeapon(this.ItemKey);
		if (installedWeapon.canFire())
		{
			this.drone.SetAttackTarget(nearestHealTargetInRange, 1200);
			if (this.drone.GetAttackTarget())
			{
				this.drone.SetActiveWeapon(installedWeapon);
				this.drone.SetState(EntityDrone.State.Heal, true);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002120 RID: 8480 RVA: 0x000C836C File Offset: 0x000C656C
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
		DroneWeapons.Weapon installedWeapon = this.drone.GetInstalledWeapon(this.ItemKey);
		EntityAlive attackTarget2 = this.drone.GetAttackTarget();
		if (attackTarget2 && attackTarget2.AttachedToEntity as EntityVehicle != null)
		{
			if (installedWeapon != null)
			{
				installedWeapon.Fire(attackTarget);
			}
			this.OnAttackExit();
			return false;
		}
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
			else if (this.actionTimer <= 0f)
			{
				this.OnAttackExit();
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002121 RID: 8481 RVA: 0x000C853D File Offset: 0x000C673D
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x06002122 RID: 8482 RVA: 0x000C8545 File Offset: 0x000C6745
	public override void Reset()
	{
		base.Reset();
		this.OnAttackExit();
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x000C8553 File Offset: 0x000C6753
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackEnter()
	{
		base.OnAttackEnter();
	}

	// Token: 0x06002124 RID: 8484 RVA: 0x000C855B File Offset: 0x000C675B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackAction()
	{
		base.OnAttackAction();
	}

	// Token: 0x06002125 RID: 8485 RVA: 0x000C8563 File Offset: 0x000C6763
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackExit()
	{
		base.OnAttackExit();
	}

	// Token: 0x040016CC RID: 5836
	[PublicizedFrom(EAccessModifier.Private)]
	public float healTargetRange = 15f;
}
