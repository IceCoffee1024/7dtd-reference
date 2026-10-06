using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004E0 RID: 1248
[Preserve]
public class EntitySupplyPlane : Entity
{
	// Token: 0x06002863 RID: 10339 RVA: 0x000EC025 File Offset: 0x000EA225
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06002864 RID: 10340 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsDeadIfOutOfWorld()
	{
		return false;
	}

	// Token: 0x06002865 RID: 10341 RVA: 0x000FAE3F File Offset: 0x000F903F
	public void SetDirectionToFly(Vector3 _directionToFly, int _ticksToFly)
	{
		this.ticksToFly = _ticksToFly;
		this.motion = _directionToFly * 6f;
		this.IsMovementReplicated = false;
	}

	// Token: 0x06002866 RID: 10342 RVA: 0x000FAE60 File Offset: 0x000F9060
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

	// Token: 0x06002867 RID: 10343 RVA: 0x000FAECC File Offset: 0x000F90CC
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

	// Token: 0x06002868 RID: 10344 RVA: 0x000FAF48 File Offset: 0x000F9148
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

	// Token: 0x06002869 RID: 10345 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsSavedToFile()
	{
		return false;
	}

	// Token: 0x0600286A RID: 10346 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanCollideWithBlocks()
	{
		return false;
	}

	// Token: 0x04001E58 RID: 7768
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int ticksToFly;

	// Token: 0x04001E59 RID: 7769
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isPlayedSound;

	// Token: 0x04001E5A RID: 7770
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera mainCamera;

	// Token: 0x04001E5B RID: 7771
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter planeMF;

	// Token: 0x04001E5C RID: 7772
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Mesh planeMesh;
}
