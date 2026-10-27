using System;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001D05 RID: 7429
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.Steam)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600DC00 RID: 56320 RVA: 0x004E94E8 File Offset: 0x004E76E8
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierSteam(_userId);
		}
	}
}
