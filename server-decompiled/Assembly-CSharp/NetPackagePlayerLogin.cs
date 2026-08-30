using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x0200082B RID: 2091
[Preserve]
public class NetPackagePlayerLogin : NetPackage
{
	// Token: 0x17000668 RID: 1640
	// (get) Token: 0x06003D69 RID: 15721 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003D6A RID: 15722 RVA: 0x00186052 File Offset: 0x00184252
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

	// Token: 0x06003D6B RID: 15723 RVA: 0x00186084 File Offset: 0x00184284
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

	// Token: 0x06003D6C RID: 15724 RVA: 0x00186100 File Offset: 0x00184300
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

	// Token: 0x06003D6D RID: 15725 RVA: 0x001861A6 File Offset: 0x001843A6
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.PlayerLoginRPC(base.Sender, this.playerName, this.platformUserAndToken, this.crossplatformUserAndToken, this.compVersion, this.discordUserId);
	}

	// Token: 0x17000669 RID: 1641
	// (get) Token: 0x06003D6E RID: 15726 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D6F RID: 15727 RVA: 0x001861D2 File Offset: 0x001843D2
	public override int GetLength()
	{
		return 120;
	}

	// Token: 0x040031CA RID: 12746
	[PublicizedFrom(EAccessModifier.Private)]
	public string playerName;

	// Token: 0x040031CB RID: 12747
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> platformUserAndToken;

	// Token: 0x040031CC RID: 12748
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> crossplatformUserAndToken;

	// Token: 0x040031CD RID: 12749
	[PublicizedFrom(EAccessModifier.Private)]
	public string version;

	// Token: 0x040031CE RID: 12750
	[PublicizedFrom(EAccessModifier.Private)]
	public string compVersion;

	// Token: 0x040031CF RID: 12751
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong discordUserId;
}
