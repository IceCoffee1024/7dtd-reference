using System;
using Audio;
using UnityEngine;

// Token: 0x020003CC RID: 972
public class JunkSledgeFireController : MiniTurretFireController
{
	// Token: 0x06001D5B RID: 7515 RVA: 0x000B16AC File Offset: 0x000AF8AC
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

	// Token: 0x06001D5C RID: 7516 RVA: 0x000B187A File Offset: 0x000AFA7A
	public override void Fire()
	{
		this.ArmState = JunkSledgeFireController.ArmStates.Extending;
		Manager.Play(this.entityTurret, this.fireSound, 1f, true);
	}

	// Token: 0x06001D5D RID: 7517 RVA: 0x000B189C File Offset: 0x000AFA9C
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

	// Token: 0x04001354 RID: 4948
	public JunkSledgeFireController.ArmStates ArmState;

	// Token: 0x04001355 RID: 4949
	public Transform Arm1;

	// Token: 0x04001356 RID: 4950
	public float Arm1StartZ;

	// Token: 0x04001357 RID: 4951
	public float Arm1EndZ;

	// Token: 0x04001358 RID: 4952
	public Transform Arm2;

	// Token: 0x04001359 RID: 4953
	public float Arm2StartZ;

	// Token: 0x0400135A RID: 4954
	public float Arm2EndZ;

	// Token: 0x0400135B RID: 4955
	public float ExtentionTime;

	// Token: 0x0400135C RID: 4956
	public float RetractionTime;

	// Token: 0x0400135D RID: 4957
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float timeCounter;

	// Token: 0x020003CD RID: 973
	public enum ArmStates
	{
		// Token: 0x0400135F RID: 4959
		Idle,
		// Token: 0x04001360 RID: 4960
		Extending,
		// Token: 0x04001361 RID: 4961
		Retracting
	}
}
