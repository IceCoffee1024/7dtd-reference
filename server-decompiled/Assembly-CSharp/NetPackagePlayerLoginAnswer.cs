using System;
using System.Runtime.CompilerServices;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000837 RID: 2103
[Preserve]
public class NetPackagePlayerLoginAnswer : NetPackage
{
	// Token: 0x1700068F RID: 1679
	// (get) Token: 0x06003D54 RID: 15700 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003D55 RID: 15701 RVA: 0x00186E1A File Offset: 0x0018501A
	public NetPackagePlayerLoginAnswer Setup(bool _bAllowed, string _data, PlatformLobbyId _platformLobbyId, [TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> _platformUserAndToken, [TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> _crossplatformUserAndToken)
	{
		this.bAllowed = _bAllowed;
		this.data = _data;
		this.platformLobbyId = _platformLobbyId;
		this.platformUserAndToken = _platformUserAndToken;
		this.crossplatformUserAndToken = _crossplatformUserAndToken;
		this.RecalcLength();
		return this;
	}

	// Token: 0x06003D56 RID: 15702 RVA: 0x00186E48 File Offset: 0x00185048
	[PublicizedFrom(EAccessModifier.Private)]
	public void RecalcLength()
	{
		Encoding utf = Encoding.UTF8;
		this.length = 1 + this.data.GetBinaryWriterLength(utf) + this.platformLobbyId.GetWriteLength(utf) + this.platformUserAndToken.Item1.GetToStreamLength(utf, true) + (this.platformUserAndToken.Item2 ?? "").GetBinaryWriterLength(utf) + this.crossplatformUserAndToken.Item1.GetToStreamLength(utf, true) + (this.crossplatformUserAndToken.Item2 ?? "").GetBinaryWriterLength(utf);
	}

	// Token: 0x06003D57 RID: 15703 RVA: 0x00186ED8 File Offset: 0x001850D8
	public override void read(PooledBinaryReader _br)
	{
		this.bAllowed = _br.ReadBoolean();
		this.data = _br.ReadString();
		this.platformLobbyId = PlatformLobbyId.Read(_br);
		this.platformUserAndToken = new ValueTuple<PlatformUserIdentifierAbs, string>(PlatformUserIdentifierAbs.FromStream(_br, false, true), _br.ReadString());
		this.crossplatformUserAndToken = new ValueTuple<PlatformUserIdentifierAbs, string>(PlatformUserIdentifierAbs.FromStream(_br, false, true), _br.ReadString());
		this.RecalcLength();
	}

	// Token: 0x06003D58 RID: 15704 RVA: 0x00186F44 File Offset: 0x00185144
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.bAllowed);
		_bw.Write(this.data);
		this.platformLobbyId.Write(_bw);
		this.platformUserAndToken.Item1.ToStream(_bw, true);
		_bw.Write(this.platformUserAndToken.Item2 ?? "");
		this.crossplatformUserAndToken.Item1.ToStream(_bw, true);
		_bw.Write(this.crossplatformUserAndToken.Item2 ?? "");
	}

	// Token: 0x06003D59 RID: 15705 RVA: 0x00186FD4 File Offset: 0x001851D4
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (this.bAllowed)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.PlayerAllowed(this.data, this.platformLobbyId, this.platformUserAndToken, this.crossplatformUserAndToken);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.PlayerDenied(this.data);
	}

	// Token: 0x17000690 RID: 1680
	// (get) Token: 0x06003D5A RID: 15706 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040031D7 RID: 12759
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAllowed;

	// Token: 0x040031D8 RID: 12760
	[PublicizedFrom(EAccessModifier.Private)]
	public string data;

	// Token: 0x040031D9 RID: 12761
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformLobbyId platformLobbyId;

	// Token: 0x040031DA RID: 12762
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> platformUserAndToken;

	// Token: 0x040031DB RID: 12763
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> crossplatformUserAndToken;

	// Token: 0x040031DC RID: 12764
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;
}
