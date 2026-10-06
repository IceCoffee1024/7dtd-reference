using System;

// Token: 0x02001638 RID: 5688
public class vp_Gameplay
{
	// Token: 0x1700151F RID: 5407
	// (get) Token: 0x0600B281 RID: 45697 RVA: 0x00429B87 File Offset: 0x00427D87
	// (set) Token: 0x0600B282 RID: 45698 RVA: 0x00429B97 File Offset: 0x00427D97
	public static bool isMaster
	{
		get
		{
			return !vp_Gameplay.isMultiplayer || vp_Gameplay.m_IsMaster;
		}
		set
		{
			if (!vp_Gameplay.isMultiplayer)
			{
				return;
			}
			vp_Gameplay.m_IsMaster = value;
		}
	}

	// Token: 0x04008602 RID: 34306
	public static bool isMultiplayer = false;

	// Token: 0x04008603 RID: 34307
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool m_IsMaster = true;
}
