using System;
using Platform.XBL;

namespace Platform.GameCore
{
	// Token: 0x02001CAA RID: 7338
	public class Api : XblPlatformApi
	{
		// Token: 0x17001B76 RID: 7030
		// (get) Token: 0x0600D99E RID: 55710 RVA: 0x004E047E File Offset: 0x004DE67E
		public override string SCID
		{
			get
			{
				return this.scid;
			}
		}

		// Token: 0x0600D99F RID: 55711 RVA: 0x004E0486 File Offset: 0x004DE686
		public override void Init(IPlatform owner)
		{
			Log.Out("[XBL] API Startup. SCID: " + this.SCID);
		}

		// Token: 0x0600D9A0 RID: 55712 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool InitServerApis()
		{
			return false;
		}

		// Token: 0x0600D9A1 RID: 55713 RVA: 0x000027FC File Offset: 0x000009FC
		public override void ServerApiLoaded()
		{
		}

		// Token: 0x0400A51C RID: 42268
		[PublicizedFrom(EAccessModifier.Private)]
		public string scid;
	}
}
