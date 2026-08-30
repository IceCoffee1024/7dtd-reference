using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000154 RID: 340
	[NativeHeader("Runtime/GfxDevice/HDROutputSettings.h")]
	[UsedByNativeCode]
	public class HDROutputSettings
	{
		// Token: 0x06000AAF RID: 2735 RVA: 0x00011578 File Offset: 0x0000F778
		[VisibleToOtherModules(new string[] { "UnityEngine.XRModule" })]
		internal HDROutputSettings()
		{
			this.m_DisplayIndex = 0;
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00011589 File Offset: 0x0000F789
		[VisibleToOtherModules(new string[] { "UnityEngine.XRModule" })]
		internal HDROutputSettings(int displayIndex)
		{
			this.m_DisplayIndex = displayIndex;
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0001159C File Offset: 0x0000F79C
		public static HDROutputSettings main
		{
			get
			{
				return HDROutputSettings._mainDisplay;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x000115B4 File Offset: 0x0000F7B4
		public bool active
		{
			get
			{
				return HDROutputSettings.GetActive(this.m_DisplayIndex);
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x000115D4 File Offset: 0x0000F7D4
		public bool available
		{
			get
			{
				return HDROutputSettings.GetAvailable(this.m_DisplayIndex);
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x000115F4 File Offset: 0x0000F7F4
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x00011611 File Offset: 0x0000F811
		public bool automaticHDRTonemapping
		{
			get
			{
				return HDROutputSettings.GetAutomaticHDRTonemapping(this.m_DisplayIndex);
			}
			set
			{
				HDROutputSettings.SetAutomaticHDRTonemapping(this.m_DisplayIndex, value);
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00011624 File Offset: 0x0000F824
		public ColorGamut displayColorGamut
		{
			get
			{
				return HDROutputSettings.GetDisplayColorGamut(this.m_DisplayIndex);
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x00011644 File Offset: 0x0000F844
		public RenderTextureFormat format
		{
			get
			{
				return GraphicsFormatUtility.GetRenderTextureFormat(HDROutputSettings.GetGraphicsFormat(this.m_DisplayIndex));
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00011668 File Offset: 0x0000F868
		public GraphicsFormat graphicsFormat
		{
			get
			{
				return HDROutputSettings.GetGraphicsFormat(this.m_DisplayIndex);
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00011688 File Offset: 0x0000F888
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x000116A5 File Offset: 0x0000F8A5
		public float paperWhiteNits
		{
			get
			{
				return HDROutputSettings.GetPaperWhiteNits(this.m_DisplayIndex);
			}
			set
			{
				HDROutputSettings.SetPaperWhiteNits(this.m_DisplayIndex, value);
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x000116B8 File Offset: 0x0000F8B8
		public int maxFullFrameToneMapLuminance
		{
			get
			{
				return HDROutputSettings.GetMaxFullFrameToneMapLuminance(this.m_DisplayIndex);
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x000116D8 File Offset: 0x0000F8D8
		public int maxToneMapLuminance
		{
			get
			{
				return HDROutputSettings.GetMaxToneMapLuminance(this.m_DisplayIndex);
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x000116F8 File Offset: 0x0000F8F8
		public int minToneMapLuminance
		{
			get
			{
				return HDROutputSettings.GetMinToneMapLuminance(this.m_DisplayIndex);
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x00011718 File Offset: 0x0000F918
		public bool HDRModeChangeRequested
		{
			get
			{
				return HDROutputSettings.GetHDRModeChangeRequested(this.m_DisplayIndex);
			}
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00011735 File Offset: 0x0000F935
		public void RequestHDRModeChange(bool enabled)
		{
			HDROutputSettings.RequestHDRModeChangeInternal(this.m_DisplayIndex, enabled);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00011748 File Offset: 0x0000F948
		[Obsolete("SetPaperWhiteInNits is deprecated, please use paperWhiteNits instead.")]
		public static void SetPaperWhiteInNits(float paperWhite)
		{
			int num = 0;
			bool available = HDROutputSettings.GetAvailable(num);
			if (available)
			{
				HDROutputSettings.SetPaperWhiteNits(num, paperWhite);
			}
		}

		// Token: 0x06000AC1 RID: 2753
		[FreeFunction("HDROutputSettingsBindings::GetActive", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetActive(int displayIndex);

		// Token: 0x06000AC2 RID: 2754
		[FreeFunction("HDROutputSettingsBindings::GetAvailable", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAvailable(int displayIndex);

		// Token: 0x06000AC3 RID: 2755
		[FreeFunction("HDROutputSettingsBindings::GetAutomaticHDRTonemapping", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAutomaticHDRTonemapping(int displayIndex);

		// Token: 0x06000AC4 RID: 2756
		[FreeFunction("HDROutputSettingsBindings::SetAutomaticHDRTonemapping", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAutomaticHDRTonemapping(int displayIndex, bool scripted);

		// Token: 0x06000AC5 RID: 2757
		[FreeFunction("HDROutputSettingsBindings::GetDisplayColorGamut", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ColorGamut GetDisplayColorGamut(int displayIndex);

		// Token: 0x06000AC6 RID: 2758
		[FreeFunction("HDROutputSettingsBindings::GetGraphicsFormat", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern GraphicsFormat GetGraphicsFormat(int displayIndex);

		// Token: 0x06000AC7 RID: 2759
		[FreeFunction("HDROutputSettingsBindings::GetPaperWhiteNits", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetPaperWhiteNits(int displayIndex);

		// Token: 0x06000AC8 RID: 2760
		[FreeFunction("HDROutputSettingsBindings::SetPaperWhiteNits", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPaperWhiteNits(int displayIndex, float paperWhite);

		// Token: 0x06000AC9 RID: 2761
		[FreeFunction("HDROutputSettingsBindings::GetMaxFullFrameToneMapLuminance", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMaxFullFrameToneMapLuminance(int displayIndex);

		// Token: 0x06000ACA RID: 2762
		[FreeFunction("HDROutputSettingsBindings::GetMaxToneMapLuminance", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMaxToneMapLuminance(int displayIndex);

		// Token: 0x06000ACB RID: 2763
		[FreeFunction("HDROutputSettingsBindings::GetMinToneMapLuminance", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMinToneMapLuminance(int displayIndex);

		// Token: 0x06000ACC RID: 2764
		[FreeFunction("HDROutputSettingsBindings::GetHDRModeChangeRequested", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetHDRModeChangeRequested(int displayIndex);

		// Token: 0x06000ACD RID: 2765
		[FreeFunction("HDROutputSettingsBindings::RequestHDRModeChange", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestHDRModeChangeInternal(int displayIndex, bool enabled);

		// Token: 0x04000444 RID: 1092
		private int m_DisplayIndex;

		// Token: 0x04000445 RID: 1093
		public static HDROutputSettings[] displays = new HDROutputSettings[]
		{
			new HDROutputSettings()
		};

		// Token: 0x04000446 RID: 1094
		private static HDROutputSettings _mainDisplay = HDROutputSettings.displays[0];
	}
}
