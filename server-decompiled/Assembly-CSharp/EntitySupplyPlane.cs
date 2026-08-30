using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004D3 RID: 1235
[Preserve]
public class EntitySupplyPlane : Entity
{
	// Token: 0x06002833 RID: 10291 RVA: 0x000EB2D9 File Offset: 0x000E94D9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06002834 RID: 10292 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsDeadIfOutOfWorld()
	{
		return false;
	}

	// Token: 0x06002835 RID: 10293 RVA: 0x000F930A File Offset: 0x000F750A
	public void SetDirectionToFly(Vector3 _directionToFly, int _ticksToFly)
	{
		this.ticksToFly = _ticksToFly;
		this.motion = _directionToFly * 6f;
		this.IsMovementReplicated = false;
	}

	// Token: 0x06002836 RID: 10294 RVA: 0x000F932C File Offset: 0x000F752C
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

	// Token: 0x06002837 RID: 10295 RVA: 0x000F9398 File Offset: 0x000F7598
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

	// Token: 0x06002838 RID: 10296 RVA: 0x000F9414 File Offset: 0x000F7614
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

	// Token: 0x06002839 RID: 10297 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsSavedToFile()
	{
		return false;
	}

	// Token: 0x0600283A RID: 10298 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanCollideWithBlocks()
	{
		return false;
	}

	// Token: 0x04001E22 RID: 7714
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int ticksToFly;

	// Token: 0x04001E23 RID: 7715
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isPlayedSound;

	// Token: 0x04001E24 RID: 7716
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera mainCamera;

	// Token: 0x04001E25 RID: 7717
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter planeMF;

	// Token: 0x04001E26 RID: 7718
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Mesh planeMesh;
}
