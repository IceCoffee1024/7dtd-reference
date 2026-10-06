using System;

namespace Platform
{
	// Token: 0x02001C26 RID: 7206
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class UserIdentifierFactoryAttribute : Attribute
	{
		// Token: 0x0600D62E RID: 54830 RVA: 0x004D0450 File Offset: 0x004CE650
		public UserIdentifierFactoryAttribute(EPlatformIdentifier _targetPlatform)
		{
			this.TargetPlatform = _targetPlatform;
		}

		// Token: 0x0400A2A5 RID: 41637
		public readonly EPlatformIdentifier TargetPlatform;
	}
}
