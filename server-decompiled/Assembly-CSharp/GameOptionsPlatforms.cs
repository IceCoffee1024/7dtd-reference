using System;
using Platform;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x020011BC RID: 4540
public static class GameOptionsPlatforms
{
	// Token: 0x0600914C RID: 37196 RVA: 0x0036D459 File Offset: 0x0036B659
	public static float GetStreamingMipmapBudget()
	{
		return (float)SystemInfo.graphicsMemorySize * 0.9f;
	}

	// Token: 0x0600914D RID: 37197 RVA: 0x0036D468 File Offset: 0x0036B668
	public static string GetItemIconFilterString()
	{
		string text = "mip0";
		if ((float)SystemInfo.graphicsMemorySize <= 3200f || SystemInfo.systemMemorySize < 6800)
		{
			text = "mip1";
		}
		return text;
	}

	// Token: 0x0600914E RID: 37198 RVA: 0x0036D49C File Offset: 0x0036B69C
	public static int CalcTextureQualityMin()
	{
		int systemMemorySize = SystemInfo.systemMemorySize;
		if (SystemInfo.graphicsMemorySize < 2400 || systemMemorySize < 4900)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x0600914F RID: 37199 RVA: 0x0036D4C8 File Offset: 0x0036B6C8
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

	// Token: 0x17001138 RID: 4408
	// (get) Token: 0x06009150 RID: 37200 RVA: 0x0036D59D File Offset: 0x0036B79D
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

	// Token: 0x06009151 RID: 37201 RVA: 0x0036D5AC File Offset: 0x0036B7AC
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

	// Token: 0x06009152 RID: 37202 RVA: 0x0036D5F9 File Offset: 0x0036B7F9
	public static int ApplyTextureFilterLimit(int filter)
	{
		if ((float)SystemInfo.graphicsMemorySize < 3200f)
		{
			filter = 0;
		}
		return filter;
	}

	// Token: 0x020011BD RID: 4541
	public enum GfxPreset
	{
		// Token: 0x04006B33 RID: 27443
		Lowest,
		// Token: 0x04006B34 RID: 27444
		Low,
		// Token: 0x04006B35 RID: 27445
		Medium,
		// Token: 0x04006B36 RID: 27446
		High,
		// Token: 0x04006B37 RID: 27447
		Ultra,
		// Token: 0x04006B38 RID: 27448
		Custom,
		// Token: 0x04006B39 RID: 27449
		ConsolePerformance,
		// Token: 0x04006B3A RID: 27450
		LEGACY_ConsolePerformanceFSR,
		// Token: 0x04006B3B RID: 27451
		ConsoleQuality,
		// Token: 0x04006B3C RID: 27452
		LEGACY_ConsoleQualityFSR,
		// Token: 0x04006B3D RID: 27453
		Simplified = 100
	}

	// Token: 0x020011BE RID: 4542
	public static class UpscalerMode
	{
		// Token: 0x06009153 RID: 37203 RVA: 0x0036D60C File Offset: 0x0036B80C
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

		// Token: 0x04006B3E RID: 27454
		public const int Off = 0;

		// Token: 0x04006B3F RID: 27455
		public const int FSR2 = 1;

		// Token: 0x04006B40 RID: 27456
		public const int FSR3 = 2;

		// Token: 0x04006B41 RID: 27457
		public const int DynamicResolution = 3;

		// Token: 0x04006B42 RID: 27458
		public const int Scale = 4;

		// Token: 0x04006B43 RID: 27459
		public const int DLSS = 5;
	}
}
