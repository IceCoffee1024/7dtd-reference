using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200043E RID: 1086
public class EAIDroneItemModHealWeapon : EAIDroneItemTask
{
	// Token: 0x06002122 RID: 8482 RVA: 0x000C85F0 File Offset: 0x000C67F0
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x000C85F9 File Offset: 0x000C67F9
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
	}

	// Token: 0x06002124 RID: 8484 RVA: 0x000C8604 File Offset: 0x000C6804
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

	// Token: 0x06002125 RID: 8485 RVA: 0x000C8718 File Offset: 0x000C6918
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

	// Token: 0x06002126 RID: 8486 RVA: 0x000C88E9 File Offset: 0x000C6AE9
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x06002127 RID: 8487 RVA: 0x000C88F1 File Offset: 0x000C6AF1
	public override void Reset()
	{
		base.Reset();
		this.OnAttackExit();
	}

	// Token: 0x06002128 RID: 8488 RVA: 0x000C88FF File Offset: 0x000C6AFF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackEnter()
	{
		base.OnAttackEnter();
	}

	// Token: 0x06002129 RID: 8489 RVA: 0x000C8907 File Offset: 0x000C6B07
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackAction()
	{
		base.OnAttackAction();
	}

	// Token: 0x0600212A RID: 8490 RVA: 0x000C890F File Offset: 0x000C6B0F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackExit()
	{
		base.OnAttackExit();
	}

	// Token: 0x040016DC RID: 5852
	[PublicizedFrom(EAccessModifier.Private)]
	public float healTargetRange = 15f;
}
