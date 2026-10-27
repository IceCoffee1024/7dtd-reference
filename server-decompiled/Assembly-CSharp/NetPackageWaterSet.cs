using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000874 RID: 2164
[Preserve]
public class NetPackageWaterSet : NetPackage
{
	// Token: 0x170006A7 RID: 1703
	// (get) Token: 0x06003E8E RID: 16014 RVA: 0x0018C9DF File Offset: 0x0018ABDF
	public bool HasChanges
	{
		get
		{
			return this.changes.Count > 0;
		}
	}

	// Token: 0x06003E8F RID: 16015 RVA: 0x0018C9EF File Offset: 0x0018ABEF
	public void SetSenderId(int _entityId)
	{
		this.senderEntityId = _entityId;
	}

	// Token: 0x06003E90 RID: 16016 RVA: 0x0018C9F8 File Offset: 0x0018ABF8
	public void Reset()
	{
		this.senderEntityId = -1;
		this.changes.Clear();
	}

	// Token: 0x06003E91 RID: 16017 RVA: 0x0018CA0C File Offset: 0x0018AC0C
	public void AddChange(int _worldX, int _worldY, int _worldZ, WaterValue _data)
	{
		this.AddChange(new Vector3i(_worldX, _worldY, _worldZ), _data);
	}

	// Token: 0x06003E92 RID: 16018 RVA: 0x0018CA20 File Offset: 0x0018AC20
	public void AddChange(Vector3i _worldPos, WaterValue _data)
	{
		NetPackageWaterSet.WaterSetInfo waterSetInfo = new NetPackageWaterSet.WaterSetInfo
		{
			worldPos = _worldPos,
			waterData = _data
		};
		this.changes.Add(waterSetInfo);
	}

	// Token: 0x06003E93 RID: 16019 RVA: 0x0018CA54 File Offset: 0x0018AC54
	public override void read(PooledBinaryReader _br)
	{
		this.senderEntityId = _br.ReadInt32();
		this.changes.Clear();
		int num = (int)_br.ReadUInt16();
		for (int i = 0; i < num; i++)
		{
			NetPackageWaterSet.WaterSetInfo waterSetInfo = default(NetPackageWaterSet.WaterSetInfo);
			waterSetInfo.Read(_br);
			this.changes.Add(waterSetInfo);
		}
	}

	// Token: 0x06003E94 RID: 16020 RVA: 0x0018CAA8 File Offset: 0x0018ACA8
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.senderEntityId);
		int count = this.changes.Count;
		_bw.Write((ushort)count);
		for (int i = 0; i < this.changes.Count; i++)
		{
			this.changes[i].Write(_bw);
		}
	}

	// Token: 0x06003E95 RID: 16021 RVA: 0x0018CB08 File Offset: 0x0018AD08
	public void ApplyChanges(ChunkCluster _cc)
	{
		_cc.ChunkPosNeedsRegeneration_DelayedStart();
		for (int i = 0; i < this.changes.Count; i++)
		{
			NetPackageWaterSet.WaterSetInfo waterSetInfo = this.changes[i];
			_cc.SetWater(waterSetInfo.worldPos, waterSetInfo.waterData);
			GameManager.Instance.World.HandleWaterLevelChanged(waterSetInfo.worldPos, waterSetInfo.waterData.GetMassPercent());
		}
		_cc.ChunkPosNeedsRegeneration_DelayedStop();
	}

	// Token: 0x06003E96 RID: 16022 RVA: 0x0018CB78 File Offset: 0x0018AD78
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(this, false, -1, this.senderEntityId, -1, null, 192, false);
		}
		if (_world == null)
		{
			return;
		}
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		this.ApplyChanges(chunkCache);
	}

	// Token: 0x040032CB RID: 13003
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderEntityId = -1;

	// Token: 0x040032CC RID: 13004
	[PublicizedFrom(EAccessModifier.Private)]
	public List<NetPackageWaterSet.WaterSetInfo> changes = new List<NetPackageWaterSet.WaterSetInfo>();

	// Token: 0x02000875 RID: 2165
	[PublicizedFrom(EAccessModifier.Private)]
	public struct WaterSetInfo
	{
		// Token: 0x06003E98 RID: 16024 RVA: 0x0018CBE4 File Offset: 0x0018ADE4
		public void Read(PooledBinaryReader _br)
		{
			this.worldPos = StreamUtils.ReadVector3i(_br);
			this.waterData.Read(_br);
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x0018CBFE File Offset: 0x0018ADFE
		public void Write(PooledBinaryWriter _bw)
		{
			StreamUtils.Write(_bw, this.worldPos);
			this.waterData.Write(_bw);
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x0018CC18 File Offset: 0x0018AE18
		public static int GetLength()
		{
			return 12 + WaterValue.SerializedLength();
		}

		// Token: 0x040032CD RID: 13005
		public Vector3i worldPos;

		// Token: 0x040032CE RID: 13006
		public WaterValue waterData;
	}
}
