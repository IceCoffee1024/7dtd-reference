using System;
using UnityEngine.Scripting;

// Token: 0x020008F4 RID: 2292
[Preserve]
public class NetPackagePersistentPlayerState : NetPackage
{
	// Token: 0x17000712 RID: 1810
	// (get) Token: 0x060041E3 RID: 16867 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x060041E4 RID: 16868 RVA: 0x0019DD07 File Offset: 0x0019BF07
	public NetPackagePersistentPlayerState Setup(PersistentPlayerData ppData, EnumPersistentPlayerDataReason reason)
	{
		this.m_ppData = ppData;
		this.m_reason = reason;
		return this;
	}

	// Token: 0x060041E5 RID: 16869 RVA: 0x0019DD18 File Offset: 0x0019BF18
	public override void read(PooledBinaryReader _reader)
	{
		this.m_reason = (EnumPersistentPlayerDataReason)_reader.ReadByte();
		this.m_ppData = PersistentPlayerData.Read(_reader);
	}

	// Token: 0x060041E6 RID: 16870 RVA: 0x0019DD32 File Offset: 0x0019BF32
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.m_reason);
		this.m_ppData.Write(_writer);
	}

	// Token: 0x060041E7 RID: 16871 RVA: 0x0019DD54 File Offset: 0x0019BF54
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.PersistentPlayerLogin(this.m_ppData);
	}

	// Token: 0x040035AA RID: 13738
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerData m_ppData;

	// Token: 0x040035AB RID: 13739
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumPersistentPlayerDataReason m_reason;
}
