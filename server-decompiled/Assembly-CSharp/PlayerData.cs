using System;
using System.IO;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020008F2 RID: 2290
[Preserve]
public class PlayerData
{
	// Token: 0x0600425B RID: 16987 RVA: 0x0019DCCC File Offset: 0x0019BECC
	public PlayerData(PlatformUserIdentifierAbs _primaryId, PlatformUserIdentifierAbs _nativeId, AuthoredText _playerName, EPlayGroup _playGroup)
	{
		this.PrimaryId = _primaryId;
		this.NativeId = _nativeId;
		this.PlayGroup = _playGroup;
		this.PlayerName = _playerName;
		this.PlatformData = PlatformUserManager.GetOrCreate(_primaryId);
		this.PlatformData.NativeId = _nativeId;
	}

	// Token: 0x0600425C RID: 16988 RVA: 0x0019DD0C File Offset: 0x0019BF0C
	public static PlayerData Read(BinaryReader _reader)
	{
		PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
		PlatformUserIdentifierAbs platformUserIdentifierAbs2 = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
		AuthoredText authoredText = AuthoredText.FromStream(_reader);
		EPlayGroup eplayGroup = (EPlayGroup)_reader.ReadByte();
		GeneratedTextManager.PrefilterText(authoredText, GeneratedTextManager.TextFilteringMode.Filter);
		return new PlayerData(platformUserIdentifierAbs, platformUserIdentifierAbs2, authoredText, eplayGroup);
	}

	// Token: 0x0600425D RID: 16989 RVA: 0x0019DD47 File Offset: 0x0019BF47
	public void Write(BinaryWriter _writer)
	{
		this.PrimaryId.ToStream(_writer, false);
		this.NativeId.ToStream(_writer, false);
		AuthoredText.ToStream(this.PlayerName, _writer);
		_writer.Write((byte)this.PlayGroup);
	}

	// Token: 0x040035BF RID: 13759
	public readonly IPlatformUserData PlatformData;

	// Token: 0x040035C0 RID: 13760
	public readonly PlatformUserIdentifierAbs PrimaryId;

	// Token: 0x040035C1 RID: 13761
	public readonly PlatformUserIdentifierAbs NativeId;

	// Token: 0x040035C2 RID: 13762
	public readonly EPlayGroup PlayGroup;

	// Token: 0x040035C3 RID: 13763
	public readonly AuthoredText PlayerName;
}
