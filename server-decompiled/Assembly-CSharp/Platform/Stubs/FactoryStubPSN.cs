using System;
using UnityEngine.Scripting;

namespace Platform.Stubs
{
	// Token: 0x02001CDD RID: 7389
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.PSN)]
	public class FactoryStubPSN : AbsPlatform
	{
		// Token: 0x0600DABA RID: 55994 RVA: 0x004E3FF0 File Offset: 0x004E21F0
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
