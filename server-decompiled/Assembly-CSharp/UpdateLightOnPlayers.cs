using System;
using UnityEngine;

// Token: 0x02001319 RID: 4889
public class UpdateLightOnPlayers : UpdateLight
{
	// Token: 0x06009A7B RID: 39547 RVA: 0x003A8698 File Offset: 0x003A6898
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

	// Token: 0x06009A7C RID: 39548 RVA: 0x003A87B1 File Offset: 0x003A69B1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnApplicationFocus(bool focusStatus)
	{
		this.forceUpdateFrame = Time.frameCount + 3;
		this.isForceUpdate = true;
	}

	// Token: 0x06009A7D RID: 39549 RVA: 0x003A87C7 File Offset: 0x003A69C7
	public void ForceUpdate()
	{
		this.updateDelay = 0f;
	}

	// Token: 0x04007464 RID: 29796
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cUpdateTime = 0.05f;

	// Token: 0x04007465 RID: 29797
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isForceUpdate;

	// Token: 0x04007466 RID: 29798
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int forceUpdateFrame;

	// Token: 0x04007467 RID: 29799
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool lastFPV;

	// Token: 0x04007468 RID: 29800
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float updateDelay;
}
