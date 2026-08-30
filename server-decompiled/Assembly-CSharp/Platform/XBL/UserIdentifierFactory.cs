using System;
using UnityEngine.Scripting;

namespace Platform.XBL
{
	// Token: 0x02001C11 RID: 7185
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.XBL)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600D58D RID: 54669 RVA: 0x004CECF8 File Offset: 0x004CCEF8
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierXbl(_userId);
		}
	}
}
