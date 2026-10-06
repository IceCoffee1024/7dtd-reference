using System;
using UnityEngine.Scripting;

namespace Platform.XBL
{
	// Token: 0x02001C43 RID: 7235
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.XBL)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600D705 RID: 55045 RVA: 0x004D37E0 File Offset: 0x004D19E0
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierXbl(_userId);
		}
	}
}
