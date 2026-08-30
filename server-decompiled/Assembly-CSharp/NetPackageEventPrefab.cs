using System;
using UnityEngine.Scripting;

// Token: 0x020007F2 RID: 2034
[Preserve]
public class NetPackageEventPrefab : NetPackage
{
	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x06003C0A RID: 15370 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C0B RID: 15371 RVA: 0x00180DB9 File Offset: 0x0017EFB9
	public NetPackageEventPrefab Setup(NetPackageEventPrefab.Operation _operation, PrefabInstance _pi)
	{
		this.operation = _operation;
		this.serializablePi = new PrefabInstance.Serializable(_pi);
		return this;
	}

	// Token: 0x06003C0C RID: 15372 RVA: 0x00180DCF File Offset: 0x0017EFCF
	public override void read(PooledBinaryReader _br)
	{
		this.operation = (NetPackageEventPrefab.Operation)_br.ReadByte();
		this.serializablePi = new PrefabInstance.Serializable(_br);
	}

	// Token: 0x06003C0D RID: 15373 RVA: 0x00180DE9 File Offset: 0x0017EFE9
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.operation);
		this.serializablePi.Write(_bw);
	}

	// Token: 0x06003C0E RID: 15374 RVA: 0x00180E0C File Offset: 0x0017F00C
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

	// Token: 0x06003C0F RID: 15375 RVA: 0x00180E8F File Offset: 0x0017F08F
	public override int GetLength()
	{
		return 1 + this.serializablePi.GetLength();
	}

	// Token: 0x040030CC RID: 12492
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEventPrefab.Operation operation;

	// Token: 0x040030CD RID: 12493
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabInstance.Serializable serializablePi;

	// Token: 0x020007F3 RID: 2035
	public enum Operation
	{
		// Token: 0x040030CF RID: 12495
		Add,
		// Token: 0x040030D0 RID: 12496
		Remove
	}
}
