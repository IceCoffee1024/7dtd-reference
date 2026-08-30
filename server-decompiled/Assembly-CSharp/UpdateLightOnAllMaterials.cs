using System;

// Token: 0x020012F7 RID: 4855
public class UpdateLightOnAllMaterials : UpdateLight
{
	// Token: 0x060099C5 RID: 39365 RVA: 0x003A3857 File Offset: 0x003A1A57
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		if (!GameManager.IsDedicatedServer && GameLightManager.Instance != null)
		{
			GameLightManager.Instance.AddUpdateLight(this);
		}
	}

	// Token: 0x060099C6 RID: 39366 RVA: 0x003A3878 File Offset: 0x003A1A78
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
