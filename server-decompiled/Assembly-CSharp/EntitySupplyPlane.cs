using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004E2 RID: 1250
[Preserve]
public class EntitySupplyPlane : Entity
{
	// Token: 0x0600286A RID: 10346 RVA: 0x000EC435 File Offset: 0x000EA635
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x0600286B RID: 10347 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsDeadIfOutOfWorld()
	{
		return false;
	}

	// Token: 0x0600286C RID: 10348 RVA: 0x000FB377 File Offset: 0x000F9577
	public void SetDirectionToFly(Vector3 _directionToFly, int _ticksToFly)
	{
		this.ticksToFly = _ticksToFly;
		this.motion = _directionToFly * 6f;
		this.IsMovementReplicated = false;
	}

	// Token: 0x0600286D RID: 10349 RVA: 0x000FB398 File Offset: 0x000F9598
	[PublicizedFrom(EAccessModifier.Private)]
	public void MoveBoundsInsideFrustrum(Transform _parentT)
	{
		if (!this.planeMesh)
		{
			return;
		}
		float magnitude = (this.mainCamera.transform.position - _parentT.position).magnitude;
		Vector3 vector = Vector3.one * (magnitude * 1.25f);
		Vector3 zero = Vector3.zero;
		this.planeMesh.bounds = new Bounds(zero, vector);
	}

	// Token: 0x0600286E RID: 10350 RVA: 0x000FB404 File Offset: 0x000F9604
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateFarDraw()
	{
		if (!this.mainCamera)
		{
			this.mainCamera = Camera.main;
			if (!this.mainCamera)
			{
				return;
			}
		}
		if (!this.planeMesh)
		{
			this.planeMF = base.transform.GetComponentInChildren<MeshFilter>();
			if (this.planeMF)
			{
				this.planeMesh = this.planeMF.mesh;
			}
		}
		this.MoveBoundsInsideFrustrum(base.transform);
	}

	// Token: 0x0600286F RID: 10351 RVA: 0x000FB480 File Offset: 0x000F9680
	public override void OnUpdatePosition(float _partialTicks)
	{
		base.OnUpdatePosition(_partialTicks);
		this.UpdateFarDraw();
		this.interpolateTargetRot = 0;
		this.position += this.motion * _partialTicks;
		if (!this.isEntityRemote)
		{
			int num = this.ticksToFly - 1;
			this.ticksToFly = num;
			if (num <= 0)
			{
				this.MarkToUnload();
			}
		}
		if (!this.isPlayedSound)
		{
			Manager.Play(this, "SupplyDrops/Supply_Crate_Plane_lp", 1f, false);
			this.isPlayedSound = true;
		}
		base.SetAirBorne(true);
	}

	// Token: 0x06002870 RID: 10352 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsSavedToFile()
	{
		return false;
	}

	// Token: 0x06002871 RID: 10353 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanCollideWithBlocks()
	{
		return false;
	}

	// Token: 0x04001E68 RID: 7784
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int ticksToFly;

	// Token: 0x04001E69 RID: 7785
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isPlayedSound;

	// Token: 0x04001E6A RID: 7786
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera mainCamera;

	// Token: 0x04001E6B RID: 7787
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter planeMF;

	// Token: 0x04001E6C RID: 7788
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Mesh planeMesh;
}
