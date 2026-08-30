using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000436 RID: 1078
public class EAIDroneItemModHealWeapon : EAIDroneItemTask
{
	// Token: 0x06002111 RID: 8465 RVA: 0x000C7984 File Offset: 0x000C5B84
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
	}

	// Token: 0x06002112 RID: 8466 RVA: 0x000C798D File Offset: 0x000C5B8D
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
	}

	// Token: 0x06002113 RID: 8467 RVA: 0x000C7998 File Offset: 0x000C5B98
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

	// Token: 0x06002114 RID: 8468 RVA: 0x000C7AAC File Offset: 0x000C5CAC
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

	// Token: 0x06002115 RID: 8469 RVA: 0x000C7C7D File Offset: 0x000C5E7D
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x06002116 RID: 8470 RVA: 0x000C7C85 File Offset: 0x000C5E85
	public override void Reset()
	{
		base.Reset();
		this.OnAttackExit();
	}

	// Token: 0x06002117 RID: 8471 RVA: 0x000C7C93 File Offset: 0x000C5E93
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackEnter()
	{
		base.OnAttackEnter();
	}

	// Token: 0x06002118 RID: 8472 RVA: 0x000C7C9B File Offset: 0x000C5E9B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackAction()
	{
		base.OnAttackAction();
	}

	// Token: 0x06002119 RID: 8473 RVA: 0x000C7CA3 File Offset: 0x000C5EA3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnAttackExit()
	{
		base.OnAttackExit();
	}

	// Token: 0x040016C0 RID: 5824
	[PublicizedFrom(EAccessModifier.Private)]
	public float healTargetRange = 15f;
}
