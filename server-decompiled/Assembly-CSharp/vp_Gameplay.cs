using System;

// Token: 0x02001619 RID: 5657
public class vp_Gameplay
{
	// Token: 0x170014DC RID: 5340
	// (get) Token: 0x0600B1EF RID: 45551 RVA: 0x00425EAB File Offset: 0x004240AB
	// (set) Token: 0x0600B1F0 RID: 45552 RVA: 0x00425EBB File Offset: 0x004240BB
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

	// Token: 0x040085AC RID: 34220
	public static bool isMultiplayer = false;

	// Token: 0x040085AD RID: 34221
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool m_IsMaster = true;
}
