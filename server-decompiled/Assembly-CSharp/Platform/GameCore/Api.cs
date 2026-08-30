using System;
using Platform.XBL;

namespace Platform.GameCore
{
	// Token: 0x02001C51 RID: 7249
	public class Api : XblPlatformApi
	{
		// Token: 0x17001A5E RID: 6750
		// (get) Token: 0x0600D695 RID: 54933 RVA: 0x004D551E File Offset: 0x004D371E
		public override string SCID
		{
			get
			{
				return this.scid;
			}
		}

		// Token: 0x0600D696 RID: 54934 RVA: 0x004D5526 File Offset: 0x004D3726
		public override void Init(IPlatform owner)
		{
			Log.Out("[XBL] API Startup. SCID: " + this.SCID);
		}

		// Token: 0x0600D697 RID: 54935 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool InitServerApis()
		{
			return false;
		}

		// Token: 0x0600D698 RID: 54936 RVA: 0x000027FC File Offset: 0x000009FC
		public override void ServerApiLoaded()
		{
		}

		// Token: 0x0400A316 RID: 41750
		[PublicizedFrom(EAccessModifier.Private)]
		public string scid;
	}
}
