using System;
using System.IO;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020008E6 RID: 2278
[Preserve]
public class PlayerData
{
	// Token: 0x060041F6 RID: 16886 RVA: 0x0019BBF4 File Offset: 0x00199DF4
	public PlayerData(PlatformUserIdentifierAbs _primaryId, PlatformUserIdentifierAbs _nativeId, AuthoredText _playerName, EPlayGroup _playGroup)
	{
		this.PrimaryId = _primaryId;
		this.NativeId = _nativeId;
		this.PlayGroup = _playGroup;
		this.PlayerName = _playerName;
		this.PlatformData = PlatformUserManager.GetOrCreate(_primaryId);
		this.PlatformData.NativeId = _nativeId;
	}

	// Token: 0x060041F7 RID: 16887 RVA: 0x0019BC34 File Offset: 0x00199E34
	public static PlayerData Read(BinaryReader _reader)
	{
		PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
		PlatformUserIdentifierAbs platformUserIdentifierAbs2 = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
		AuthoredText authoredText = AuthoredText.FromStream(_reader);
		EPlayGroup eplayGroup = (EPlayGroup)_reader.ReadByte();
		GeneratedTextManager.PrefilterText(authoredText, GeneratedTextManager.TextFilteringMode.Filter);
		return new PlayerData(platformUserIdentifierAbs, platformUserIdentifierAbs2, authoredText, eplayGroup);
	}

	// Token: 0x060041F8 RID: 16888 RVA: 0x0019BC6F File Offset: 0x00199E6F
	public void Write(BinaryWriter _writer)
	{
		this.PrimaryId.ToStream(_writer, false);
		this.NativeId.ToStream(_writer, false);
		AuthoredText.ToStream(this.PlayerName, _writer);
		_writer.Write((byte)this.PlayGroup);
	}

	// Token: 0x0400353E RID: 13630
	public readonly IPlatformUserData PlatformData;

	// Token: 0x0400353F RID: 13631
	public readonly PlatformUserIdentifierAbs PrimaryId;

	// Token: 0x04003540 RID: 13632
	public readonly PlatformUserIdentifierAbs NativeId;

	// Token: 0x04003541 RID: 13633
	public readonly EPlayGroup PlayGroup;

	// Token: 0x04003542 RID: 13634
	public readonly AuthoredText PlayerName;
}
