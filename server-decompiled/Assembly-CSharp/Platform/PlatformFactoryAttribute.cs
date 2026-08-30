using System;

namespace Platform
{
	// Token: 0x02001BA9 RID: 7081
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class PlatformFactoryAttribute : Attribute
	{
		// Token: 0x0600D25B RID: 53851 RVA: 0x004C2394 File Offset: 0x004C0594
		public PlatformFactoryAttribute(EPlatformIdentifier _targetPlatform)
		{
			this.TargetPlatform = _targetPlatform;
		}

		// Token: 0x0400A018 RID: 40984
		public readonly EPlatformIdentifier TargetPlatform;
	}
}
