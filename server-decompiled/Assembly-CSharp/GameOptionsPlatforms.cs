using System;
using Platform;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x020011F0 RID: 4592
public static class GameOptionsPlatforms
{
	// Token: 0x060092DE RID: 37598 RVA: 0x00376F4D File Offset: 0x0037514D
	public static float GetStreamingMipmapBudget()
	{
		return (float)SystemInfo.graphicsMemorySize * 0.9f;
	}

	// Token: 0x060092DF RID: 37599 RVA: 0x00376F5C File Offset: 0x0037515C
	public static string GetItemIconFilterString()
	{
		string text = "mip0";
		if ((float)SystemInfo.graphicsMemorySize <= 3200f || SystemInfo.systemMemorySize < 6800)
		{
			text = "mip1";
		}
		return text;
	}

	// Token: 0x060092E0 RID: 37600 RVA: 0x00376F90 File Offset: 0x00375190
	public static int CalcTextureQualityMin()
	{
		int systemMemorySize = SystemInfo.systemMemorySize;
		if (SystemInfo.graphicsMemorySize < 2400 || systemMemorySize < 4900)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x060092E1 RID: 37601 RVA: 0x00376FBC File Offset: 0x003751BC
	public static GameOptionsPlatforms.GfxPreset CalcDefaultGfxPreset()
	{
		if ((DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent())
		{
			GameOptionsPlatforms.GfxPreset gfxPreset = GameOptionsPlatforms.GfxPreset.Medium;
			float num = (float)SystemInfo.systemMemorySize;
			float num2 = (float)SystemInfo.graphicsMemorySize;
			if (!SystemInfo.operatingSystem.Contains(" Steam ") && (num2 < 2400f || num < 4800f))
			{
				gfxPreset = GameOptionsPlatforms.GfxPreset.Low;
			}
			if (num2 > 7500f && num > 5200f)
			{
				string text = SystemInfo.graphicsDeviceVendor.ToLower();
				if (text.Contains("nvidia"))
				{
					if (!GameOptionsPlatforms.FindGfxName(" 1070, 305"))
					{
						gfxPreset = GameOptionsPlatforms.GfxPreset.High;
						if (GameOptionsPlatforms.FindGfxName(" 208, 307, 308, 309, 407, 408, 409, 507, 508, 509"))
						{
							gfxPreset = GameOptionsPlatforms.GfxPreset.Ultra;
						}
					}
				}
				else if ((text == "amd" || text == "ati") && !GameOptionsPlatforms.FindGfxName("RX 570,RX 580,RX 590,RX 5500,RX 65,RX 66"))
				{
					gfxPreset = GameOptionsPlatforms.GfxPreset.High;
					if (GameOptionsPlatforms.FindGfxName(" 680, 690, 695, 770, 780, 790, 907, 908, 909"))
					{
						gfxPreset = GameOptionsPlatforms.GfxPreset.Ultra;
					}
				}
			}
			return gfxPreset;
		}
		if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent())
		{
			return GameOptionsPlatforms.GfxPreset.ConsolePerformance;
		}
		return GameOptionsPlatforms.GfxPreset.Medium;
	}

	// Token: 0x170011B7 RID: 4535
	// (get) Token: 0x060092E2 RID: 37602 RVA: 0x00377091 File Offset: 0x00375291
	public static int DefaultUpscalerMode
	{
		get
		{
			if (!FSR3.FSR3Supported())
			{
				return 4;
			}
			return 2;
		}
	}

	// Token: 0x060092E3 RID: 37603 RVA: 0x003770A0 File Offset: 0x003752A0
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool FindGfxName(string names)
	{
		string text = SystemInfo.graphicsDeviceName.ToLower();
		if (text.Contains("laptop"))
		{
			return false;
		}
		string[] array = names.Split(',', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			if (text.Contains(array[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060092E4 RID: 37604 RVA: 0x003770ED File Offset: 0x003752ED
	public static int ApplyTextureFilterLimit(int filter)
	{
		if ((float)SystemInfo.graphicsMemorySize < 3200f)
		{
			filter = 0;
		}
		return filter;
	}

	// Token: 0x020011F1 RID: 4593
	public enum GfxPreset
	{
		// Token: 0x04006C75 RID: 27765
		Lowest,
		// Token: 0x04006C76 RID: 27766
		Low,
		// Token: 0x04006C77 RID: 27767
		Medium,
		// Token: 0x04006C78 RID: 27768
		High,
		// Token: 0x04006C79 RID: 27769
		Ultra,
		// Token: 0x04006C7A RID: 27770
		Custom,
		// Token: 0x04006C7B RID: 27771
		ConsolePerformance,
		// Token: 0x04006C7C RID: 27772
		LEGACY_ConsolePerformanceFSR,
		// Token: 0x04006C7D RID: 27773
		ConsoleQuality,
		// Token: 0x04006C7E RID: 27774
		LEGACY_ConsoleQualityFSR,
		// Token: 0x04006C7F RID: 27775
		Simplified = 100
	}

	// Token: 0x020011F2 RID: 4594
	public static class UpscalerMode
	{
		// Token: 0x060092E5 RID: 37605 RVA: 0x00377100 File Offset: 0x00375300
		public static string ToString(int upscalerSettingValue)
		{
			switch (upscalerSettingValue)
			{
			case 0:
				return "Off";
			case 1:
				return "FSR2";
			case 2:
				return "FSR3";
			case 3:
				return "Dynamic Resolution";
			case 4:
				return "Scale";
			case 5:
				return "DLSS";
			default:
				return "Unknown";
			}
		}

		// Token: 0x04006C80 RID: 27776
		public const int Off = 0;

		// Token: 0x04006C81 RID: 27777
		public const int FSR2 = 1;

		// Token: 0x04006C82 RID: 27778
		public const int FSR3 = 2;

		// Token: 0x04006C83 RID: 27779
		public const int DynamicResolution = 3;

		// Token: 0x04006C84 RID: 27780
		public const int Scale = 4;

		// Token: 0x04006C85 RID: 27781
		public const int DLSS = 5;
	}
}
