using System;
using UnityEngine.Scripting;

// Token: 0x020007FF RID: 2047
[Preserve]
public class NetPackageEventPrefab : NetPackage
{
	// Token: 0x17000666 RID: 1638
	// (get) Token: 0x06003C30 RID: 15408 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C31 RID: 15409 RVA: 0x001823F5 File Offset: 0x001805F5
	public NetPackageEventPrefab Setup(NetPackageEventPrefab.Operation _operation, PrefabInstance _pi)
	{
		this.operation = _operation;
		this.serializablePi = new PrefabInstance.Serializable(_pi);
		return this;
	}

	// Token: 0x06003C32 RID: 15410 RVA: 0x0018240B File Offset: 0x0018060B
	public override void read(PooledBinaryReader _br)
	{
		this.operation = (NetPackageEventPrefab.Operation)_br.ReadByte();
		this.serializablePi = new PrefabInstance.Serializable(_br);
	}

	// Token: 0x06003C33 RID: 15411 RVA: 0x00182425 File Offset: 0x00180625
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.operation);
		this.serializablePi.Write(_bw);
	}

	// Token: 0x06003C34 RID: 15412 RVA: 0x00182448 File Offset: 0x00180648
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			if (!_callbacks.worldInitInfoReceived)
			{
				return;
			}
			int id = this.serializablePi.id;
			string prefabName = this.serializablePi.prefabName;
			Vector3i position = this.serializablePi.position;
			byte rotation = this.serializablePi.rotation;
			NetPackageEventPrefab.Operation operation = this.operation;
			if (operation == NetPackageEventPrefab.Operation.Add)
			{
				_world.m_EventPrefabsClient.TryAdd(id, prefabName, rotation, position);
				return;
			}
			if (operation != NetPackageEventPrefab.Operation.Remove)
			{
				return;
			}
			_world.m_EventPrefabsClient.Remove(id, prefabName, rotation, position);
		}
	}

	// Token: 0x040030E5 RID: 12517
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEventPrefab.Operation operation;

	// Token: 0x040030E6 RID: 12518
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabInstance.Serializable serializablePi;

	// Token: 0x02000800 RID: 2048
	public enum Operation
	{
		// Token: 0x040030E8 RID: 12520
		Add,
		// Token: 0x040030E9 RID: 12521
		Remove
	}
}
