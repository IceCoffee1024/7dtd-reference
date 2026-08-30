using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Platform
{
	// Token: 0x02001BA8 RID: 7080
	public class PlatformConfiguration
	{
		// Token: 0x170019C6 RID: 6598
		// (get) Token: 0x0600D250 RID: 53840 RVA: 0x004C1FFD File Offset: 0x004C01FD
		// (set) Token: 0x0600D251 RID: 53841 RVA: 0x004C2025 File Offset: 0x004C0225
		public EPlatformIdentifier NativePlatform
		{
			get
			{
				if (this.nativePlatform != EPlatformIdentifier.Count)
				{
					return this.nativePlatform;
				}
				Log.Warning(string.Format("[Platform] Platform config file has no valid entry for platform, defaulting to {0}", EPlatformIdentifier.Local));
				return EPlatformIdentifier.Local;
			}
			set
			{
				this.nativePlatform = value;
			}
		}

		// Token: 0x170019C7 RID: 6599
		// (get) Token: 0x0600D252 RID: 53842 RVA: 0x004C202E File Offset: 0x004C022E
		// (set) Token: 0x0600D253 RID: 53843 RVA: 0x004C2056 File Offset: 0x004C0256
		public EPlatformIdentifier CrossPlatform
		{
			get
			{
				if (this.crossPlatform != EPlatformIdentifier.Count)
				{
					return this.crossPlatform;
				}
				Log.Warning(string.Format("[Platform] Platform config file has no valid entry for cross platform, defaulting to {0}", EPlatformIdentifier.None));
				return EPlatformIdentifier.None;
			}
			set
			{
				this.crossPlatform = value;
			}
		}

		// Token: 0x0600D254 RID: 53844 RVA: 0x004C2060 File Offset: 0x004C0260
		public bool ParsePlatform(string _platformGroup, string _value)
		{
			if (string.IsNullOrEmpty(_platformGroup))
			{
				return false;
			}
			if (string.IsNullOrEmpty(_value))
			{
				return false;
			}
			_value = _value.Trim();
			if (_platformGroup == "platform")
			{
				EPlatformIdentifier eplatformIdentifier;
				if (!PlatformManager.TryPlatformIdentifierFromString(_value, out eplatformIdentifier))
				{
					Log.Warning("[Platform] Can not parse platform name '" + _value + "'");
				}
				else
				{
					this.nativePlatform = eplatformIdentifier;
				}
				return true;
			}
			if (_platformGroup == "crossplatform")
			{
				EPlatformIdentifier eplatformIdentifier;
				if (!PlatformManager.TryPlatformIdentifierFromString(_value, out eplatformIdentifier))
				{
					Log.Warning("[Platform] Can not parse cross platform name '" + _value + "'");
				}
				else
				{
					this.crossPlatform = eplatformIdentifier;
				}
				return true;
			}
			if (!(_platformGroup == "serverplatforms"))
			{
				Log.Warning("[Platform] Unsupported platform group specifier '" + _platformGroup + "'");
				return false;
			}
			this.ServerPlatforms.Clear();
			string[] array = _value.Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (!string.IsNullOrEmpty(text))
				{
					EPlatformIdentifier eplatformIdentifier;
					if (!PlatformManager.TryPlatformIdentifierFromString(text, out eplatformIdentifier))
					{
						Log.Warning("[Platform] Can not parse server platform name '" + text + "'");
					}
					else if (eplatformIdentifier == EPlatformIdentifier.Count || eplatformIdentifier == EPlatformIdentifier.None)
					{
						Log.Warning("[Platform] Unsupported platform for server operations '" + text + "'");
					}
					else
					{
						this.ServerPlatforms.Add(eplatformIdentifier);
					}
				}
			}
			return true;
		}

		// Token: 0x0600D255 RID: 53845 RVA: 0x004C21A0 File Offset: 0x004C03A0
		public string WriteString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("platform=");
			stringBuilder.AppendLine(PlatformManager.PlatformStringFromEnum(this.NativePlatform));
			stringBuilder.Append("crossplatform=");
			stringBuilder.AppendLine(PlatformManager.PlatformStringFromEnum(this.CrossPlatform));
			stringBuilder.Append("serverplatforms=");
			foreach (EPlatformIdentifier eplatformIdentifier in this.ServerPlatforms)
			{
				stringBuilder.Append(PlatformManager.PlatformStringFromEnum(eplatformIdentifier));
				stringBuilder.Append(",");
			}
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}

		// Token: 0x0600D256 RID: 53846 RVA: 0x004C2260 File Offset: 0x004C0460
		public void WriteFile(string _configFilename = null)
		{
			if (_configFilename == null)
			{
				_configFilename = GameIO.GetApplicationPath() + "/platform.cfg";
			}
			string text = this.WriteString();
			File.WriteAllText(_configFilename, text);
		}

		// Token: 0x0600D257 RID: 53847 RVA: 0x004C2290 File Offset: 0x004C0490
		public static bool ReadString(ref PlatformConfiguration _result, string _config)
		{
			if (_result == null)
			{
				_result = new PlatformConfiguration();
			}
			using (StringReader stringReader = new StringReader(_config))
			{
				PlatformConfiguration.Parse(ref _result, stringReader);
			}
			return true;
		}

		// Token: 0x0600D258 RID: 53848 RVA: 0x004C22D4 File Offset: 0x004C04D4
		public static bool ReadFile(ref PlatformConfiguration _result, string _configFilename = null)
		{
			if (_result == null)
			{
				_result = new PlatformConfiguration();
			}
			if (_configFilename == null)
			{
				_configFilename = GameIO.GetApplicationPath() + "/platform.cfg";
			}
			if (!File.Exists(_configFilename))
			{
				return false;
			}
			using (StreamReader streamReader = File.OpenText(_configFilename))
			{
				PlatformConfiguration.Parse(ref _result, streamReader);
			}
			return true;
		}

		// Token: 0x0600D259 RID: 53849 RVA: 0x004C2338 File Offset: 0x004C0538
		[PublicizedFrom(EAccessModifier.Private)]
		public static void Parse(ref PlatformConfiguration _result, TextReader _stream)
		{
			while (_stream.Peek() >= 0)
			{
				string[] array = _stream.ReadLine().Split('=', StringSplitOptions.None);
				if (array.Length == 2)
				{
					_result.ParsePlatform(array[0], array[1]);
				}
			}
		}

		// Token: 0x0400A013 RID: 40979
		[PublicizedFrom(EAccessModifier.Private)]
		public const EPlatformIdentifier defaultNativePlatform = EPlatformIdentifier.Local;

		// Token: 0x0400A014 RID: 40980
		[PublicizedFrom(EAccessModifier.Private)]
		public const EPlatformIdentifier defaultCrossPlatform = EPlatformIdentifier.None;

		// Token: 0x0400A015 RID: 40981
		[PublicizedFrom(EAccessModifier.Private)]
		public EPlatformIdentifier nativePlatform = EPlatformIdentifier.Count;

		// Token: 0x0400A016 RID: 40982
		[PublicizedFrom(EAccessModifier.Private)]
		public EPlatformIdentifier crossPlatform = EPlatformIdentifier.Count;

		// Token: 0x0400A017 RID: 40983
		public readonly List<EPlatformIdentifier> ServerPlatforms = new List<EPlatformIdentifier>();
	}
}
