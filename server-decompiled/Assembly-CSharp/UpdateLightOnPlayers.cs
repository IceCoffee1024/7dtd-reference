using System;
using UnityEngine;

// Token: 0x020012F9 RID: 4857
public class UpdateLightOnPlayers : UpdateLight
{
	// Token: 0x060099CF RID: 39375 RVA: 0x003A3AE8 File Offset: 0x003A1CE8
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

	// Token: 0x060099D0 RID: 39376 RVA: 0x003A3C01 File Offset: 0x003A1E01
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnApplicationFocus(bool focusStatus)
	{
		this.forceUpdateFrame = Time.frameCount + 3;
		this.isForceUpdate = true;
	}

	// Token: 0x060099D1 RID: 39377 RVA: 0x003A3C17 File Offset: 0x003A1E17
	public void ForceUpdate()
	{
		this.updateDelay = 0f;
	}

	// Token: 0x040073E9 RID: 29673
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cUpdateTime = 0.05f;

	// Token: 0x040073EA RID: 29674
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isForceUpdate;

	// Token: 0x040073EB RID: 29675
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int forceUpdateFrame;

	// Token: 0x040073EC RID: 29676
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool lastFPV;

	// Token: 0x040073ED RID: 29677
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float updateDelay;
}
