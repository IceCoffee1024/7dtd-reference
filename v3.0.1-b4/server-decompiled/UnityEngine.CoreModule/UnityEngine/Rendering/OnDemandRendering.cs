using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x0200042F RID: 1071
	[RequiredByNativeCode]
	public class OnDemandRendering
	{
		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x0600222E RID: 8750 RVA: 0x00038D00 File Offset: 0x00036F00
		public static bool willCurrentFrameRender
		{
			get
			{
				return Time.frameCount % OnDemandRendering.renderFrameInterval == 0;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x0600222F RID: 8751 RVA: 0x00038D20 File Offset: 0x00036F20
		// (set) Token: 0x06002230 RID: 8752 RVA: 0x00038D37 File Offset: 0x00036F37
		public static int renderFrameInterval
		{
			get
			{
				return OnDemandRendering.m_RenderFrameInterval;
			}
			set
			{
				OnDemandRendering.m_RenderFrameInterval = Math.Max(1, value);
			}
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x00038D46 File Offset: 0x00036F46
		[RequiredByNativeCode]
		internal static void GetRenderFrameInterval(out int frameInterval)
		{
			frameInterval = OnDemandRendering.renderFrameInterval;
		}

		// Token: 0x06002232 RID: 8754
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float GetEffectiveRenderFrameRate();

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002233 RID: 8755 RVA: 0x00038D50 File Offset: 0x00036F50
		public static int effectiveRenderFrameRate
		{
			get
			{
				float effectiveRenderFrameRate = OnDemandRendering.GetEffectiveRenderFrameRate();
				bool flag = (double)effectiveRenderFrameRate <= 0.0;
				int num;
				if (flag)
				{
					num = (int)effectiveRenderFrameRate;
				}
				else
				{
					num = (int)(effectiveRenderFrameRate + 0.5f);
				}
				return num;
			}
		}

		// Token: 0x04000D20 RID: 3360
		private static int m_RenderFrameInterval = 1;
	}
}
