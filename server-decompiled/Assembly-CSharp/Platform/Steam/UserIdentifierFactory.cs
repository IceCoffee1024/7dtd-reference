using System;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001CA2 RID: 7330
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.Steam)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600D972 RID: 55666 RVA: 0x004E0E00 File Offset: 0x004DF000
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierSteam(_userId);
		}
	}
}
