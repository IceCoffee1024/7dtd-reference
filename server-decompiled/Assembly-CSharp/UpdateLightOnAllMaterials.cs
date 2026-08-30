using System;

// Token: 0x020012DD RID: 4829
public class UpdateLightOnAllMaterials : UpdateLight
{
	// Token: 0x060098CA RID: 39114 RVA: 0x0039E5BF File Offset: 0x0039C7BF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		if (!GameManager.IsDedicatedServer && GameLightManager.Instance != null)
		{
			GameLightManager.Instance.AddUpdateLight(this);
		}
	}

	// Token: 0x060098CB RID: 39115 RVA: 0x0039E5E0 File Offset: 0x0039C7E0
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
