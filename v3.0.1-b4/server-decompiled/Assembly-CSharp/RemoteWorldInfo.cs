using System;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000D65 RID: 3429
public struct RemoteWorldInfo
{
	// Token: 0x06006826 RID: 26662 RVA: 0x00292834 File Offset: 0x00290A34
	public RemoteWorldInfo(string _gameName, string _worldName, VersionInformation _gameVersion, long _saveSize)
	{
		this.GameName = _gameName;
		this.WorldName = _worldName;
		this.GameVersion = _gameVersion;
		this.SaveSize = _saveSize;
	}

	// Token: 0x06006827 RID: 26663 RVA: 0x00292854 File Offset: 0x00290A54
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

	// Token: 0x06006828 RID: 26664 RVA: 0x00292A34 File Offset: 0x00290C34
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

	// Token: 0x04004FC3 RID: 20419
	public const string FileNameString = "RemoteWorldInfo.xml";

	// Token: 0x04004FC4 RID: 20420
	[PublicizedFrom(EAccessModifier.Private)]
	public const string GameNameString = "gameName";

	// Token: 0x04004FC5 RID: 20421
	[PublicizedFrom(EAccessModifier.Private)]
	public const string WorldNameString = "worldName";

	// Token: 0x04004FC6 RID: 20422
	[PublicizedFrom(EAccessModifier.Private)]
	public const string GameVersionString = "gameVersion";

	// Token: 0x04004FC7 RID: 20423
	[PublicizedFrom(EAccessModifier.Private)]
	public const string SaveSizeString = "saveSize";

	// Token: 0x04004FC8 RID: 20424
	public readonly string GameName;

	// Token: 0x04004FC9 RID: 20425
	public readonly string WorldName;

	// Token: 0x04004FCA RID: 20426
	public readonly VersionInformation GameVersion;

	// Token: 0x04004FCB RID: 20427
	public readonly long SaveSize;
}
