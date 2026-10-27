using System;
using UnityEngine.Scripting;

// Token: 0x020008FE RID: 2302
[Preserve]
public class NetPackagePersistentPlayerState : NetPackage
{
	// Token: 0x17000718 RID: 1816
	// (get) Token: 0x06004206 RID: 16902 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06004207 RID: 16903 RVA: 0x0019EDAF File Offset: 0x0019CFAF
	public NetPackagePersistentPlayerState Setup(PersistentPlayerData ppData, EnumPersistentPlayerDataReason reason)
	{
		this.m_ppData = ppData;
		this.m_reason = reason;
		return this;
	}

	// Token: 0x06004208 RID: 16904 RVA: 0x0019EDC0 File Offset: 0x0019CFC0
	public override void read(PooledBinaryReader _reader)
	{
		this.m_reason = (EnumPersistentPlayerDataReason)_reader.ReadByte();
		this.m_ppData = PersistentPlayerData.Read(_reader);
	}

	// Token: 0x06004209 RID: 16905 RVA: 0x0019EDDA File Offset: 0x0019CFDA
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.m_reason);
		this.m_ppData.Write(_writer);
	}

	// Token: 0x0600420A RID: 16906 RVA: 0x0019EDFC File Offset: 0x0019CFFC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.PersistentPlayerLogin(this.m_ppData);
	}

	// Token: 0x040035CA RID: 13770
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerData m_ppData;

	// Token: 0x040035CB RID: 13771
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumPersistentPlayerDataReason m_reason;
}
