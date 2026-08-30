using System;

namespace Platform
{
	// Token: 0x02001BF4 RID: 7156
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class UserIdentifierFactoryAttribute : Attribute
	{
		// Token: 0x0600D4B6 RID: 54454 RVA: 0x004CB968 File Offset: 0x004C9B68
		public UserIdentifierFactoryAttribute(EPlatformIdentifier _targetPlatform)
		{
			this.TargetPlatform = _targetPlatform;
		}

		// Token: 0x0400A1D0 RID: 41424
		public readonly EPlatformIdentifier TargetPlatform;
	}
}
