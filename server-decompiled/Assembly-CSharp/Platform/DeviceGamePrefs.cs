using System;
using System.IO;

namespace Platform
{
	// Token: 0x02001BA8 RID: 7080
	public static class DeviceGamePrefs
	{
		// Token: 0x0600D3BA RID: 54202 RVA: 0x004CD924 File Offset: 0x004CBB24
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

		// Token: 0x0600D3BB RID: 54203 RVA: 0x004CD9B8 File Offset: 0x004CBBB8
		public static string ConfigFilename(string _deviceName)
		{
			return "gameprefs_" + _deviceName;
		}

		// Token: 0x0600D3BC RID: 54204 RVA: 0x004CD9C8 File Offset: 0x004CBBC8
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

		// Token: 0x0600D3BD RID: 54205 RVA: 0x004CDA30 File Offset: 0x004CBC30
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
