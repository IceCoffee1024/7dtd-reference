using System;
using UnityEngine.Scripting;

// Token: 0x020001D6 RID: 470
[Preserve]
public class NetPackageAddRemoveBuff : NetPackage
{
	// Token: 0x06000E77 RID: 3703 RVA: 0x0005F9D0 File Offset: 0x0005DBD0
	public NetPackageAddRemoveBuff Setup(int _entityId, string _buffName, float _duration, bool _adding, int _instigatorId, Vector3i _instigatorPos)
	{
		this.entityId = _entityId;
		this.buffName = _buffName;
		this.adding = _adding;
		this.instigatorId = _instigatorId;
		this.duration = _duration;
		this.instigatorPos = _instigatorPos;
		return this;
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x0005FA00 File Offset: 0x0005DC00
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.buffName = _reader.ReadString();
		this.duration = _reader.ReadSingle();
		this.adding = _reader.ReadBoolean();
		this.instigatorId = _reader.ReadInt32();
		this.instigatorPos = StreamUtils.ReadVector3i(_reader);
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x0005FA58 File Offset: 0x0005DC58
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write(this.buffName);
		_writer.Write(this.duration);
		_writer.Write(this.adding);
		_writer.Write(this.instigatorId);
		StreamUtils.Write(_writer, this.instigatorPos);
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x0005FAB4 File Offset: 0x0005DCB4
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			ConnectionManager instance = SingletonMonoBehaviour<ConnectionManager>.Instance;
			NetPackage netPackage = NetPackageManager.GetPackage<NetPackageAddRemoveBuff>().Setup(this.entityId, this.buffName, this.duration, this.adding, this.instigatorId, this.instigatorPos);
			bool flag = false;
			int num = -1;
			int num2 = this.entityId;
			instance.SendPackage(netPackage, flag, num, this.instigatorId, num2, null, 192, false);
		}
		EntityAlive entityAlive = _world.GetEntity(this.entityId) as EntityAlive;
		if (entityAlive != null)
		{
			if (this.adding)
			{
				entityAlive.Buffs.AddBuff(this.buffName, this.instigatorPos, this.instigatorId, false, false, this.duration);
				return;
			}
			entityAlive.Buffs.RemoveBuff(this.buffName, this.instigatorId, false);
		}
	}

	// Token: 0x04000C53 RID: 3155
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04000C54 RID: 3156
	[PublicizedFrom(EAccessModifier.Private)]
	public int instigatorId;

	// Token: 0x04000C55 RID: 3157
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName;

	// Token: 0x04000C56 RID: 3158
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration;

	// Token: 0x04000C57 RID: 3159
	[PublicizedFrom(EAccessModifier.Private)]
	public bool adding;

	// Token: 0x04000C58 RID: 3160
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i instigatorPos;
}
