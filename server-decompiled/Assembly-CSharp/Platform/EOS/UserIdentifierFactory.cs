using System;
using UnityEngine.Scripting;

namespace Platform.EOS
{
	// Token: 0x02001D8E RID: 7566
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.EOS)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600DFD0 RID: 57296 RVA: 0x004FA8A8 File Offset: 0x004F8AA8
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierEos(_userId);
		}
	}
}
