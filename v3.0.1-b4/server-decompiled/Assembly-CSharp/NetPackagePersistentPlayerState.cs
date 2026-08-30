using System;
using UnityEngine.Scripting;

// Token: 0x020008E3 RID: 2275
[Preserve]
public class NetPackagePersistentPlayerState : NetPackage
{
	// Token: 0x170006E7 RID: 1767
	// (get) Token: 0x060041EB RID: 16875 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x060041EC RID: 16876 RVA: 0x0019BB2B File Offset: 0x00199D2B
	public NetPackagePersistentPlayerState Setup(PersistentPlayerData ppData, EnumPersistentPlayerDataReason reason)
	{
		this.m_ppData = ppData;
		this.m_reason = reason;
		return this;
	}

	// Token: 0x060041ED RID: 16877 RVA: 0x0019BB3C File Offset: 0x00199D3C
	public override void read(PooledBinaryReader _reader)
	{
		this.m_reason = (EnumPersistentPlayerDataReason)_reader.ReadByte();
		this.m_ppData = PersistentPlayerData.Read(_reader);
	}

	// Token: 0x060041EE RID: 16878 RVA: 0x0019BB56 File Offset: 0x00199D56
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.m_reason);
		this.m_ppData.Write(_writer);
	}

	// Token: 0x060041EF RID: 16879 RVA: 0x0019BB78 File Offset: 0x00199D78
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.PersistentPlayerLogin(this.m_ppData);
	}

	// Token: 0x060041F0 RID: 16880 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int GetLength()
	{
		return 1000;
	}

	// Token: 0x04003535 RID: 13621
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerData m_ppData;

	// Token: 0x04003536 RID: 13622
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumPersistentPlayerDataReason m_reason;
}
