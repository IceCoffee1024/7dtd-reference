using System;

// Token: 0x0200164A RID: 5706
public class vp_Gameplay
{
	// Token: 0x1700152B RID: 5419
	// (get) Token: 0x0600B2D9 RID: 45785 RVA: 0x0042BF8B File Offset: 0x0042A18B
	// (set) Token: 0x0600B2DA RID: 45786 RVA: 0x0042BF9B File Offset: 0x0042A19B
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

	// Token: 0x04008643 RID: 34371
	public static bool isMultiplayer = false;

	// Token: 0x04008644 RID: 34372
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool m_IsMaster = true;
}
