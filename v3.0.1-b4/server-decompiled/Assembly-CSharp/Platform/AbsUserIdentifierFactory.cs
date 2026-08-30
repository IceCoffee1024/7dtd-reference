using System;

namespace Platform
{
	// Token: 0x02001B2A RID: 6954
	public abstract class AbsUserIdentifierFactory
	{
		// Token: 0x0600D017 RID: 53271
		public abstract PlatformUserIdentifierAbs FromId(string _userId);

		// Token: 0x0600D018 RID: 53272 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsUserIdentifierFactory()
		{
		}
	}
}
