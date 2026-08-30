using System;

namespace Platform
{
	// Token: 0x02001B51 RID: 6993
	public abstract class AbsUserIdentifierFactory
	{
		// Token: 0x0600D1A0 RID: 53664
		public abstract PlatformUserIdentifierAbs FromId(string _userId);

		// Token: 0x0600D1A1 RID: 53665 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsUserIdentifierFactory()
		{
		}
	}
}
