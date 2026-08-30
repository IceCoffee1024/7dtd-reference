using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000867 RID: 2151
[Preserve]
public class NetPackageWaterSet : NetPackage
{
	// Token: 0x17000682 RID: 1666
	// (get) Token: 0x06003ED2 RID: 16082 RVA: 0x0018BAAB File Offset: 0x00189CAB
	public bool HasChanges
	{
		get
		{
			return this.changes.Count > 0;
		}
	}

	// Token: 0x06003ED3 RID: 16083 RVA: 0x0018BABB File Offset: 0x00189CBB
	public void SetSenderId(int _entityId)
	{
		this.senderEntityId = _entityId;
	}

	// Token: 0x06003ED4 RID: 16084 RVA: 0x0018BAC4 File Offset: 0x00189CC4
	public void Reset()
	{
		this.senderEntityId = -1;
		this.changes.Clear();
	}

	// Token: 0x06003ED5 RID: 16085 RVA: 0x0018BAD8 File Offset: 0x00189CD8
	public void AddChange(int _worldX, int _worldY, int _worldZ, WaterValue _data)
	{
		this.AddChange(new Vector3i(_worldX, _worldY, _worldZ), _data);
	}

	// Token: 0x06003ED6 RID: 16086 RVA: 0x0018BAEC File Offset: 0x00189CEC
	public void AddChange(Vector3i _worldPos, WaterValue _data)
	{
		NetPackageWaterSet.WaterSetInfo waterSetInfo = new NetPackageWaterSet.WaterSetInfo
		{
			worldPos = _worldPos,
			waterData = _data
		};
		this.changes.Add(waterSetInfo);
	}

	// Token: 0x06003ED7 RID: 16087 RVA: 0x0018BB20 File Offset: 0x00189D20
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

	// Token: 0x06003ED8 RID: 16088 RVA: 0x0018BB74 File Offset: 0x00189D74
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

	// Token: 0x06003ED9 RID: 16089 RVA: 0x0018BBD4 File Offset: 0x00189DD4
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

	// Token: 0x06003EDA RID: 16090 RVA: 0x0018BC44 File Offset: 0x00189E44
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

	// Token: 0x06003EDB RID: 16091 RVA: 0x0018BC96 File Offset: 0x00189E96
	public override int GetLength()
	{
		return 2 + this.changes.Count * NetPackageWaterSet.WaterSetInfo.GetLength();
	}

	// Token: 0x040032C0 RID: 12992
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderEntityId = -1;

	// Token: 0x040032C1 RID: 12993
	[PublicizedFrom(EAccessModifier.Private)]
	public List<NetPackageWaterSet.WaterSetInfo> changes = new List<NetPackageWaterSet.WaterSetInfo>();

	// Token: 0x02000868 RID: 2152
	[PublicizedFrom(EAccessModifier.Private)]
	public struct WaterSetInfo
	{
		// Token: 0x06003EDD RID: 16093 RVA: 0x0018BCC5 File Offset: 0x00189EC5
		public void Read(BinaryReader _br)
		{
			this.worldPos = StreamUtils.ReadVector3i(_br);
			this.waterData.Read(_br);
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x0018BCDF File Offset: 0x00189EDF
		public void Write(BinaryWriter _bw)
		{
			StreamUtils.Write(_bw, this.worldPos);
			this.waterData.Write(_bw);
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x0018BCF9 File Offset: 0x00189EF9
		public static int GetLength()
		{
			return 12 + WaterValue.SerializedLength();
		}

		// Token: 0x040032C2 RID: 12994
		public Vector3i worldPos;

		// Token: 0x040032C3 RID: 12995
		public WaterValue waterData;
	}
}
