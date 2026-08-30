using System;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000D7A RID: 3450
public struct PlayerMetaInfo
{
	// Token: 0x060068C5 RID: 26821 RVA: 0x00295C05 File Offset: 0x00293E05
	public PlayerMetaInfo(PlatformUserIdentifierAbs _nativeId, string _name, int _level, float _distanceWalked)
	{
		this.NativeId = _nativeId;
		this.Name = _name;
		this.Level = _level;
		this.DistanceWalked = _distanceWalked;
	}

	// Token: 0x060068C6 RID: 26822 RVA: 0x00295C24 File Offset: 0x00293E24
	public static bool TryRead(string _filePath, out PlayerMetaInfo _playerMetaInfo)
	{
		if (!SdFile.Exists(_filePath))
		{
			Debug.LogError("Failed to read PlayerMetaInfo. No file found at path: " + _filePath);
			_playerMetaInfo = default(PlayerMetaInfo);
			return false;
		}
		bool flag;
		try
		{
			XElement root = SdXDocument.Load(_filePath).Root;
			if (root == null)
			{
				Debug.LogError("Failed to read PlayerMetaInfo at path \"" + _filePath + "\". Could not find root node.");
				_playerMetaInfo = default(PlayerMetaInfo);
				flag = false;
			}
			else
			{
				string text;
				if (!root.TryGetAttribute("name", out text))
				{
					Debug.LogWarning("No name in PlayerMetaInfo at path \"" + _filePath + "\". Could not find name attribute.");
					text = null;
				}
				string text2;
				PlatformUserIdentifierAbs platformUserIdentifierAbs;
				if (!root.TryGetAttribute("nativeid", out text2))
				{
					Debug.LogWarning("No native id in PlayerMetaInfo at path \"" + _filePath + "\". Could not find nativeid attribute.");
					platformUserIdentifierAbs = null;
				}
				else if (!PlatformUserIdentifierAbs.TryFromCombinedString(text2, out platformUserIdentifierAbs))
				{
					Debug.LogError("Could not parse native id from PlayerMetaInfo at path \"" + _filePath + "\". Combined id string: " + text2);
					_playerMetaInfo = default(PlayerMetaInfo);
					return false;
				}
				string text3;
				int num;
				string text4;
				float num2;
				if (!root.TryGetAttribute("level", out text3) || !int.TryParse(text3, out num))
				{
					Debug.LogError("Failed to read PlayerMetaInfo at path \"" + _filePath + "\". Could not find level attribute.");
					_playerMetaInfo = default(PlayerMetaInfo);
					flag = false;
				}
				else if (!root.TryGetAttribute("distanceWalked", out text4) || !float.TryParse(text4, out num2))
				{
					Debug.LogError("Failed to read PlayerMetaInfo at path \"" + _filePath + "\". Could not find distanceWalked attribute.");
					_playerMetaInfo = default(PlayerMetaInfo);
					flag = false;
				}
				else
				{
					_playerMetaInfo = new PlayerMetaInfo(platformUserIdentifierAbs, text, num, num2);
					flag = true;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("Failed to read PlayerMetaInfo at path \"{0}\". Failed with exception: \n\n{1}", _filePath, ex));
			_playerMetaInfo = default(PlayerMetaInfo);
			flag = false;
		}
		return flag;
	}

	// Token: 0x060068C7 RID: 26823 RVA: 0x00295DE4 File Offset: 0x00293FE4
	public void Write(string _filePath)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.CreateXmlDeclaration();
		XmlElement xmlElement = xmlDocument.AddXmlElement("PlayerMetaInfo");
		if (this.NativeId != null)
		{
			xmlElement.SetAttribute("nativeid", this.NativeId.CombinedString);
		}
		if (this.Name != null)
		{
			xmlElement.SetAttribute("name", this.Name);
		}
		xmlElement.SetAttribute("level", this.Level.ToString());
		xmlElement.SetAttribute("distanceWalked", this.DistanceWalked.ToString());
		xmlDocument.SdSave(_filePath);
	}

	// Token: 0x060068C8 RID: 26824 RVA: 0x00295E78 File Offset: 0x00294078
	public static PlayerMetaInfo FromStream(PooledBinaryReader _reader)
	{
		PlatformUserIdentifierAbs platformUserIdentifierAbs = null;
		if (_reader.ReadBoolean())
		{
			platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
		}
		string text = null;
		if (_reader.ReadBoolean())
		{
			text = _reader.ReadString();
		}
		int num = _reader.ReadInt32();
		float num2 = _reader.ReadSingle();
		return new PlayerMetaInfo(platformUserIdentifierAbs, text, num, num2);
	}

	// Token: 0x060068C9 RID: 26825 RVA: 0x00295EC0 File Offset: 0x002940C0
	public void Write(PooledBinaryWriter _writer)
	{
		bool flag = this.NativeId != null;
		_writer.Write(flag);
		if (flag)
		{
			this.NativeId.ToStream(_writer, false);
		}
		bool flag2 = this.Name != null;
		_writer.Write(flag2);
		if (flag2)
		{
			_writer.Write(this.Name);
		}
		_writer.Write(this.Level);
		_writer.Write(this.DistanceWalked);
	}

	// Token: 0x0400506E RID: 20590
	public const string Ext = "meta";

	// Token: 0x0400506F RID: 20591
	[PublicizedFrom(EAccessModifier.Private)]
	public const string NativeIdAttr = "nativeid";

	// Token: 0x04005070 RID: 20592
	[PublicizedFrom(EAccessModifier.Private)]
	public const string NameAttr = "name";

	// Token: 0x04005071 RID: 20593
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LevelAttr = "level";

	// Token: 0x04005072 RID: 20594
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DistanceWalkedAttr = "distanceWalked";

	// Token: 0x04005073 RID: 20595
	public readonly PlatformUserIdentifierAbs NativeId;

	// Token: 0x04005074 RID: 20596
	public readonly string Name;

	// Token: 0x04005075 RID: 20597
	public readonly int Level;

	// Token: 0x04005076 RID: 20598
	public readonly float DistanceWalked;
}
