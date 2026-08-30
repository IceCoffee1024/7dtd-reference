using System;
using UnityEngine.Scripting;

// Token: 0x020007BF RID: 1983
[Preserve]
public class NetPackageConfirmSpawnEntity : NetPackage
{
	// Token: 0x1700061C RID: 1564
	// (get) Token: 0x06003ADF RID: 15071 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AE0 RID: 15072 RVA: 0x0017CF4D File Offset: 0x0017B14D
	public NetPackageConfirmSpawnEntity Setup(int _createdEntityId, Guid _key)
	{
		this.createdEntityId = _createdEntityId;
		this.key = _key;
		return this;
	}

	// Token: 0x06003AE1 RID: 15073 RVA: 0x0017CF60 File Offset: 0x0017B160
	public override void read(PooledBinaryReader _reader)
	{
		this.createdEntityId = (int)_reader.ReadInt64();
		byte[] array = new byte[16];
		_reader.Read(array, 0, 16);
		this.key = new Guid(array);
	}

	// Token: 0x06003AE2 RID: 15074 RVA: 0x0017CF99 File Offset: 0x0017B199
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((long)this.createdEntityId);
		_writer.Write(this.key.ToByteArray());
	}

	// Token: 0x06003AE3 RID: 15075 RVA: 0x0017CFC0 File Offset: 0x0017B1C0
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

	// Token: 0x06003AE4 RID: 15076 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x0400300A RID: 12298
	[PublicizedFrom(EAccessModifier.Private)]
	public int createdEntityId;

	// Token: 0x0400300B RID: 12299
	[PublicizedFrom(EAccessModifier.Private)]
	public Guid key;
}
