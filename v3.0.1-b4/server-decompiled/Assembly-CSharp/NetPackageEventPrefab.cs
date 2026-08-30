using System;
using UnityEngine.Scripting;

// Token: 0x020007E7 RID: 2023
[Preserve]
public class NetPackageEventPrefab : NetPackage
{
	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x06003BAB RID: 15275 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BAC RID: 15276 RVA: 0x0017E9E1 File Offset: 0x0017CBE1
	public NetPackageEventPrefab Setup(NetPackageEventPrefab.Operation _operation, PrefabInstance _pi)
	{
		this.operation = _operation;
		this.serializablePi = new PrefabInstance.Serializable(_pi);
		return this;
	}

	// Token: 0x06003BAD RID: 15277 RVA: 0x0017E9F7 File Offset: 0x0017CBF7
	public override void read(PooledBinaryReader _br)
	{
		this.operation = (NetPackageEventPrefab.Operation)_br.ReadByte();
		this.serializablePi = new PrefabInstance.Serializable(_br);
	}

	// Token: 0x06003BAE RID: 15278 RVA: 0x0017EA11 File Offset: 0x0017CC11
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.operation);
		this.serializablePi.Write(_bw);
	}

	// Token: 0x06003BAF RID: 15279 RVA: 0x0017EA34 File Offset: 0x0017CC34
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

	// Token: 0x06003BB0 RID: 15280 RVA: 0x0017EAB7 File Offset: 0x0017CCB7
	public override int GetLength()
	{
		return 1 + this.serializablePi.GetLength();
	}

	// Token: 0x0400304D RID: 12365
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEventPrefab.Operation operation;

	// Token: 0x0400304E RID: 12366
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabInstance.Serializable serializablePi;

	// Token: 0x020007E8 RID: 2024
	public enum Operation
	{
		// Token: 0x04003050 RID: 12368
		Add,
		// Token: 0x04003051 RID: 12369
		Remove
	}
}
