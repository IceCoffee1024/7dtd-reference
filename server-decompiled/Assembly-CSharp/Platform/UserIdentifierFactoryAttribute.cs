using System;

namespace Platform
{
	// Token: 0x02001BCD RID: 7117
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class UserIdentifierFactoryAttribute : Attribute
	{
		// Token: 0x0600D328 RID: 54056 RVA: 0x004C5574 File Offset: 0x004C3774
		public UserIdentifierFactoryAttribute(EPlatformIdentifier _targetPlatform)
		{
			this.TargetPlatform = _targetPlatform;
		}

		// Token: 0x0400A09F RID: 41119
		public readonly EPlatformIdentifier TargetPlatform;
	}
}
