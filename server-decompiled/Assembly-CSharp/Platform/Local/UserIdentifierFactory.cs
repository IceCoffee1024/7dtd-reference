using System;
using UnityEngine.Scripting;

namespace Platform.Local
{
	// Token: 0x02001CCB RID: 7371
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.Local)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600DAE7 RID: 56039 RVA: 0x004E3E06 File Offset: 0x004E2006
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierLocal(_userId);
		}
	}
}
