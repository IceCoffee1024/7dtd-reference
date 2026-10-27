using System;
using UnityEngine.Scripting;

namespace Platform.XBL
{
	// Token: 0x02001C74 RID: 7284
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.XBL)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600D819 RID: 55321 RVA: 0x004D7308 File Offset: 0x004D5508
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierXbl(_userId);
		}
	}
}
