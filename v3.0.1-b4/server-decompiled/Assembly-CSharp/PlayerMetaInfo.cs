using System;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000D66 RID: 3430
public struct PlayerMetaInfo
{
	// Token: 0x06006829 RID: 26665 RVA: 0x00292AB1 File Offset: 0x00290CB1
	public PlayerMetaInfo(PlatformUserIdentifierAbs _nativeId, string _name, int _level, float _distanceWalked)
	{
		this.NativeId = _nativeId;
		this.Name = _name;
		this.Level = _level;
		this.DistanceWalked = _distanceWalked;
	}

	// Token: 0x0600682A RID: 26666 RVA: 0x00292AD0 File Offset: 0x00290CD0
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

	// Token: 0x0600682B RID: 26667 RVA: 0x00292C90 File Offset: 0x00290E90
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

	// Token: 0x0600682C RID: 26668 RVA: 0x00292D24 File Offset: 0x00290F24
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

	// Token: 0x0600682D RID: 26669 RVA: 0x00292D6C File Offset: 0x00290F6C
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

	// Token: 0x04004FCC RID: 20428
	public const string Ext = "meta";

	// Token: 0x04004FCD RID: 20429
	[PublicizedFrom(EAccessModifier.Private)]
	public const string NativeIdAttr = "nativeid";

	// Token: 0x04004FCE RID: 20430
	[PublicizedFrom(EAccessModifier.Private)]
	public const string NameAttr = "name";

	// Token: 0x04004FCF RID: 20431
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LevelAttr = "level";

	// Token: 0x04004FD0 RID: 20432
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DistanceWalkedAttr = "distanceWalked";

	// Token: 0x04004FD1 RID: 20433
	public readonly PlatformUserIdentifierAbs NativeId;

	// Token: 0x04004FD2 RID: 20434
	public readonly string Name;

	// Token: 0x04004FD3 RID: 20435
	public readonly int Level;

	// Token: 0x04004FD4 RID: 20436
	public readonly float DistanceWalked;
}
