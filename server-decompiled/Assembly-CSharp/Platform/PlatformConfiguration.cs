using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Platform
{
	// Token: 0x02001C32 RID: 7218
	public class PlatformConfiguration
	{
		// Token: 0x17001B26 RID: 6950
		// (get) Token: 0x0600D66A RID: 54890 RVA: 0x004D0A01 File Offset: 0x004CEC01
		// (set) Token: 0x0600D66B RID: 54891 RVA: 0x004D0A29 File Offset: 0x004CEC29
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

		// Token: 0x17001B27 RID: 6951
		// (get) Token: 0x0600D66C RID: 54892 RVA: 0x004D0A32 File Offset: 0x004CEC32
		// (set) Token: 0x0600D66D RID: 54893 RVA: 0x004D0A5A File Offset: 0x004CEC5A
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

		// Token: 0x0600D66E RID: 54894 RVA: 0x004D0A64 File Offset: 0x004CEC64
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

		// Token: 0x0600D66F RID: 54895 RVA: 0x004D0BA4 File Offset: 0x004CEDA4
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

		// Token: 0x0600D670 RID: 54896 RVA: 0x004D0C64 File Offset: 0x004CEE64
		public void WriteFile(string _configFilename = null)
		{
			if (_configFilename == null)
			{
				_configFilename = GameIO.GetApplicationPath() + "/platform.cfg";
			}
			string text = this.WriteString();
			File.WriteAllText(_configFilename, text);
		}

		// Token: 0x0600D671 RID: 54897 RVA: 0x004D0C94 File Offset: 0x004CEE94
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

		// Token: 0x0600D672 RID: 54898 RVA: 0x004D0CD8 File Offset: 0x004CEED8
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

		// Token: 0x0600D673 RID: 54899 RVA: 0x004D0D3C File Offset: 0x004CEF3C
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

		// Token: 0x0400A302 RID: 41730
		[PublicizedFrom(EAccessModifier.Private)]
		public const EPlatformIdentifier defaultNativePlatform = EPlatformIdentifier.Local;

		// Token: 0x0400A303 RID: 41731
		[PublicizedFrom(EAccessModifier.Private)]
		public const EPlatformIdentifier defaultCrossPlatform = EPlatformIdentifier.None;

		// Token: 0x0400A304 RID: 41732
		[PublicizedFrom(EAccessModifier.Private)]
		public EPlatformIdentifier nativePlatform = EPlatformIdentifier.Count;

		// Token: 0x0400A305 RID: 41733
		[PublicizedFrom(EAccessModifier.Private)]
		public EPlatformIdentifier crossPlatform = EPlatformIdentifier.Count;

		// Token: 0x0400A306 RID: 41734
		public readonly List<EPlatformIdentifier> ServerPlatforms = new List<EPlatformIdentifier>();
	}
}
