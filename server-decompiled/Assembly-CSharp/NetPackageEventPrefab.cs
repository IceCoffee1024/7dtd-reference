using System;
using UnityEngine.Scripting;

// Token: 0x020007F5 RID: 2037
[Preserve]
public class NetPackageEventPrefab : NetPackage
{
	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x06003C0D RID: 15373 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C0E RID: 15374 RVA: 0x001812F1 File Offset: 0x0017F4F1
	public NetPackageEventPrefab Setup(NetPackageEventPrefab.Operation _operation, PrefabInstance _pi)
	{
		this.operation = _operation;
		this.serializablePi = new PrefabInstance.Serializable(_pi);
		return this;
	}

	// Token: 0x06003C0F RID: 15375 RVA: 0x00181307 File Offset: 0x0017F507
	public override void read(PooledBinaryReader _br)
	{
		this.operation = (NetPackageEventPrefab.Operation)_br.ReadByte();
		this.serializablePi = new PrefabInstance.Serializable(_br);
	}

	// Token: 0x06003C10 RID: 15376 RVA: 0x00181321 File Offset: 0x0017F521
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.operation);
		this.serializablePi.Write(_bw);
	}

	// Token: 0x06003C11 RID: 15377 RVA: 0x00181344 File Offset: 0x0017F544
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

	// Token: 0x040030C5 RID: 12485
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEventPrefab.Operation operation;

	// Token: 0x040030C6 RID: 12486
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabInstance.Serializable serializablePi;

	// Token: 0x020007F6 RID: 2038
	public enum Operation
	{
		// Token: 0x040030C8 RID: 12488
		Add,
		// Token: 0x040030C9 RID: 12489
		Remove
	}
}
