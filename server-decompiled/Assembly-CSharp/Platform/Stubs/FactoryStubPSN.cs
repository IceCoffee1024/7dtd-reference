using System;
using UnityEngine.Scripting;

namespace Platform.Stubs
{
	// Token: 0x02001C7A RID: 7290
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.PSN)]
	public class FactoryStubPSN : AbsPlatform
	{
		// Token: 0x0600D82D RID: 55341 RVA: 0x004DB964 File Offset: 0x004D9B64
		public override void CreateInstances()
		{
			if (!base.AsServerOnly)
			{
				throw new NotSupportedException("This platform can only be used as a server platform.");
			}
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			if (crossplatformPlatform == null || crossplatformPlatform.PlatformIdentifier != EPlatformIdentifier.EOS)
			{
				throw new NotSupportedException("This server platform requires EOS as the cross-platform.");
			}
		}
	}
}
