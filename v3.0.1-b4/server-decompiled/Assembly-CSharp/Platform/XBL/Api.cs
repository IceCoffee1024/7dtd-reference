using System;

namespace Platform.XBL
{
	// Token: 0x02001C08 RID: 7176
	public class Api : XblPlatformApi
	{
		// Token: 0x17001A1E RID: 6686
		// (get) Token: 0x0600D487 RID: 54407 RVA: 0x004CAABE File Offset: 0x004C8CBE
		public override string SCID
		{
			get
			{
				return "00000000-0000-0000-0000-0000680ee616";
			}
		}

		// Token: 0x0600D488 RID: 54408 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D489 RID: 54409 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool InitServerApis()
		{
			return true;
		}

		// Token: 0x0600D48A RID: 54410 RVA: 0x000027FC File Offset: 0x000009FC
		public override void ServerApiLoaded()
		{
		}

		// Token: 0x0400A1E8 RID: 41448
		public const string s_scid = "00000000-0000-0000-0000-0000680ee616";

		// Token: 0x0400A1E9 RID: 41449
		public const int TitleId = 1745806870;
	}
}
