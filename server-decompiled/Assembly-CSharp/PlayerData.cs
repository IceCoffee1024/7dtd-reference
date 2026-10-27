using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000901 RID: 2305
[Preserve]
public class PlayerData
{
	// Token: 0x06004210 RID: 16912 RVA: 0x0019EE78 File Offset: 0x0019D078
	public PlayerData(PlatformUserIdentifierAbs _primaryId, PlatformUserIdentifierAbs _nativeId, AuthoredText _playerName, EPlayGroup _playGroup)
	{
		this.PrimaryId = _primaryId;
		this.NativeId = _nativeId;
		this.PlayGroup = _playGroup;
		this.PlayerName = _playerName;
		this.PlatformData = PlatformUserManager.GetOrCreate(_primaryId);
		this.PlatformData.NativeId = _nativeId;
	}

	// Token: 0x06004211 RID: 16913 RVA: 0x0019EEB8 File Offset: 0x0019D0B8
	public static PlayerData Read(PooledBinaryReader _reader)
	{
		PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
		PlatformUserIdentifierAbs platformUserIdentifierAbs2 = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
		AuthoredText authoredText = AuthoredText.FromStream(_reader);
		EPlayGroup eplayGroup = (EPlayGroup)_reader.ReadByte();
		GeneratedTextManager.PrefilterText(authoredText, GeneratedTextManager.TextFilteringMode.Filter);
		return new PlayerData(platformUserIdentifierAbs, platformUserIdentifierAbs2, authoredText, eplayGroup);
	}

	// Token: 0x06004212 RID: 16914 RVA: 0x0019EEF3 File Offset: 0x0019D0F3
	public void Write(PooledBinaryWriter _writer)
	{
		this.PrimaryId.ToStream(_writer, false);
		this.NativeId.ToStream(_writer, false);
		AuthoredText.ToStream(this.PlayerName, _writer);
		_writer.Write((byte)this.PlayGroup);
	}

	// Token: 0x040035D3 RID: 13779
	public readonly IPlatformUserData PlatformData;

	// Token: 0x040035D4 RID: 13780
	public readonly PlatformUserIdentifierAbs PrimaryId;

	// Token: 0x040035D5 RID: 13781
	public readonly PlatformUserIdentifierAbs NativeId;

	// Token: 0x040035D6 RID: 13782
	public readonly EPlayGroup PlayGroup;

	// Token: 0x040035D7 RID: 13783
	public readonly AuthoredText PlayerName;
}
