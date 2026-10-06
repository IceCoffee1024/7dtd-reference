using System;
using System.Runtime.CompilerServices;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200082D RID: 2093
[Preserve]
public class NetPackagePlayerLoginAnswer : NetPackage
{
	// Token: 0x17000689 RID: 1673
	// (get) Token: 0x06003D31 RID: 15665 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003D32 RID: 15666 RVA: 0x00185D16 File Offset: 0x00183F16
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

	// Token: 0x06003D33 RID: 15667 RVA: 0x00185D44 File Offset: 0x00183F44
	[PublicizedFrom(EAccessModifier.Private)]
	public void RecalcLength()
	{
		Encoding utf = Encoding.UTF8;
		this.length = 1 + this.data.GetBinaryWriterLength(utf) + this.platformLobbyId.GetWriteLength(utf) + this.platformUserAndToken.Item1.GetToStreamLength(utf, true) + (this.platformUserAndToken.Item2 ?? "").GetBinaryWriterLength(utf) + this.crossplatformUserAndToken.Item1.GetToStreamLength(utf, true) + (this.crossplatformUserAndToken.Item2 ?? "").GetBinaryWriterLength(utf);
	}

	// Token: 0x06003D34 RID: 15668 RVA: 0x00185DD4 File Offset: 0x00183FD4
	public override void read(PooledBinaryReader _br)
	{
		this.bAllowed = _br.ReadBoolean();
		this.data = _br.ReadString();
		this.platformLobbyId = PlatformLobbyId.Read(_br);
		this.platformUserAndToken = new ValueTuple<PlatformUserIdentifierAbs, string>(PlatformUserIdentifierAbs.FromStream(_br, false, true), _br.ReadString());
		this.crossplatformUserAndToken = new ValueTuple<PlatformUserIdentifierAbs, string>(PlatformUserIdentifierAbs.FromStream(_br, false, true), _br.ReadString());
		this.RecalcLength();
	}

	// Token: 0x06003D35 RID: 15669 RVA: 0x00185E40 File Offset: 0x00184040
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

	// Token: 0x06003D36 RID: 15670 RVA: 0x00185ED0 File Offset: 0x001840D0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (this.bAllowed)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.PlayerAllowed(this.data, this.platformLobbyId, this.platformUserAndToken, this.crossplatformUserAndToken);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.PlayerDenied(this.data);
	}

	// Token: 0x1700068A RID: 1674
	// (get) Token: 0x06003D37 RID: 15671 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040031B7 RID: 12727
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAllowed;

	// Token: 0x040031B8 RID: 12728
	[PublicizedFrom(EAccessModifier.Private)]
	public string data;

	// Token: 0x040031B9 RID: 12729
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformLobbyId platformLobbyId;

	// Token: 0x040031BA RID: 12730
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> platformUserAndToken;

	// Token: 0x040031BB RID: 12731
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> crossplatformUserAndToken;

	// Token: 0x040031BC RID: 12732
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;
}
