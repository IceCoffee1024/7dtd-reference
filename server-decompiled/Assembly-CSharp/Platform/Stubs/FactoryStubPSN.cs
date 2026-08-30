using System;
using UnityEngine.Scripting;

namespace Platform.Stubs
{
	// Token: 0x02001C53 RID: 7251
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.PSN)]
	public class FactoryStubPSN : AbsPlatform
	{
		// Token: 0x0600D69D RID: 54941 RVA: 0x004D5540 File Offset: 0x004D3740
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
