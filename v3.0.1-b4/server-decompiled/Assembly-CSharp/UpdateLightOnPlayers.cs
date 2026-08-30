using System;
using UnityEngine;

// Token: 0x020012DF RID: 4831
public class UpdateLightOnPlayers : UpdateLight
{
	// Token: 0x060098D4 RID: 39124 RVA: 0x0039E850 File Offset: 0x0039CA50
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (GameManager.Instance == null || GameManager.Instance.World == null || !GameManager.Instance.gameStateManager.IsGameStarted())
		{
			return;
		}
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		if (!this.entity)
		{
			Transform transform = RootTransformRefEntity.FindEntityUpwards(base.transform);
			if (transform)
			{
				this.entity = transform.GetComponent<Entity>();
			}
		}
		else if (this.entity.emodel.IsFPV != this.lastFPV)
		{
			this.lastFPV = this.entity.emodel.IsFPV;
			this.forceUpdateFrame = Time.frameCount + 5;
			this.isForceUpdate = true;
		}
		if (this.isForceUpdate)
		{
			this.appliedLit = -1f;
			this.updateDelay = 0f;
			if (Time.frameCount >= this.forceUpdateFrame)
			{
				this.isForceUpdate = false;
			}
		}
		this.updateDelay -= Time.deltaTime;
		if (this.updateDelay > 0f)
		{
			return;
		}
		this.updateDelay = 0.05f;
		base.UpdateLighting(0.15f);
	}

	// Token: 0x060098D5 RID: 39125 RVA: 0x0039E969 File Offset: 0x0039CB69
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnApplicationFocus(bool focusStatus)
	{
		this.forceUpdateFrame = Time.frameCount + 3;
		this.isForceUpdate = true;
	}

	// Token: 0x060098D6 RID: 39126 RVA: 0x0039E97F File Offset: 0x0039CB7F
	public void ForceUpdate()
	{
		this.updateDelay = 0f;
	}

	// Token: 0x04007311 RID: 29457
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cUpdateTime = 0.05f;

	// Token: 0x04007312 RID: 29458
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isForceUpdate;

	// Token: 0x04007313 RID: 29459
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int forceUpdateFrame;

	// Token: 0x04007314 RID: 29460
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool lastFPV;

	// Token: 0x04007315 RID: 29461
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float updateDelay;
}
