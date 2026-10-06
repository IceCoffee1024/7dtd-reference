using System;

namespace Platform
{
	// Token: 0x02001C02 RID: 7170
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class PlatformFactoryAttribute : Attribute
	{
		// Token: 0x0600D561 RID: 54625 RVA: 0x004CD270 File Offset: 0x004CB470
		public PlatformFactoryAttribute(EPlatformIdentifier _targetPlatform)
		{
			this.TargetPlatform = _targetPlatform;
		}

		// Token: 0x0400A21E RID: 41502
		public readonly EPlatformIdentifier TargetPlatform;
	}
}
