using System;
using Platform.XBL;

namespace Platform.GameCore
{
	// Token: 0x02001CDB RID: 7387
	public class Api : XblPlatformApi
	{
		// Token: 0x17001BBE RID: 7102
		// (get) Token: 0x0600DAB2 RID: 55986 RVA: 0x004E3FCE File Offset: 0x004E21CE
		public override string SCID
		{
			get
			{
				return this.scid;
			}
		}

		// Token: 0x0600DAB3 RID: 55987 RVA: 0x004E3FD6 File Offset: 0x004E21D6
		public override void Init(IPlatform owner)
		{
			Log.Out("[XBL] API Startup. SCID: " + this.SCID);
		}

		// Token: 0x0600DAB4 RID: 55988 RVA: 0x00010E62 File Offset: 0x0000F062
		public override bool InitServerApis()
		{
			return false;
		}

		// Token: 0x0600DAB5 RID: 55989 RVA: 0x000027FC File Offset: 0x000009FC
		public override void ServerApiLoaded()
		{
		}

		// Token: 0x0400A605 RID: 42501
		[PublicizedFrom(EAccessModifier.Private)]
		public string scid;
	}
}
