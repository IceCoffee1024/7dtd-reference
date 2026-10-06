using System;
using Audio;
using UnityEngine;

// Token: 0x020003D3 RID: 979
public class JunkSledgeFireController : MiniTurretFireController
{
	// Token: 0x06001D70 RID: 7536 RVA: 0x000B1FFC File Offset: 0x000B01FC
	public new void Update()
	{
		base.Update();
		if (this.ArmState == JunkSledgeFireController.ArmStates.Idle)
		{
			return;
		}
		if (this.ArmState == JunkSledgeFireController.ArmStates.Extending)
		{
			float num = Mathf.Clamp01(this.timeCounter / (this.fireRateMax * 0.5f * 0.25f));
			this.Arm1.localPosition = new Vector3(this.Arm1.localPosition.x, this.Arm1.localPosition.y, Mathf.Lerp(this.Arm1StartZ, this.Arm1EndZ, num));
			this.Arm2.localPosition = new Vector3(this.Arm2.localPosition.x, this.Arm2.localPosition.y, Mathf.Lerp(this.Arm2StartZ, this.Arm2EndZ, num));
			if (num >= 1f)
			{
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					this.hitTarget();
				}
				this.ArmState = JunkSledgeFireController.ArmStates.Retracting;
				this.timeCounter = 0f;
			}
		}
		else
		{
			float num2 = Mathf.Clamp01(this.timeCounter / (this.fireRateMax * 0.5f * 0.75f));
			this.Arm1.localPosition = new Vector3(this.Arm1.localPosition.x, this.Arm1.localPosition.y, Mathf.Lerp(this.Arm1EndZ, this.Arm1StartZ, num2));
			this.Arm2.localPosition = new Vector3(this.Arm2.localPosition.x, this.Arm2.localPosition.y, Mathf.Lerp(this.Arm2EndZ, this.Arm2StartZ, num2));
			if (num2 >= 1f)
			{
				this.ArmState = JunkSledgeFireController.ArmStates.Idle;
				this.timeCounter = 0f;
			}
		}
		this.timeCounter += Time.deltaTime;
	}

	// Token: 0x06001D71 RID: 7537 RVA: 0x000B21CA File Offset: 0x000B03CA
	public override void Fire()
	{
		this.ArmState = JunkSledgeFireController.ArmStates.Extending;
		Manager.Play(this.entityTurret, this.fireSound, 1f, true);
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x000B21EC File Offset: 0x000B03EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void hitTarget()
	{
		Vector3 position = this.Cone.transform.position;
		EntityAlive entityAlive = GameManager.Instance.World.GetEntity(this.entityTurret.belongsPlayerId) as EntityAlive;
		float maxDistance = base.MaxDistance;
		Vector3 vector = this.Cone.transform.forward;
		vector *= -1f;
		Ray ray = new Ray(position + Origin.position, vector);
		Voxel.Raycast(GameManager.Instance.World, ray, maxDistance, -538750989, 128, 0.15f);
		ItemActionAttack.Hit(Voxel.voxelRayHitInfo.Clone(), this.entityTurret.belongsPlayerId, EnumDamageTypes.Bashing, base.GetDamageBlock(this.entityTurret.OriginalItemValue, BlockValue.Air, entityAlive, 1), base.GetDamageEntity(this.entityTurret.OriginalItemValue, entityAlive, 1), 1f, this.entityTurret.OriginalItemValue.PercentUsesLeft, 0f, 0f, "metal", this.damageMultiplier, this.buffActions, new ItemActionAttack.AttackHitInfo(), 1, 0, 0f, null, null, ItemActionAttack.EnumAttackMode.RealNoHarvesting, null, this.entityTurret.entityId, this.entityTurret.OriginalItemValue, false, false, false, null);
	}

	// Token: 0x04001362 RID: 4962
	public JunkSledgeFireController.ArmStates ArmState;

	// Token: 0x04001363 RID: 4963
	public Transform Arm1;

	// Token: 0x04001364 RID: 4964
	public float Arm1StartZ;

	// Token: 0x04001365 RID: 4965
	public float Arm1EndZ;

	// Token: 0x04001366 RID: 4966
	public Transform Arm2;

	// Token: 0x04001367 RID: 4967
	public float Arm2StartZ;

	// Token: 0x04001368 RID: 4968
	public float Arm2EndZ;

	// Token: 0x04001369 RID: 4969
	public float ExtentionTime;

	// Token: 0x0400136A RID: 4970
	public float RetractionTime;

	// Token: 0x0400136B RID: 4971
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float timeCounter;

	// Token: 0x020003D4 RID: 980
	public enum ArmStates
	{
		// Token: 0x0400136D RID: 4973
		Idle,
		// Token: 0x0400136E RID: 4974
		Extending,
		// Token: 0x0400136F RID: 4975
		Retracting
	}
}
