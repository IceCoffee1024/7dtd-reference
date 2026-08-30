using System;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001C7B RID: 7291
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.Steam)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600D7E0 RID: 55264 RVA: 0x004DA8E4 File Offset: 0x004D8AE4
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierSteam(_userId);
		}
	}
}
