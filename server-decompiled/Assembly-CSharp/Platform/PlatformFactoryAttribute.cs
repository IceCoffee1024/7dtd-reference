using System;

namespace Platform
{
	// Token: 0x02001BD0 RID: 7120
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class PlatformFactoryAttribute : Attribute
	{
		// Token: 0x0600D3E9 RID: 54249 RVA: 0x004C8788 File Offset: 0x004C6988
		public PlatformFactoryAttribute(EPlatformIdentifier _targetPlatform)
		{
			this.TargetPlatform = _targetPlatform;
		}

		// Token: 0x0400A149 RID: 41289
		public readonly EPlatformIdentifier TargetPlatform;
	}
}
