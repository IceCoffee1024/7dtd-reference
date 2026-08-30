using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003FF RID: 1023
	internal static class CameraEventUtils
	{
		// Token: 0x060021AD RID: 8621 RVA: 0x00037FC4 File Offset: 0x000361C4
		public static bool IsValid(CameraEvent value)
		{
			return value >= CameraEvent.BeforeDepthTexture && value <= CameraEvent.AfterHaloAndLensFlares;
		}

		// Token: 0x04000BDD RID: 3037
		private const CameraEvent k_MinimumValue = CameraEvent.BeforeDepthTexture;

		// Token: 0x04000BDE RID: 3038
		private const CameraEvent k_MaximumValue = CameraEvent.AfterHaloAndLensFlares;
	}
}
