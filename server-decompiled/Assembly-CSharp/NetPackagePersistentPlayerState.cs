using System;
using UnityEngine.Scripting;

// Token: 0x020008EF RID: 2287
[Preserve]
public class NetPackagePersistentPlayerState : NetPackage
{
	// Token: 0x170006F3 RID: 1779
	// (get) Token: 0x06004250 RID: 16976 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06004251 RID: 16977 RVA: 0x0019DC03 File Offset: 0x0019BE03
	public NetPackagePersistentPlayerState Setup(PersistentPlayerData ppData, EnumPersistentPlayerDataReason reason)
	{
		this.m_ppData = ppData;
		this.m_reason = reason;
		return this;
	}

	// Token: 0x06004252 RID: 16978 RVA: 0x0019DC14 File Offset: 0x0019BE14
	public override void read(PooledBinaryReader _reader)
	{
		this.m_reason = (EnumPersistentPlayerDataReason)_reader.ReadByte();
		this.m_ppData = PersistentPlayerData.Read(_reader);
	}

	// Token: 0x06004253 RID: 16979 RVA: 0x0019DC2E File Offset: 0x0019BE2E
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.m_reason);
		this.m_ppData.Write(_writer);
	}

	// Token: 0x06004254 RID: 16980 RVA: 0x0019DC50 File Offset: 0x0019BE50
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.PersistentPlayerLogin(this.m_ppData);
	}

	// Token: 0x06004255 RID: 16981 RVA: 0x00061652 File Offset: 0x0005F852
	public override int GetLength()
	{
		return 1000;
	}

	// Token: 0x040035B6 RID: 13750
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerData m_ppData;

	// Token: 0x040035B7 RID: 13751
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumPersistentPlayerDataReason m_reason;
}
