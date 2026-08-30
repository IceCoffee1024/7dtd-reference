using System;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000D79 RID: 3449
public struct RemoteWorldInfo
{
	// Token: 0x060068C2 RID: 26818 RVA: 0x00295988 File Offset: 0x00293B88
	public RemoteWorldInfo(string _gameName, string _worldName, VersionInformation _gameVersion, long _saveSize)
	{
		this.GameName = _gameName;
		this.WorldName = _worldName;
		this.GameVersion = _gameVersion;
		this.SaveSize = _saveSize;
	}

	// Token: 0x060068C3 RID: 26819 RVA: 0x002959A8 File Offset: 0x00293BA8
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

	// Token: 0x060068C4 RID: 26820 RVA: 0x00295B88 File Offset: 0x00293D88
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

	// Token: 0x04005065 RID: 20581
	public const string FileNameString = "RemoteWorldInfo.xml";

	// Token: 0x04005066 RID: 20582
	[PublicizedFrom(EAccessModifier.Private)]
	public const string GameNameString = "gameName";

	// Token: 0x04005067 RID: 20583
	[PublicizedFrom(EAccessModifier.Private)]
	public const string WorldNameString = "worldName";

	// Token: 0x04005068 RID: 20584
	[PublicizedFrom(EAccessModifier.Private)]
	public const string GameVersionString = "gameVersion";

	// Token: 0x04005069 RID: 20585
	[PublicizedFrom(EAccessModifier.Private)]
	public const string SaveSizeString = "saveSize";

	// Token: 0x0400506A RID: 20586
	public readonly string GameName;

	// Token: 0x0400506B RID: 20587
	public readonly string WorldName;

	// Token: 0x0400506C RID: 20588
	public readonly VersionInformation GameVersion;

	// Token: 0x0400506D RID: 20589
	public readonly long SaveSize;
}
