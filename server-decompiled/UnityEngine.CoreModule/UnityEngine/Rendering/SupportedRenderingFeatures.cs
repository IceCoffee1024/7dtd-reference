using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x0200047C RID: 1148
	public class SupportedRenderingFeatures
	{
		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x0600271A RID: 10010 RVA: 0x000433C4 File Offset: 0x000415C4
		// (set) Token: 0x0600271B RID: 10011 RVA: 0x000433F1 File Offset: 0x000415F1
		public static SupportedRenderingFeatures active
		{
			get
			{
				bool flag = SupportedRenderingFeatures.s_Active == null;
				if (flag)
				{
					SupportedRenderingFeatures.s_Active = new SupportedRenderingFeatures();
				}
				return SupportedRenderingFeatures.s_Active;
			}
			set
			{
				SupportedRenderingFeatures.s_Active = value;
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x0600271C RID: 10012 RVA: 0x000433FA File Offset: 0x000415FA
		// (set) Token: 0x0600271D RID: 10013 RVA: 0x00043402 File Offset: 0x00041602
		public SupportedRenderingFeatures.ReflectionProbeModes reflectionProbeModes { get; set; } = SupportedRenderingFeatures.ReflectionProbeModes.None;

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x0600271E RID: 10014 RVA: 0x0004340B File Offset: 0x0004160B
		// (set) Token: 0x0600271F RID: 10015 RVA: 0x00043413 File Offset: 0x00041613
		public SupportedRenderingFeatures.LightmapMixedBakeModes defaultMixedLightingModes { get; set; } = SupportedRenderingFeatures.LightmapMixedBakeModes.None;

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06002720 RID: 10016 RVA: 0x0004341C File Offset: 0x0004161C
		// (set) Token: 0x06002721 RID: 10017 RVA: 0x00043424 File Offset: 0x00041624
		public SupportedRenderingFeatures.LightmapMixedBakeModes mixedLightingModes { get; set; } = SupportedRenderingFeatures.LightmapMixedBakeModes.IndirectOnly | SupportedRenderingFeatures.LightmapMixedBakeModes.Subtractive | SupportedRenderingFeatures.LightmapMixedBakeModes.Shadowmask;

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06002722 RID: 10018 RVA: 0x0004342D File Offset: 0x0004162D
		// (set) Token: 0x06002723 RID: 10019 RVA: 0x00043435 File Offset: 0x00041635
		public LightmapBakeType lightmapBakeTypes { get; set; } = LightmapBakeType.Realtime | LightmapBakeType.Baked | LightmapBakeType.Mixed;

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06002724 RID: 10020 RVA: 0x0004343E File Offset: 0x0004163E
		// (set) Token: 0x06002725 RID: 10021 RVA: 0x00043446 File Offset: 0x00041646
		public LightmapsMode lightmapsModes { get; set; } = LightmapsMode.CombinedDirectional;

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06002726 RID: 10022 RVA: 0x0004344F File Offset: 0x0004164F
		// (set) Token: 0x06002727 RID: 10023 RVA: 0x00043457 File Offset: 0x00041657
		[Obsolete("Bake with the Progressive Lightmapper. The backend that uses Enlighten to bake is deprecated.", false)]
		public bool enlightenLightmapper { get; set; } = true;

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06002728 RID: 10024 RVA: 0x00043460 File Offset: 0x00041660
		// (set) Token: 0x06002729 RID: 10025 RVA: 0x00043468 File Offset: 0x00041668
		public bool enlighten { get; set; } = true;

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x0600272A RID: 10026 RVA: 0x00043471 File Offset: 0x00041671
		// (set) Token: 0x0600272B RID: 10027 RVA: 0x00043479 File Offset: 0x00041679
		public bool lightProbeProxyVolumes { get; set; } = true;

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x0600272C RID: 10028 RVA: 0x00043482 File Offset: 0x00041682
		// (set) Token: 0x0600272D RID: 10029 RVA: 0x0004348A File Offset: 0x0004168A
		public bool motionVectors { get; set; } = true;

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x0600272E RID: 10030 RVA: 0x00043493 File Offset: 0x00041693
		// (set) Token: 0x0600272F RID: 10031 RVA: 0x0004349B File Offset: 0x0004169B
		public bool receiveShadows { get; set; } = true;

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06002730 RID: 10032 RVA: 0x000434A4 File Offset: 0x000416A4
		// (set) Token: 0x06002731 RID: 10033 RVA: 0x000434AC File Offset: 0x000416AC
		public bool reflectionProbes { get; set; } = true;

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06002732 RID: 10034 RVA: 0x000434B5 File Offset: 0x000416B5
		// (set) Token: 0x06002733 RID: 10035 RVA: 0x000434BD File Offset: 0x000416BD
		public bool reflectionProbesBlendDistance { get; set; } = true;

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06002734 RID: 10036 RVA: 0x000434C6 File Offset: 0x000416C6
		// (set) Token: 0x06002735 RID: 10037 RVA: 0x000434CE File Offset: 0x000416CE
		public bool rendererPriority { get; set; } = false;

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06002736 RID: 10038 RVA: 0x000434D7 File Offset: 0x000416D7
		// (set) Token: 0x06002737 RID: 10039 RVA: 0x000434DF File Offset: 0x000416DF
		public bool rendersUIOverlay { get; set; } = false;

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06002738 RID: 10040 RVA: 0x000434E8 File Offset: 0x000416E8
		// (set) Token: 0x06002739 RID: 10041 RVA: 0x000434F0 File Offset: 0x000416F0
		public bool overridesEnvironmentLighting { get; set; } = false;

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x0600273A RID: 10042 RVA: 0x000434F9 File Offset: 0x000416F9
		// (set) Token: 0x0600273B RID: 10043 RVA: 0x00043501 File Offset: 0x00041701
		public bool overridesFog { get; set; } = false;

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x0600273C RID: 10044 RVA: 0x0004350A File Offset: 0x0004170A
		// (set) Token: 0x0600273D RID: 10045 RVA: 0x00043512 File Offset: 0x00041712
		public bool overridesRealtimeReflectionProbes { get; set; } = false;

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x0600273E RID: 10046 RVA: 0x0004351B File Offset: 0x0004171B
		// (set) Token: 0x0600273F RID: 10047 RVA: 0x00043523 File Offset: 0x00041723
		public bool overridesOtherLightingSettings { get; set; } = false;

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06002740 RID: 10048 RVA: 0x0004352C File Offset: 0x0004172C
		// (set) Token: 0x06002741 RID: 10049 RVA: 0x00043534 File Offset: 0x00041734
		public bool editableMaterialRenderQueue { get; set; } = true;

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06002742 RID: 10050 RVA: 0x0004353D File Offset: 0x0004173D
		// (set) Token: 0x06002743 RID: 10051 RVA: 0x00043545 File Offset: 0x00041745
		public bool overridesLODBias { get; set; } = false;

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002744 RID: 10052 RVA: 0x0004354E File Offset: 0x0004174E
		// (set) Token: 0x06002745 RID: 10053 RVA: 0x00043556 File Offset: 0x00041756
		public bool overridesMaximumLODLevel { get; set; } = false;

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06002746 RID: 10054 RVA: 0x0004355F File Offset: 0x0004175F
		// (set) Token: 0x06002747 RID: 10055 RVA: 0x00043567 File Offset: 0x00041767
		public bool overridesEnableLODCrossFade { get; set; } = false;

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06002748 RID: 10056 RVA: 0x00043570 File Offset: 0x00041770
		// (set) Token: 0x06002749 RID: 10057 RVA: 0x00043578 File Offset: 0x00041778
		public bool rendererProbes { get; set; } = true;

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x0600274A RID: 10058 RVA: 0x00043581 File Offset: 0x00041781
		// (set) Token: 0x0600274B RID: 10059 RVA: 0x00043589 File Offset: 0x00041789
		public bool particleSystemInstancing { get; set; } = true;

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x0600274C RID: 10060 RVA: 0x00043592 File Offset: 0x00041792
		// (set) Token: 0x0600274D RID: 10061 RVA: 0x0004359A File Offset: 0x0004179A
		public bool autoAmbientProbeBaking { get; set; } = true;

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x0600274E RID: 10062 RVA: 0x000435A3 File Offset: 0x000417A3
		// (set) Token: 0x0600274F RID: 10063 RVA: 0x000435AB File Offset: 0x000417AB
		public bool autoDefaultReflectionProbeBaking { get; set; } = true;

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002750 RID: 10064 RVA: 0x000435B4 File Offset: 0x000417B4
		// (set) Token: 0x06002751 RID: 10065 RVA: 0x000435BC File Offset: 0x000417BC
		public bool overridesShadowmask { get; set; } = false;

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06002752 RID: 10066 RVA: 0x000435C5 File Offset: 0x000417C5
		// (set) Token: 0x06002753 RID: 10067 RVA: 0x000435CD File Offset: 0x000417CD
		public bool overridesLightProbeSystem { get; set; } = false;

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06002754 RID: 10068 RVA: 0x000435D6 File Offset: 0x000417D6
		// (set) Token: 0x06002755 RID: 10069 RVA: 0x000435DE File Offset: 0x000417DE
		public bool supportsHDR { get; set; } = false;

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06002756 RID: 10070 RVA: 0x000435E7 File Offset: 0x000417E7
		// (set) Token: 0x06002757 RID: 10071 RVA: 0x000435EF File Offset: 0x000417EF
		public string overridesLightProbeSystemWarningMessage { get; set; } = "The rendering pipeline used has an alternative method to handle light probes. Please consult the documentation for the used SRP to setup the alternative.";

		// Token: 0x06002758 RID: 10072 RVA: 0x000435F8 File Offset: 0x000417F8
		internal unsafe static MixedLightingMode FallbackMixedLightingMode()
		{
			MixedLightingMode mixedLightingMode;
			SupportedRenderingFeatures.FallbackMixedLightingModeByRef(new IntPtr((void*)(&mixedLightingMode)));
			return mixedLightingMode;
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x0004361C File Offset: 0x0004181C
		[RequiredByNativeCode]
		internal unsafe static void FallbackMixedLightingModeByRef(IntPtr fallbackModePtr)
		{
			MixedLightingMode* ptr = (MixedLightingMode*)(void*)fallbackModePtr;
			bool flag = SupportedRenderingFeatures.active.defaultMixedLightingModes != SupportedRenderingFeatures.LightmapMixedBakeModes.None && (SupportedRenderingFeatures.active.mixedLightingModes & SupportedRenderingFeatures.active.defaultMixedLightingModes) == SupportedRenderingFeatures.active.defaultMixedLightingModes;
			if (flag)
			{
				SupportedRenderingFeatures.LightmapMixedBakeModes defaultMixedLightingModes = SupportedRenderingFeatures.active.defaultMixedLightingModes;
				SupportedRenderingFeatures.LightmapMixedBakeModes lightmapMixedBakeModes = defaultMixedLightingModes;
				if (lightmapMixedBakeModes != SupportedRenderingFeatures.LightmapMixedBakeModes.Subtractive)
				{
					if (lightmapMixedBakeModes != SupportedRenderingFeatures.LightmapMixedBakeModes.Shadowmask)
					{
						*ptr = MixedLightingMode.IndirectOnly;
					}
					else
					{
						*ptr = MixedLightingMode.Shadowmask;
					}
				}
				else
				{
					*ptr = MixedLightingMode.Subtractive;
				}
			}
			else
			{
				bool flag2 = SupportedRenderingFeatures.IsMixedLightingModeSupported(MixedLightingMode.Shadowmask);
				if (flag2)
				{
					*ptr = MixedLightingMode.Shadowmask;
				}
				else
				{
					bool flag3 = SupportedRenderingFeatures.IsMixedLightingModeSupported(MixedLightingMode.Subtractive);
					if (flag3)
					{
						*ptr = MixedLightingMode.Subtractive;
					}
					else
					{
						*ptr = MixedLightingMode.IndirectOnly;
					}
				}
			}
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000436B8 File Offset: 0x000418B8
		internal unsafe static bool IsMixedLightingModeSupported(MixedLightingMode mixedMode)
		{
			bool flag;
			SupportedRenderingFeatures.IsMixedLightingModeSupportedByRef(mixedMode, new IntPtr((void*)(&flag)));
			return flag;
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x000436DC File Offset: 0x000418DC
		[RequiredByNativeCode]
		internal unsafe static void IsMixedLightingModeSupportedByRef(MixedLightingMode mixedMode, IntPtr isSupportedPtr)
		{
			bool* ptr = (bool*)(void*)isSupportedPtr;
			bool flag = !SupportedRenderingFeatures.IsLightmapBakeTypeSupported(LightmapBakeType.Mixed);
			if (flag)
			{
				*ptr = false;
			}
			else
			{
				*ptr = (mixedMode == MixedLightingMode.IndirectOnly && (SupportedRenderingFeatures.active.mixedLightingModes & SupportedRenderingFeatures.LightmapMixedBakeModes.IndirectOnly) == SupportedRenderingFeatures.LightmapMixedBakeModes.IndirectOnly) || (mixedMode == MixedLightingMode.Subtractive && (SupportedRenderingFeatures.active.mixedLightingModes & SupportedRenderingFeatures.LightmapMixedBakeModes.Subtractive) == SupportedRenderingFeatures.LightmapMixedBakeModes.Subtractive) || (mixedMode == MixedLightingMode.Shadowmask && (SupportedRenderingFeatures.active.mixedLightingModes & SupportedRenderingFeatures.LightmapMixedBakeModes.Shadowmask) == SupportedRenderingFeatures.LightmapMixedBakeModes.Shadowmask);
			}
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x00043744 File Offset: 0x00041944
		internal unsafe static bool IsLightmapBakeTypeSupported(LightmapBakeType bakeType)
		{
			bool flag;
			SupportedRenderingFeatures.IsLightmapBakeTypeSupportedByRef(bakeType, new IntPtr((void*)(&flag)));
			return flag;
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x00043768 File Offset: 0x00041968
		[RequiredByNativeCode]
		internal unsafe static void IsLightmapBakeTypeSupportedByRef(LightmapBakeType bakeType, IntPtr isSupportedPtr)
		{
			bool* ptr = (bool*)(void*)isSupportedPtr;
			bool flag = bakeType == LightmapBakeType.Mixed;
			if (flag)
			{
				bool flag2 = SupportedRenderingFeatures.IsLightmapBakeTypeSupported(LightmapBakeType.Baked);
				bool flag3 = !flag2 || SupportedRenderingFeatures.active.mixedLightingModes == SupportedRenderingFeatures.LightmapMixedBakeModes.None;
				if (flag3)
				{
					*ptr = false;
					return;
				}
			}
			*ptr = (SupportedRenderingFeatures.active.lightmapBakeTypes & bakeType) == bakeType;
			bool flag4 = bakeType == LightmapBakeType.Realtime && !SupportedRenderingFeatures.active.enlighten;
			if (flag4)
			{
				*ptr = false;
			}
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x000437DC File Offset: 0x000419DC
		internal unsafe static bool IsLightmapsModeSupported(LightmapsMode mode)
		{
			bool flag;
			SupportedRenderingFeatures.IsLightmapsModeSupportedByRef(mode, new IntPtr((void*)(&flag)));
			return flag;
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x00043800 File Offset: 0x00041A00
		[RequiredByNativeCode]
		internal unsafe static void IsLightmapsModeSupportedByRef(LightmapsMode mode, IntPtr isSupportedPtr)
		{
			bool* ptr = (bool*)(void*)isSupportedPtr;
			*ptr = (SupportedRenderingFeatures.active.lightmapsModes & mode) == mode;
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x00043828 File Offset: 0x00041A28
		internal unsafe static bool IsLightmapperSupported(int lightmapper)
		{
			bool flag;
			SupportedRenderingFeatures.IsLightmapperSupportedByRef(lightmapper, new IntPtr((void*)(&flag)));
			return flag;
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x0004384C File Offset: 0x00041A4C
		[RequiredByNativeCode]
		internal unsafe static void IsLightmapperSupportedByRef(int lightmapper, IntPtr isSupportedPtr)
		{
			bool* ptr = (bool*)(void*)isSupportedPtr;
			*ptr = lightmapper != 0 || SupportedRenderingFeatures.active.enlightenLightmapper;
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x00043874 File Offset: 0x00041A74
		[RequiredByNativeCode]
		internal unsafe static void IsUIOverlayRenderedBySRP(IntPtr isSupportedPtr)
		{
			bool* ptr = (bool*)(void*)isSupportedPtr;
			*ptr = SupportedRenderingFeatures.active.rendersUIOverlay;
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x00043898 File Offset: 0x00041A98
		[RequiredByNativeCode]
		internal unsafe static void IsAutoAmbientProbeBakingSupported(IntPtr isSupportedPtr)
		{
			bool* ptr = (bool*)(void*)isSupportedPtr;
			*ptr = SupportedRenderingFeatures.active.autoAmbientProbeBaking;
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x000438BC File Offset: 0x00041ABC
		[RequiredByNativeCode]
		internal unsafe static void IsAutoDefaultReflectionProbeBakingSupported(IntPtr isSupportedPtr)
		{
			bool* ptr = (bool*)(void*)isSupportedPtr;
			*ptr = SupportedRenderingFeatures.active.autoDefaultReflectionProbeBaking;
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x000438E0 File Offset: 0x00041AE0
		[RequiredByNativeCode]
		internal unsafe static void OverridesLightProbeSystem(IntPtr overridesPtr)
		{
			bool* ptr = (bool*)(void*)overridesPtr;
			*ptr = SupportedRenderingFeatures.active.overridesLightProbeSystem;
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x00043904 File Offset: 0x00041B04
		internal unsafe static int FallbackLightmapper()
		{
			int num;
			SupportedRenderingFeatures.FallbackLightmapperByRef(new IntPtr((void*)(&num)));
			return num;
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x00043928 File Offset: 0x00041B28
		[RequiredByNativeCode]
		internal unsafe static void FallbackLightmapperByRef(IntPtr lightmapperPtr)
		{
			int* ptr = (int*)(void*)lightmapperPtr;
			*ptr = 1;
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06002768 RID: 10088 RVA: 0x00043940 File Offset: 0x00041B40
		// (set) Token: 0x06002769 RID: 10089 RVA: 0x00002669 File Offset: 0x00000869
		[Obsolete("terrainDetailUnsupported is deprecated.")]
		public bool terrainDetailUnsupported
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x04000ECE RID: 3790
		private static SupportedRenderingFeatures s_Active = new SupportedRenderingFeatures();

		// Token: 0x0200047D RID: 1149
		[Flags]
		public enum ReflectionProbeModes
		{
			// Token: 0x04000EEE RID: 3822
			None = 0,
			// Token: 0x04000EEF RID: 3823
			Rotation = 1
		}

		// Token: 0x0200047E RID: 1150
		[Flags]
		public enum LightmapMixedBakeModes
		{
			// Token: 0x04000EF1 RID: 3825
			None = 0,
			// Token: 0x04000EF2 RID: 3826
			IndirectOnly = 1,
			// Token: 0x04000EF3 RID: 3827
			Subtractive = 2,
			// Token: 0x04000EF4 RID: 3828
			Shadowmask = 4
		}
	}
}
