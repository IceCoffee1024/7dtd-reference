using System;

namespace Platform.XBL
{
	// Token: 0x02001C2F RID: 7215
	public class Api : XblPlatformApi
	{
		// Token: 0x17001A90 RID: 6800
		// (get) Token: 0x0600D615 RID: 54805 RVA: 0x004D0EB2 File Offset: 0x004CF0B2
		public override string SCID
		{
			get
			{
				return "00000000-0000-0000-0000-0000680ee616";
			}
		}

		// Token: 0x0600D616 RID: 54806 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D617 RID: 54807 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool InitServerApis()
		{
			return true;
		}

		// Token: 0x0600D618 RID: 54808 RVA: 0x000027FC File Offset: 0x000009FC
		public override void ServerApiLoaded()
		{
		}

		// Token: 0x0400A319 RID: 41753
		public const string s_scid = "00000000-0000-0000-0000-0000680ee616";

		// Token: 0x0400A31A RID: 41754
		public const int TitleId = 1745806870;
	}
}
