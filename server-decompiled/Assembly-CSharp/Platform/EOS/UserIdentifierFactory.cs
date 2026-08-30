using System;
using UnityEngine.Scripting;

namespace Platform.EOS
{
	// Token: 0x02001D2B RID: 7467
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.EOS)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600DD41 RID: 56641 RVA: 0x004F20C0 File Offset: 0x004F02C0
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierEos(_userId);
		}
	}
}
