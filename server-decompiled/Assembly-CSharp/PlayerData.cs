using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020008F7 RID: 2295
[Preserve]
public class PlayerData
{
	// Token: 0x060041ED RID: 16877 RVA: 0x0019DDD0 File Offset: 0x0019BFD0
	public PlayerData(PlatformUserIdentifierAbs _primaryId, PlatformUserIdentifierAbs _nativeId, AuthoredText _playerName, EPlayGroup _playGroup)
	{
		this.PrimaryId = _primaryId;
		this.NativeId = _nativeId;
		this.PlayGroup = _playGroup;
		this.PlayerName = _playerName;
		this.PlatformData = PlatformUserManager.GetOrCreate(_primaryId);
		this.PlatformData.NativeId = _nativeId;
	}

	// Token: 0x060041EE RID: 16878 RVA: 0x0019DE10 File Offset: 0x0019C010
	public static PlayerData Read(PooledBinaryReader _reader)
	{
		PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
		PlatformUserIdentifierAbs platformUserIdentifierAbs2 = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
		AuthoredText authoredText = AuthoredText.FromStream(_reader);
		EPlayGroup eplayGroup = (EPlayGroup)_reader.ReadByte();
		GeneratedTextManager.PrefilterText(authoredText, GeneratedTextManager.TextFilteringMode.Filter);
		return new PlayerData(platformUserIdentifierAbs, platformUserIdentifierAbs2, authoredText, eplayGroup);
	}

	// Token: 0x060041EF RID: 16879 RVA: 0x0019DE4B File Offset: 0x0019C04B
	public void Write(PooledBinaryWriter _writer)
	{
		this.PrimaryId.ToStream(_writer, false);
		this.NativeId.ToStream(_writer, false);
		AuthoredText.ToStream(this.PlayerName, _writer);
		_writer.Write((byte)this.PlayGroup);
	}

	// Token: 0x040035B3 RID: 13747
	public readonly IPlatformUserData PlatformData;

	// Token: 0x040035B4 RID: 13748
	public readonly PlatformUserIdentifierAbs PrimaryId;

	// Token: 0x040035B5 RID: 13749
	public readonly PlatformUserIdentifierAbs NativeId;

	// Token: 0x040035B6 RID: 13750
	public readonly EPlayGroup PlayGroup;

	// Token: 0x040035B7 RID: 13751
	public readonly AuthoredText PlayerName;
}
