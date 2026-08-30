using System;
using UnityEngine.Scripting;

namespace Platform.EOS
{
	// Token: 0x02001D03 RID: 7427
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.EOS)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600DBA0 RID: 56224 RVA: 0x004EB64C File Offset: 0x004E984C
		public override PlatformUserIdentifierAbs FromId(string _userId)
		{
			return new UserIdentifierEos(_userId);
		}
	}
}
