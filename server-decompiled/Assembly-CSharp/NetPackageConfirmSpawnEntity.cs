using System;
using UnityEngine.Scripting;

// Token: 0x020007C2 RID: 1986
[Preserve]
public class NetPackageConfirmSpawnEntity : NetPackage
{
	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x06003B0A RID: 15114 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B0B RID: 15115 RVA: 0x0017D56A File Offset: 0x0017B76A
	public NetPackageConfirmSpawnEntity Setup(int _createdEntityId, Guid _key)
	{
		this.createdEntityId = _createdEntityId;
		this.key = _key;
		return this;
	}

	// Token: 0x06003B0C RID: 15116 RVA: 0x0017D57C File Offset: 0x0017B77C
	public override void read(PooledBinaryReader _reader)
	{
		this.createdEntityId = (int)_reader.ReadInt64();
		byte[] array = new byte[16];
		_reader.Read(array, 0, 16);
		this.key = new Guid(array);
	}

	// Token: 0x06003B0D RID: 15117 RVA: 0x0017D5B5 File Offset: 0x0017B7B5
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((long)this.createdEntityId);
		_writer.Write(this.key.ToByteArray());
	}

	// Token: 0x06003B0E RID: 15118 RVA: 0x0017D5DC File Offset: 0x0017B7DC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world != null)
		{
			EntityPlayerLocal primaryPlayer = _world.GetPrimaryPlayer();
			if (primaryPlayer != null)
			{
				Entity entity = _world.GetEntity(this.createdEntityId);
				if (entity != null)
				{
					primaryPlayer.HandleRequestedEntitySpawn(this.key, entity);
				}
			}
		}
	}

	// Token: 0x04003004 RID: 12292
	[PublicizedFrom(EAccessModifier.Private)]
	public int createdEntityId;

	// Token: 0x04003005 RID: 12293
	[PublicizedFrom(EAccessModifier.Private)]
	public Guid key;
}
