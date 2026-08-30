using System;
using UnityEngine.Scripting;

namespace Platform.XBL
{
	// Token: 0x02001BEA RID: 7146
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.XBL)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600D3FF RID: 54271 RVA: 0x004C8904 File Offset: 0x004C6B04
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierXbl(_userId);
		}
	}
}
