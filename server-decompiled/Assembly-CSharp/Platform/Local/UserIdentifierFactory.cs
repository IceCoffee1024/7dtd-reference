using System;
using UnityEngine.Scripting;

namespace Platform.Local
{
	// Token: 0x02001D2E RID: 7470
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.Local)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600DD76 RID: 56694 RVA: 0x004EC4EE File Offset: 0x004EA6EE
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierLocal(_userId);
		}
	}
}
