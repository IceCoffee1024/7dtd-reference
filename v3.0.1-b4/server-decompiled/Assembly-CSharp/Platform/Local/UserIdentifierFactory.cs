using System;
using UnityEngine.Scripting;

namespace Platform.Local
{
	// Token: 0x02001CA4 RID: 7332
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.Local)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600D94C RID: 55628 RVA: 0x004DD7F2 File Offset: 0x004DB9F2
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierLocal(_userId);
		}
	}
}
