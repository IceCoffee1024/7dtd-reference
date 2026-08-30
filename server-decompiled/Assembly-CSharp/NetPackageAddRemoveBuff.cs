using System;
using UnityEngine.Scripting;

// Token: 0x020001D3 RID: 467
[Preserve]
public class NetPackageAddRemoveBuff : NetPackage
{
	// Token: 0x06000E7E RID: 3710 RVA: 0x0005F71C File Offset: 0x0005D91C
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

	// Token: 0x06000E7F RID: 3711 RVA: 0x0005F74C File Offset: 0x0005D94C
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.buffName = _reader.ReadString();
		this.duration = _reader.ReadSingle();
		this.adding = _reader.ReadBoolean();
		this.instigatorId = _reader.ReadInt32();
		this.instigatorPos = StreamUtils.ReadVector3i(_reader);
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x0005F7A4 File Offset: 0x0005D9A4
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

	// Token: 0x06000E81 RID: 3713 RVA: 0x0005F800 File Offset: 0x0005DA00
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

	// Token: 0x06000E82 RID: 3714 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04000C58 RID: 3160
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04000C59 RID: 3161
	[PublicizedFrom(EAccessModifier.Private)]
	public int instigatorId;

	// Token: 0x04000C5A RID: 3162
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName;

	// Token: 0x04000C5B RID: 3163
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration;

	// Token: 0x04000C5C RID: 3164
	[PublicizedFrom(EAccessModifier.Private)]
	public bool adding;

	// Token: 0x04000C5D RID: 3165
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i instigatorPos;
}
