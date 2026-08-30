using System;
using System.Runtime.CompilerServices;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200082C RID: 2092
[Preserve]
public class NetPackagePlayerLoginAnswer : NetPackage
{
	// Token: 0x1700066A RID: 1642
	// (get) Token: 0x06003D71 RID: 15729 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003D72 RID: 15730 RVA: 0x001861D6 File Offset: 0x001843D6
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

	// Token: 0x06003D73 RID: 15731 RVA: 0x00186204 File Offset: 0x00184404
	[PublicizedFrom(EAccessModifier.Private)]
	public void RecalcLength()
	{
		Encoding utf = Encoding.UTF8;
		this.length = 1 + this.data.GetBinaryWriterLength(utf) + this.platformLobbyId.GetWriteLength(utf) + this.platformUserAndToken.Item1.GetToStreamLength(utf, true) + (this.platformUserAndToken.Item2 ?? "").GetBinaryWriterLength(utf) + this.crossplatformUserAndToken.Item1.GetToStreamLength(utf, true) + (this.crossplatformUserAndToken.Item2 ?? "").GetBinaryWriterLength(utf);
	}

	// Token: 0x06003D74 RID: 15732 RVA: 0x00186294 File Offset: 0x00184494
	public override void read(PooledBinaryReader _br)
	{
		this.bAllowed = _br.ReadBoolean();
		this.data = _br.ReadString();
		this.platformLobbyId = PlatformLobbyId.Read(_br);
		this.platformUserAndToken = new ValueTuple<PlatformUserIdentifierAbs, string>(PlatformUserIdentifierAbs.FromStream(_br, false, true), _br.ReadString());
		this.crossplatformUserAndToken = new ValueTuple<PlatformUserIdentifierAbs, string>(PlatformUserIdentifierAbs.FromStream(_br, false, true), _br.ReadString());
		this.RecalcLength();
	}

	// Token: 0x06003D75 RID: 15733 RVA: 0x00186300 File Offset: 0x00184500
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

	// Token: 0x06003D76 RID: 15734 RVA: 0x00186390 File Offset: 0x00184590
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (this.bAllowed)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.PlayerAllowed(this.data, this.platformLobbyId, this.platformUserAndToken, this.crossplatformUserAndToken);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.PlayerDenied(this.data);
	}

	// Token: 0x1700066B RID: 1643
	// (get) Token: 0x06003D77 RID: 15735 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003D78 RID: 15736 RVA: 0x001863CD File Offset: 0x001845CD
	public override int GetLength()
	{
		return this.length;
	}

	// Token: 0x040031D0 RID: 12752
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAllowed;

	// Token: 0x040031D1 RID: 12753
	[PublicizedFrom(EAccessModifier.Private)]
	public string data;

	// Token: 0x040031D2 RID: 12754
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformLobbyId platformLobbyId;

	// Token: 0x040031D3 RID: 12755
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> platformUserAndToken;

	// Token: 0x040031D4 RID: 12756
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> crossplatformUserAndToken;

	// Token: 0x040031D5 RID: 12757
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;
}
