using System;
using UnityEngine.Scripting;

namespace Platform.Local
{
	// Token: 0x02001CFD RID: 7421
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.Local)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600DC62 RID: 56418 RVA: 0x004E894E File Offset: 0x004E6B4E
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierLocal(_userId);
		}
	}
}
