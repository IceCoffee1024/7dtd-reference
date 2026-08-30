using System;
using System.Runtime.CompilerServices;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000821 RID: 2081
[Preserve]
public class NetPackagePlayerLoginAnswer : NetPackage
{
	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x06003D12 RID: 15634 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003D13 RID: 15635 RVA: 0x00183DFE File Offset: 0x00181FFE
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

	// Token: 0x06003D14 RID: 15636 RVA: 0x00183E2C File Offset: 0x0018202C
	[PublicizedFrom(EAccessModifier.Private)]
	public void RecalcLength()
	{
		Encoding utf = Encoding.UTF8;
		this.length = 1 + this.data.GetBinaryWriterLength(utf) + this.platformLobbyId.GetWriteLength(utf) + this.platformUserAndToken.Item1.GetToStreamLength(utf, true) + (this.platformUserAndToken.Item2 ?? "").GetBinaryWriterLength(utf) + this.crossplatformUserAndToken.Item1.GetToStreamLength(utf, true) + (this.crossplatformUserAndToken.Item2 ?? "").GetBinaryWriterLength(utf);
	}

	// Token: 0x06003D15 RID: 15637 RVA: 0x00183EBC File Offset: 0x001820BC
	public override void read(PooledBinaryReader _br)
	{
		this.bAllowed = _br.ReadBoolean();
		this.data = _br.ReadString();
		this.platformLobbyId = PlatformLobbyId.Read(_br);
		this.platformUserAndToken = new ValueTuple<PlatformUserIdentifierAbs, string>(PlatformUserIdentifierAbs.FromStream(_br, false, true), _br.ReadString());
		this.crossplatformUserAndToken = new ValueTuple<PlatformUserIdentifierAbs, string>(PlatformUserIdentifierAbs.FromStream(_br, false, true), _br.ReadString());
		this.RecalcLength();
	}

	// Token: 0x06003D16 RID: 15638 RVA: 0x00183F28 File Offset: 0x00182128
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

	// Token: 0x06003D17 RID: 15639 RVA: 0x00183FB8 File Offset: 0x001821B8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (this.bAllowed)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.PlayerAllowed(this.data, this.platformLobbyId, this.platformUserAndToken, this.crossplatformUserAndToken);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.PlayerDenied(this.data);
	}

	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x06003D18 RID: 15640 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003D19 RID: 15641 RVA: 0x00183FF5 File Offset: 0x001821F5
	public override int GetLength()
	{
		return this.length;
	}

	// Token: 0x04003151 RID: 12625
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAllowed;

	// Token: 0x04003152 RID: 12626
	[PublicizedFrom(EAccessModifier.Private)]
	public string data;

	// Token: 0x04003153 RID: 12627
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformLobbyId platformLobbyId;

	// Token: 0x04003154 RID: 12628
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> platformUserAndToken;

	// Token: 0x04003155 RID: 12629
	[TupleElementNames(new string[] { "userId", "token" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<PlatformUserIdentifierAbs, string> crossplatformUserAndToken;

	// Token: 0x04003156 RID: 12630
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;
}
