using System;

// Token: 0x02001329 RID: 4905
public class UpdateLightOnAllMaterials : UpdateLight
{
	// Token: 0x06009AC9 RID: 39625 RVA: 0x003AA80B File Offset: 0x003A8A0B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		if (!GameManager.IsDedicatedServer && GameLightManager.Instance != null)
		{
			GameLightManager.Instance.AddUpdateLight(this);
		}
	}

	// Token: 0x06009ACA RID: 39626 RVA: 0x003AA82C File Offset: 0x003A8A2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
		if (!GameManager.IsDedicatedServer && GameLightManager.Instance != null)
		{
			GameLightManager.Instance.RemoveUpdateLight(this);
		}
	}
}
