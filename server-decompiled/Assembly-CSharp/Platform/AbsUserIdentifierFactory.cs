using System;

namespace Platform
{
	// Token: 0x02001B83 RID: 7043
	public abstract class AbsUserIdentifierFactory
	{
		// Token: 0x0600D317 RID: 54039
		public abstract PlatformUserIdentifierAbs FromId(string _userId);

		// Token: 0x0600D318 RID: 54040 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsUserIdentifierFactory()
		{
		}
	}
}
