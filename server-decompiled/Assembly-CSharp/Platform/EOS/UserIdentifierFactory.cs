using System;
using UnityEngine.Scripting;

namespace Platform.EOS
{
	// Token: 0x02001D5D RID: 7517
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.EOS)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600DEBC RID: 57020 RVA: 0x004F6D08 File Offset: 0x004F4F08
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierEos(_userId);
		}
	}
}
