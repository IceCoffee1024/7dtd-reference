using System;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001CD4 RID: 7380
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.Steam)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600DAEC RID: 56044 RVA: 0x004E5948 File Offset: 0x004E3B48
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierSteam(_userId);
		}
	}
}
