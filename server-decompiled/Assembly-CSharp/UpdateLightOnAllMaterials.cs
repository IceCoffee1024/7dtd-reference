using System;

// Token: 0x02001317 RID: 4887
public class UpdateLightOnAllMaterials : UpdateLight
{
	// Token: 0x06009A71 RID: 39537 RVA: 0x003A8407 File Offset: 0x003A6607
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		if (!GameManager.IsDedicatedServer && GameLightManager.Instance != null)
		{
			GameLightManager.Instance.AddUpdateLight(this);
		}
	}

	// Token: 0x06009A72 RID: 39538 RVA: 0x003A8428 File Offset: 0x003A6628
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
