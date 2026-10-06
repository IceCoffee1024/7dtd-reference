using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200086A RID: 2154
[Preserve]
public class NetPackageWaterSet : NetPackage
{
	// Token: 0x170006A1 RID: 1697
	// (get) Token: 0x06003E6B RID: 15979 RVA: 0x0018B937 File Offset: 0x00189B37
	public bool HasChanges
	{
		get
		{
			return this.changes.Count > 0;
		}
	}

	// Token: 0x06003E6C RID: 15980 RVA: 0x0018B947 File Offset: 0x00189B47
	public void SetSenderId(int _entityId)
	{
		this.senderEntityId = _entityId;
	}

	// Token: 0x06003E6D RID: 15981 RVA: 0x0018B950 File Offset: 0x00189B50
	public void Reset()
	{
		this.senderEntityId = -1;
		this.changes.Clear();
	}

	// Token: 0x06003E6E RID: 15982 RVA: 0x0018B964 File Offset: 0x00189B64
	public void AddChange(int _worldX, int _worldY, int _worldZ, WaterValue _data)
	{
		this.AddChange(new Vector3i(_worldX, _worldY, _worldZ), _data);
	}

	// Token: 0x06003E6F RID: 15983 RVA: 0x0018B978 File Offset: 0x00189B78
	public void AddChange(Vector3i _worldPos, WaterValue _data)
	{
		NetPackageWaterSet.WaterSetInfo waterSetInfo = new NetPackageWaterSet.WaterSetInfo
		{
			worldPos = _worldPos,
			waterData = _data
		};
		this.changes.Add(waterSetInfo);
	}

	// Token: 0x06003E70 RID: 15984 RVA: 0x0018B9AC File Offset: 0x00189BAC
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

	// Token: 0x06003E71 RID: 15985 RVA: 0x0018BA00 File Offset: 0x00189C00
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

	// Token: 0x06003E72 RID: 15986 RVA: 0x0018BA60 File Offset: 0x00189C60
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

	// Token: 0x06003E73 RID: 15987 RVA: 0x0018BAD0 File Offset: 0x00189CD0
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

	// Token: 0x040032AB RID: 12971
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderEntityId = -1;

	// Token: 0x040032AC RID: 12972
	[PublicizedFrom(EAccessModifier.Private)]
	public List<NetPackageWaterSet.WaterSetInfo> changes = new List<NetPackageWaterSet.WaterSetInfo>();

	// Token: 0x0200086B RID: 2155
	[PublicizedFrom(EAccessModifier.Private)]
	public struct WaterSetInfo
	{
		// Token: 0x06003E75 RID: 15989 RVA: 0x0018BB3C File Offset: 0x00189D3C
		public void Read(PooledBinaryReader _br)
		{
			this.worldPos = StreamUtils.ReadVector3i(_br);
			this.waterData.Read(_br);
		}

		// Token: 0x06003E76 RID: 15990 RVA: 0x0018BB56 File Offset: 0x00189D56
		public void Write(PooledBinaryWriter _bw)
		{
			StreamUtils.Write(_bw, this.worldPos);
			this.waterData.Write(_bw);
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x0018BB70 File Offset: 0x00189D70
		public static int GetLength()
		{
			return 12 + WaterValue.SerializedLength();
		}

		// Token: 0x040032AD RID: 12973
		public Vector3i worldPos;

		// Token: 0x040032AE RID: 12974
		public WaterValue waterData;
	}
}
