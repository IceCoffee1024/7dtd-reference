using System;
using UnityEngine.Scripting;

// Token: 0x020007CA RID: 1994
[Preserve]
public class NetPackageConfirmSpawnEntity : NetPackage
{
	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x06003B21 RID: 15137 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B22 RID: 15138 RVA: 0x0017E502 File Offset: 0x0017C702
	public NetPackageConfirmSpawnEntity Setup(int _createdEntityId, Guid _key)
	{
		this.createdEntityId = _createdEntityId;
		this.key = _key;
		return this;
	}

	// Token: 0x06003B23 RID: 15139 RVA: 0x0017E514 File Offset: 0x0017C714
	public override void read(PooledBinaryReader _reader)
	{
		this.createdEntityId = (int)_reader.ReadInt64();
		byte[] array = new byte[16];
		_reader.Read(array, 0, 16);
		this.key = new Guid(array);
	}

	// Token: 0x06003B24 RID: 15140 RVA: 0x0017E54D File Offset: 0x0017C74D
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((long)this.createdEntityId);
		_writer.Write(this.key.ToByteArray());
	}

	// Token: 0x06003B25 RID: 15141 RVA: 0x0017E574 File Offset: 0x0017C774
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

	// Token: 0x04003020 RID: 12320
	[PublicizedFrom(EAccessModifier.Private)]
	public int createdEntityId;

	// Token: 0x04003021 RID: 12321
	[PublicizedFrom(EAccessModifier.Private)]
	public Guid key;
}
