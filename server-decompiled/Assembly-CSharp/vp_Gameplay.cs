using System;

// Token: 0x020015FE RID: 5630
public class vp_Gameplay
{
	// Token: 0x170014A3 RID: 5283
	// (get) Token: 0x0600B0F2 RID: 45298 RVA: 0x00420B6B File Offset: 0x0041ED6B
	// (set) Token: 0x0600B0F3 RID: 45299 RVA: 0x00420B7B File Offset: 0x0041ED7B
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

	// Token: 0x040084D3 RID: 34003
	public static bool isMultiplayer = false;

	// Token: 0x040084D4 RID: 34004
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool m_IsMaster = true;
}
