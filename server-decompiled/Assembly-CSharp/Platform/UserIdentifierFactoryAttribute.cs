using System;

namespace Platform
{
	// Token: 0x02001C57 RID: 7255
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class UserIdentifierFactoryAttribute : Attribute
	{
		// Token: 0x0600D742 RID: 55106 RVA: 0x004D3F78 File Offset: 0x004D2178
		public UserIdentifierFactoryAttribute(EPlatformIdentifier _targetPlatform)
		{
			this.TargetPlatform = _targetPlatform;
		}

		// Token: 0x0400A38E RID: 41870
		public readonly EPlatformIdentifier TargetPlatform;
	}
}
