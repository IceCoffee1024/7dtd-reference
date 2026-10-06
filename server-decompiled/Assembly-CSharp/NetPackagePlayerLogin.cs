using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x0200082C RID: 2092
[Preserve]
public class NetPackagePlayerLogin : NetPackage
{
	// Token: 0x17000687 RID: 1671
	// (get) Token: 0x06003D2A RID: 15658 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003D2B RID: 15659 RVA: 0x00185B96 File Offset: 0x00183D96
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

	// Token: 0x06003D2C RID: 15660 RVA: 0x00185BC8 File Offset: 0x00183DC8
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

	// Token: 0x06003D2D RID: 15661 RVA: 0x00185C44 File Offset: 0x00183E44
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

	// Token: 0x06003D2E RID: 15662 RVA: 0x00185CEA File Offset: 0x00183EEA
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.PlayerLoginRPC(base.Sender, this.playerName, this.platformUserAndToken, this.crossplatformUserAndToken, this.compVersion, this.discordUserId);
	}

	// Token: 0x17000688 RID: 1672
	// (get) Token: 0x06003D2F RID: 15663 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x040031B1 RID: 12721
	[PublicizedFrom(EAccessModifier.Private)]
	public string playerName;

	// Token: 0x040031B2 RID: 12722
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> platformUserAndToken;

	// Token: 0x040031B3 RID: 12723
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> crossplatformUserAndToken;

	// Token: 0x040031B4 RID: 12724
	[PublicizedFrom(EAccessModifier.Private)]
	public string version;

	// Token: 0x040031B5 RID: 12725
	[PublicizedFrom(EAccessModifier.Private)]
	public string compVersion;

	// Token: 0x040031B6 RID: 12726
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong discordUserId;
}
