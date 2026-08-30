using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Platform
{
	// Token: 0x02001BCF RID: 7119
	public class PlatformConfiguration
	{
		// Token: 0x17001A38 RID: 6712
		// (get) Token: 0x0600D3DE RID: 54238 RVA: 0x004C83F1 File Offset: 0x004C65F1
		// (set) Token: 0x0600D3DF RID: 54239 RVA: 0x004C8419 File Offset: 0x004C6619
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

		// Token: 0x17001A39 RID: 6713
		// (get) Token: 0x0600D3E0 RID: 54240 RVA: 0x004C8422 File Offset: 0x004C6622
		// (set) Token: 0x0600D3E1 RID: 54241 RVA: 0x004C844A File Offset: 0x004C664A
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

		// Token: 0x0600D3E2 RID: 54242 RVA: 0x004C8454 File Offset: 0x004C6654
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

		// Token: 0x0600D3E3 RID: 54243 RVA: 0x004C8594 File Offset: 0x004C6794
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

		// Token: 0x0600D3E4 RID: 54244 RVA: 0x004C8654 File Offset: 0x004C6854
		public void WriteFile(string _configFilename = null)
		{
			if (_configFilename == null)
			{
				_configFilename = GameIO.GetApplicationPath() + "/platform.cfg";
			}
			string text = this.WriteString();
			File.WriteAllText(_configFilename, text);
		}

		// Token: 0x0600D3E5 RID: 54245 RVA: 0x004C8684 File Offset: 0x004C6884
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

		// Token: 0x0600D3E6 RID: 54246 RVA: 0x004C86C8 File Offset: 0x004C68C8
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

		// Token: 0x0600D3E7 RID: 54247 RVA: 0x004C872C File Offset: 0x004C692C
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

		// Token: 0x0400A144 RID: 41284
		[PublicizedFrom(EAccessModifier.Private)]
		public const EPlatformIdentifier defaultNativePlatform = EPlatformIdentifier.Local;

		// Token: 0x0400A145 RID: 41285
		[PublicizedFrom(EAccessModifier.Private)]
		public const EPlatformIdentifier defaultCrossPlatform = EPlatformIdentifier.None;

		// Token: 0x0400A146 RID: 41286
		[PublicizedFrom(EAccessModifier.Private)]
		public EPlatformIdentifier nativePlatform = EPlatformIdentifier.Count;

		// Token: 0x0400A147 RID: 41287
		[PublicizedFrom(EAccessModifier.Private)]
		public EPlatformIdentifier crossPlatform = EPlatformIdentifier.Count;

		// Token: 0x0400A148 RID: 41288
		public readonly List<EPlatformIdentifier> ServerPlatforms = new List<EPlatformIdentifier>();
	}
}
