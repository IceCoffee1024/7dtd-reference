using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x02000820 RID: 2080
[Preserve]
public class NetPackagePlayerLogin : NetPackage
{
	// Token: 0x1700065D RID: 1629
	// (get) Token: 0x06003D0A RID: 15626 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003D0B RID: 15627 RVA: 0x00183C7A File Offset: 0x00181E7A
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

	// Token: 0x06003D0C RID: 15628 RVA: 0x00183CAC File Offset: 0x00181EAC
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

	// Token: 0x06003D0D RID: 15629 RVA: 0x00183D28 File Offset: 0x00181F28
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

	// Token: 0x06003D0E RID: 15630 RVA: 0x00183DCE File Offset: 0x00181FCE
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.PlayerLoginRPC(base.Sender, this.playerName, this.platformUserAndToken, this.crossplatformUserAndToken, this.compVersion, this.discordUserId);
	}

	// Token: 0x1700065E RID: 1630
	// (get) Token: 0x06003D0F RID: 15631 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D10 RID: 15632 RVA: 0x00183DFA File Offset: 0x00181FFA
	public override int GetLength()
	{
		return 120;
	}

	// Token: 0x0400314B RID: 12619
	[PublicizedFrom(EAccessModifier.Private)]
	public string playerName;

	// Token: 0x0400314C RID: 12620
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> platformUserAndToken;

	// Token: 0x0400314D RID: 12621
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> crossplatformUserAndToken;

	// Token: 0x0400314E RID: 12622
	[PublicizedFrom(EAccessModifier.Private)]
	public string version;

	// Token: 0x0400314F RID: 12623
	[PublicizedFrom(EAccessModifier.Private)]
	public string compVersion;

	// Token: 0x04003150 RID: 12624
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong discordUserId;
}
