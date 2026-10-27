using System;

namespace Platform.XBL
{
	// Token: 0x02001C92 RID: 7314
	public class Api : XblPlatformApi
	{
		// Token: 0x17001B7E RID: 7038
		// (get) Token: 0x0600D8A1 RID: 55457 RVA: 0x004D94C2 File Offset: 0x004D76C2
		public override string SCID
		{
			get
			{
				return "00000000-0000-0000-0000-0000680ee616";
			}
		}

		// Token: 0x0600D8A2 RID: 55458 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D8A3 RID: 55459 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool InitServerApis()
		{
			return true;
		}

		// Token: 0x0600D8A4 RID: 55460 RVA: 0x000027FC File Offset: 0x000009FC
		public override void ServerApiLoaded()
		{
		}

		// Token: 0x0400A4D7 RID: 42199
		public const string s_scid = "00000000-0000-0000-0000-0000680ee616";

		// Token: 0x0400A4D8 RID: 42200
		public const int TitleId = 1745806870;
	}
}
