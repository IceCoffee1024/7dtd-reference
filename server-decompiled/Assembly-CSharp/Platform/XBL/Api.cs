using System;

namespace Platform.XBL
{
	// Token: 0x02001C61 RID: 7265
	public class Api : XblPlatformApi
	{
		// Token: 0x17001B36 RID: 6966
		// (get) Token: 0x0600D78D RID: 55181 RVA: 0x004D599A File Offset: 0x004D3B9A
		public override string SCID
		{
			get
			{
				return "00000000-0000-0000-0000-0000680ee616";
			}
		}

		// Token: 0x0600D78E RID: 55182 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D78F RID: 55183 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool InitServerApis()
		{
			return true;
		}

		// Token: 0x0600D790 RID: 55184 RVA: 0x000027FC File Offset: 0x000009FC
		public override void ServerApiLoaded()
		{
		}

		// Token: 0x0400A3EE RID: 41966
		public const string s_scid = "00000000-0000-0000-0000-0000680ee616";

		// Token: 0x0400A3EF RID: 41967
		public const int TitleId = 1745806870;
	}
}
