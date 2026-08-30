using System;
using Platform.XBL;

namespace Platform.GameCore
{
	// Token: 0x02001C78 RID: 7288
	public class Api : XblPlatformApi
	{
		// Token: 0x17001AD0 RID: 6864
		// (get) Token: 0x0600D825 RID: 55333 RVA: 0x004DB942 File Offset: 0x004D9B42
		public override string SCID
		{
			get
			{
				return this.scid;
			}
		}

		// Token: 0x0600D826 RID: 55334 RVA: 0x004DB94A File Offset: 0x004D9B4A
		public override void Init(IPlatform owner)
		{
			Log.Out("[XBL] API Startup. SCID: " + this.SCID);
		}

		// Token: 0x0600D827 RID: 55335 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool InitServerApis()
		{
			return false;
		}

		// Token: 0x0600D828 RID: 55336 RVA: 0x000027FC File Offset: 0x000009FC
		public override void ServerApiLoaded()
		{
		}

		// Token: 0x0400A447 RID: 42055
		[PublicizedFrom(EAccessModifier.Private)]
		public string scid;
	}
}
