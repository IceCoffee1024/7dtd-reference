using System;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000D93 RID: 3475
public struct RemoteWorldInfo
{
	// Token: 0x060068DF RID: 26847 RVA: 0x0029A23C File Offset: 0x0029843C
	public RemoteWorldInfo(string _gameName, string _worldName, VersionInformation _gameVersion, long _saveSize)
	{
		this.GameName = _gameName;
		this.WorldName = _worldName;
		this.GameVersion = _gameVersion;
		this.SaveSize = _saveSize;
	}

	// Token: 0x060068E0 RID: 26848 RVA: 0x0029A25C File Offset: 0x0029845C
	public static bool TryRead(string _filePath, out RemoteWorldInfo _remoteWorldInfo)
	{
		if (!SdFile.Exists(_filePath))
		{
			_remoteWorldInfo = default(RemoteWorldInfo);
			return false;
		}
		bool flag;
		try
		{
			XElement root = SdXDocument.Load(_filePath).Root;
			string text;
			string text2;
			string text3;
			VersionInformation versionInformation;
			string text4;
			long num;
			if (root == null)
			{
				Debug.LogError("Failed to read RemoteWorldInfo at path \"" + _filePath + "\". Could not find root node.");
				_remoteWorldInfo = default(RemoteWorldInfo);
				flag = false;
			}
			else if (!root.TryGetAttribute("gameName", out text))
			{
				Debug.LogError("Failed to read RemoteWorldInfo at path \"" + _filePath + "\". Could not find gameName attribute.");
				_remoteWorldInfo = default(RemoteWorldInfo);
				flag = false;
			}
			else if (!root.TryGetAttribute("worldName", out text2))
			{
				Debug.LogError("Failed to read RemoteWorldInfo at path \"" + _filePath + "\". Could not find worldName attribute.");
				_remoteWorldInfo = default(RemoteWorldInfo);
				flag = false;
			}
			else if (!root.TryGetAttribute("gameVersion", out text3))
			{
				Debug.LogError("Failed to read RemoteWorldInfo at path \"" + _filePath + "\". Could not find gameVersion attribute.");
				_remoteWorldInfo = default(RemoteWorldInfo);
				flag = false;
			}
			else if (!VersionInformation.TryParseSerializedString(text3, out versionInformation))
			{
				Debug.LogError("Failed to read RemoteWorldInfo at path \"" + _filePath + "\". Failed to parse gameVersion value.");
				_remoteWorldInfo = default(RemoteWorldInfo);
				flag = false;
			}
			else if (!root.TryGetAttribute("saveSize", out text4))
			{
				Debug.LogError("Failed to read RemoteWorldInfo at path \"" + _filePath + "\". Could not find saveSize attribute.");
				_remoteWorldInfo = default(RemoteWorldInfo);
				flag = false;
			}
			else if (!long.TryParse(text4, out num))
			{
				Debug.LogError("Failed to read RemoteWorldInfo at path \"" + _filePath + "\". Failed to parse saveSize value.");
				_remoteWorldInfo = default(RemoteWorldInfo);
				flag = false;
			}
			else
			{
				_remoteWorldInfo = new RemoteWorldInfo(text, text2, versionInformation, num);
				flag = true;
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("Failed to read RemoteWorldInfo at path \"{0}\". Failed with exception: \n\n{1}", _filePath, ex));
			_remoteWorldInfo = default(RemoteWorldInfo);
			flag = false;
		}
		return flag;
	}

	// Token: 0x060068E1 RID: 26849 RVA: 0x0029A43C File Offset: 0x0029863C
	public void Write(string _filePath)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.CreateXmlDeclaration();
		XmlElement xmlElement = xmlDocument.AddXmlElement("RemoteWorldInfo");
		xmlElement.SetAttrib("gameName", this.GameName);
		xmlElement.SetAttrib("worldName", this.WorldName);
		xmlElement.SetAttrib("gameVersion", this.GameVersion.SerializableString);
		xmlElement.SetAttrib("saveSize", this.SaveSize.ToString());
		xmlDocument.SdSave(_filePath);
	}

	// Token: 0x040050C1 RID: 20673
	public const string FileNameString = "RemoteWorldInfo.xml";

	// Token: 0x040050C2 RID: 20674
	[PublicizedFrom(EAccessModifier.Private)]
	public const string GameNameString = "gameName";

	// Token: 0x040050C3 RID: 20675
	[PublicizedFrom(EAccessModifier.Private)]
	public const string WorldNameString = "worldName";

	// Token: 0x040050C4 RID: 20676
	[PublicizedFrom(EAccessModifier.Private)]
	public const string GameVersionString = "gameVersion";

	// Token: 0x040050C5 RID: 20677
	[PublicizedFrom(EAccessModifier.Private)]
	public const string SaveSizeString = "saveSize";

	// Token: 0x040050C6 RID: 20678
	public readonly string GameName;

	// Token: 0x040050C7 RID: 20679
	public readonly string WorldName;

	// Token: 0x040050C8 RID: 20680
	public readonly VersionInformation GameVersion;

	// Token: 0x040050C9 RID: 20681
	public readonly long SaveSize;
}
