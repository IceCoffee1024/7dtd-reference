using System;
using Platform;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02001200 RID: 4608
public static class GameOptionsPlatforms
{
	// Token: 0x06009334 RID: 37684 RVA: 0x003790C1 File Offset: 0x003772C1
	public static float GetStreamingMipmapBudget()
	{
		return (float)SystemInfo.graphicsMemorySize * 0.9f;
	}

	// Token: 0x06009335 RID: 37685 RVA: 0x003790D0 File Offset: 0x003772D0
	public static string GetItemIconFilterString()
	{
		string text = "mip0";
		if ((float)SystemInfo.graphicsMemorySize <= 3200f || SystemInfo.systemMemorySize < 6800)
		{
			text = "mip1";
		}
		return text;
	}

	// Token: 0x06009336 RID: 37686 RVA: 0x00379104 File Offset: 0x00377304
	public static int CalcTextureQualityMin()
	{
		int systemMemorySize = SystemInfo.systemMemorySize;
		if (SystemInfo.graphicsMemorySize < 2400 || systemMemorySize < 4900)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06009337 RID: 37687 RVA: 0x00379130 File Offset: 0x00377330
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

	// Token: 0x170011C2 RID: 4546
	// (get) Token: 0x06009338 RID: 37688 RVA: 0x00379205 File Offset: 0x00377405
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

	// Token: 0x06009339 RID: 37689 RVA: 0x00379214 File Offset: 0x00377414
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

	// Token: 0x0600933A RID: 37690 RVA: 0x00379261 File Offset: 0x00377461
	public static int ApplyTextureFilterLimit(int filter)
	{
		if ((float)SystemInfo.graphicsMemorySize < 3200f)
		{
			filter = 0;
		}
		return filter;
	}

	// Token: 0x02001201 RID: 4609
	public enum GfxPreset
	{
		// Token: 0x04006C8D RID: 27789
		Lowest,
		// Token: 0x04006C8E RID: 27790
		Low,
		// Token: 0x04006C8F RID: 27791
		Medium,
		// Token: 0x04006C90 RID: 27792
		High,
		// Token: 0x04006C91 RID: 27793
		Ultra,
		// Token: 0x04006C92 RID: 27794
		Custom,
		// Token: 0x04006C93 RID: 27795
		ConsolePerformance,
		// Token: 0x04006C94 RID: 27796
		LEGACY_ConsolePerformanceFSR,
		// Token: 0x04006C95 RID: 27797
		ConsoleQuality,
		// Token: 0x04006C96 RID: 27798
		LEGACY_ConsoleQualityFSR,
		// Token: 0x04006C97 RID: 27799
		Simplified = 100
	}

	// Token: 0x02001202 RID: 4610
	public static class UpscalerMode
	{
		// Token: 0x0600933B RID: 37691 RVA: 0x00379274 File Offset: 0x00377474
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

		// Token: 0x04006C98 RID: 27800
		public const int Off = 0;

		// Token: 0x04006C99 RID: 27801
		public const int FSR2 = 1;

		// Token: 0x04006C9A RID: 27802
		public const int FSR3 = 2;

		// Token: 0x04006C9B RID: 27803
		public const int DynamicResolution = 3;

		// Token: 0x04006C9C RID: 27804
		public const int Scale = 4;

		// Token: 0x04006C9D RID: 27805
		public const int DLSS = 5;
	}
}
