using System;
using System.IO;

namespace Platform
{
	// Token: 0x02001B45 RID: 6981
	public static class DeviceGamePrefs
	{
		// Token: 0x0600D12F RID: 53551 RVA: 0x004C5314 File Offset: 0x004C3514
		public static void Apply()
		{
			if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent())
			{
				GameOptionsPlatforms.GfxPreset @int = (GameOptionsPlatforms.GfxPreset)GamePrefs.GetInt(EnumGamePrefs.OptionsGfxQualityPreset);
				if (@int != GameOptionsPlatforms.GfxPreset.ConsolePerformance && @int != GameOptionsPlatforms.GfxPreset.ConsoleQuality)
				{
					Log.Out(string.Format("[DeviceGamePrefs] Quality preset \"{0}\" is unsupported on this platform; defaulting to ConsolePerformance.", @int));
					GamePrefs.Set(EnumGamePrefs.OptionsGfxQualityPreset, 6);
				}
				int int2 = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxUpscalerMode);
				if (int2 != 2 && int2 != 4)
				{
					Log.Out(string.Format("[DeviceGamePrefs] Upscaler mode \"{0}\" is unsupported on this platform; defaulting to \"{1}\".", int2, GameOptionsPlatforms.DefaultUpscalerMode));
					GamePrefs.Set(EnumGamePrefs.OptionsGfxUpscalerMode, GameOptionsPlatforms.DefaultUpscalerMode);
				}
				GameOptionsManager.SetGraphicsQuality();
			}
			DeviceGamePrefs.ApplyConfigFilePrefs();
		}

		// Token: 0x0600D130 RID: 53552 RVA: 0x004C53A8 File Offset: 0x004C35A8
		public static string ConfigFilename(string _deviceName)
		{
			return "gameprefs_" + _deviceName;
		}

		// Token: 0x0600D131 RID: 53553 RVA: 0x004C53B8 File Offset: 0x004C35B8
		[PublicizedFrom(EAccessModifier.Private)]
		public static void ApplyConfigFilePrefs()
		{
			string text = DeviceGamePrefs.ConfigFilename(DeviceFlag.StandaloneWindows.GetDeviceName());
			string text2 = Path.Combine(GameIO.GetApplicationPath(), text + ".xml");
			if (File.Exists(text2))
			{
				Log.Out("[DeviceGamePrefs] Applying game prefs from {0}", new object[] { text2 });
				DynamicProperties dynamicProperties = new DynamicProperties();
				if (dynamicProperties.Load(GameIO.GetApplicationPath(), text))
				{
					DeviceGamePrefs.ApplyGamePrefs(dynamicProperties);
				}
			}
		}

		// Token: 0x0600D132 RID: 53554 RVA: 0x004C5420 File Offset: 0x004C3620
		[PublicizedFrom(EAccessModifier.Private)]
		public static void ApplyGamePrefs(DynamicProperties properties)
		{
			foreach (string text in properties.Values.Keys)
			{
				EnumGamePrefs enumGamePrefs;
				if (EnumUtils.TryParse<EnumGamePrefs>(text, out enumGamePrefs, true))
				{
					object obj = GamePrefs.Parse(enumGamePrefs, properties.Values[text]);
					if (obj != null)
					{
						GamePrefs.SetObject(enumGamePrefs, obj);
						Log.Out("[DeviceGamePrefs] {0}={1}", new object[]
						{
							text,
							GamePrefs.GetObject(enumGamePrefs)
						});
					}
					else
					{
						Log.Error("[DeviceGamePrefs] Invalid value for GamePref: {0}", new object[] { text });
					}
				}
				else
				{
					Log.Error("[DeviceGamePrefs] Unknown GamePref: {0}", new object[] { text });
				}
			}
		}
	}
}
