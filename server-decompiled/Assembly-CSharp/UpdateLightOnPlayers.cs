using System;
using UnityEngine;

// Token: 0x0200132B RID: 4907
public class UpdateLightOnPlayers : UpdateLight
{
	// Token: 0x06009AD3 RID: 39635 RVA: 0x003AAA9C File Offset: 0x003A8C9C
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

	// Token: 0x06009AD4 RID: 39636 RVA: 0x003AABB5 File Offset: 0x003A8DB5
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnApplicationFocus(bool focusStatus)
	{
		this.forceUpdateFrame = Time.frameCount + 3;
		this.isForceUpdate = true;
	}

	// Token: 0x06009AD5 RID: 39637 RVA: 0x003AABCB File Offset: 0x003A8DCB
	public void ForceUpdate()
	{
		this.updateDelay = 0f;
	}

	// Token: 0x040074A5 RID: 29861
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cUpdateTime = 0.05f;

	// Token: 0x040074A6 RID: 29862
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isForceUpdate;

	// Token: 0x040074A7 RID: 29863
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int forceUpdateFrame;

	// Token: 0x040074A8 RID: 29864
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool lastFPV;

	// Token: 0x040074A9 RID: 29865
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float updateDelay;
}
