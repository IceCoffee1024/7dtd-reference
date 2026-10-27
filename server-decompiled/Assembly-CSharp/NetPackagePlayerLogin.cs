using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x02000836 RID: 2102
[Preserve]
public class NetPackagePlayerLogin : NetPackage
{
	// Token: 0x1700068D RID: 1677
	// (get) Token: 0x06003D4D RID: 15693 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003D4E RID: 15694 RVA: 0x00186C9A File Offset: 0x00184E9A
	public NetPackagePlayerLogin Setup(string _playerName, [TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> _platformUserAndToken, [TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> _crossplatformUserAndToken, string _version, string _compVersion, ulong _discordUserId)
	{
		this.playerName = _playerName;
		this.platformUserAndToken = _platformUserAndToken;
		this.crossplatformUserAndToken = _crossplatformUserAndToken;
		this.version = _version;
		this.compVersion = _compVersion;
		this.discordUserId = _discordUserId;
		return this;
	}

	// Token: 0x06003D4F RID: 15695 RVA: 0x00186CCC File Offset: 0x00184ECC
	public override void read(PooledBinaryReader _br)
	{
		Log.Out("NPPL.Read");
		this.playerName = _br.ReadString();
		this.platformUserAndToken = new ValueTuple<PlatformUserIdentifierAbs, string>(PlatformUserIdentifierAbs.FromStream(_br, false, true), _br.ReadString());
		this.crossplatformUserAndToken = new ValueTuple<PlatformUserIdentifierAbs, string>(PlatformUserIdentifierAbs.FromStream(_br, false, true), _br.ReadString());
		this.version = _br.ReadString();
		this.compVersion = _br.ReadString();
		this.discordUserId = _br.ReadUInt64();
	}

	// Token: 0x06003D50 RID: 15696 RVA: 0x00186D48 File Offset: 0x00184F48
	public override void write(PooledBinaryWriter _bw)
	{
		Log.Out("NPPL.Write");
		base.write(_bw);
		_bw.Write(this.playerName);
		this.platformUserAndToken.Item1.ToStream(_bw, true);
		_bw.Write(this.platformUserAndToken.Item2 ?? "");
		this.crossplatformUserAndToken.Item1.ToStream(_bw, true);
		_bw.Write(this.crossplatformUserAndToken.Item2 ?? "");
		_bw.Write(this.version);
		_bw.Write(this.compVersion);
		_bw.Write(this.discordUserId);
	}

	// Token: 0x06003D51 RID: 15697 RVA: 0x00186DEE File Offset: 0x00184FEE
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.PlayerLoginRPC(base.Sender, this.playerName, this.platformUserAndToken, this.crossplatformUserAndToken, this.compVersion, this.discordUserId);
	}

	// Token: 0x1700068E RID: 1678
	// (get) Token: 0x06003D52 RID: 15698 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x040031D1 RID: 12753
	[PublicizedFrom(EAccessModifier.Private)]
	public string playerName;

	// Token: 0x040031D2 RID: 12754
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> platformUserAndToken;

	// Token: 0x040031D3 RID: 12755
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> crossplatformUserAndToken;

	// Token: 0x040031D4 RID: 12756
	[PublicizedFrom(EAccessModifier.Private)]
	public string version;

	// Token: 0x040031D5 RID: 12757
	[PublicizedFrom(EAccessModifier.Private)]
	public string compVersion;

	// Token: 0x040031D6 RID: 12758
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong discordUserId;
}
