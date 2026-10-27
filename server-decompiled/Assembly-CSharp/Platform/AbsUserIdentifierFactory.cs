using System;

namespace Platform
{
	// Token: 0x02001BB4 RID: 7092
	public abstract class AbsUserIdentifierFactory
	{
		// Token: 0x0600D42B RID: 54315
		public abstract PlatformUserIdentifierAbs FromId(string _userId);

		// Token: 0x0600D42C RID: 54316 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsUserIdentifierFactory()
		{
		}
	}
}
