using System;

namespace Platform
{
	// Token: 0x02001C33 RID: 7219
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class PlatformFactoryAttribute : Attribute
	{
		// Token: 0x0600D675 RID: 54901 RVA: 0x004D0D98 File Offset: 0x004CEF98
		public PlatformFactoryAttribute(EPlatformIdentifier _targetPlatform)
		{
			this.TargetPlatform = _targetPlatform;
		}

		// Token: 0x0400A307 RID: 41735
		public readonly EPlatformIdentifier TargetPlatform;
	}
}
