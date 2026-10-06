using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Platform
{
	// Token: 0x02001C01 RID: 7169
	public class PlatformConfiguration
	{
		// Token: 0x17001ADE RID: 6878
		// (get) Token: 0x0600D556 RID: 54614 RVA: 0x004CCED9 File Offset: 0x004CB0D9
		// (set) Token: 0x0600D557 RID: 54615 RVA: 0x004CCF01 File Offset: 0x004CB101
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

		// Token: 0x17001ADF RID: 6879
		// (get) Token: 0x0600D558 RID: 54616 RVA: 0x004CCF0A File Offset: 0x004CB10A
		// (set) Token: 0x0600D559 RID: 54617 RVA: 0x004CCF32 File Offset: 0x004CB132
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

		// Token: 0x0600D55A RID: 54618 RVA: 0x004CCF3C File Offset: 0x004CB13C
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

		// Token: 0x0600D55B RID: 54619 RVA: 0x004CD07C File Offset: 0x004CB27C
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

		// Token: 0x0600D55C RID: 54620 RVA: 0x004CD13C File Offset: 0x004CB33C
		public void WriteFile(string _configFilename = null)
		{
			if (_configFilename == null)
			{
				_configFilename = GameIO.GetApplicationPath() + "/platform.cfg";
			}
			string text = this.WriteString();
			File.WriteAllText(_configFilename, text);
		}

		// Token: 0x0600D55D RID: 54621 RVA: 0x004CD16C File Offset: 0x004CB36C
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

		// Token: 0x0600D55E RID: 54622 RVA: 0x004CD1B0 File Offset: 0x004CB3B0
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

		// Token: 0x0600D55F RID: 54623 RVA: 0x004CD214 File Offset: 0x004CB414
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

		// Token: 0x0400A219 RID: 41497
		[PublicizedFrom(EAccessModifier.Private)]
		public const EPlatformIdentifier defaultNativePlatform = EPlatformIdentifier.Local;

		// Token: 0x0400A21A RID: 41498
		[PublicizedFrom(EAccessModifier.Private)]
		public const EPlatformIdentifier defaultCrossPlatform = EPlatformIdentifier.None;

		// Token: 0x0400A21B RID: 41499
		[PublicizedFrom(EAccessModifier.Private)]
		public EPlatformIdentifier nativePlatform = EPlatformIdentifier.Count;

		// Token: 0x0400A21C RID: 41500
		[PublicizedFrom(EAccessModifier.Private)]
		public EPlatformIdentifier crossPlatform = EPlatformIdentifier.Count;

		// Token: 0x0400A21D RID: 41501
		public readonly List<EPlatformIdentifier> ServerPlatforms = new List<EPlatformIdentifier>();
	}
}
