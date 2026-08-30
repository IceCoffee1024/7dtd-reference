using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x0200085B RID: 2139
[Preserve]
public class NetPackageWaterSet : NetPackage
{
	// Token: 0x17000676 RID: 1654
	// (get) Token: 0x06003E6D RID: 15981 RVA: 0x001899DB File Offset: 0x00187BDB
	public bool HasChanges
	{
		get
		{
			return this.changes.Count > 0;
		}
	}

	// Token: 0x06003E6E RID: 15982 RVA: 0x001899EB File Offset: 0x00187BEB
	public void SetSenderId(int _entityId)
	{
		this.senderEntityId = _entityId;
	}

	// Token: 0x06003E6F RID: 15983 RVA: 0x001899F4 File Offset: 0x00187BF4
	public void Reset()
	{
		this.senderEntityId = -1;
		this.changes.Clear();
	}

	// Token: 0x06003E70 RID: 15984 RVA: 0x00189A08 File Offset: 0x00187C08
	public void AddChange(int _worldX, int _worldY, int _worldZ, WaterValue _data)
	{
		this.AddChange(new Vector3i(_worldX, _worldY, _worldZ), _data);
	}

	// Token: 0x06003E71 RID: 15985 RVA: 0x00189A1C File Offset: 0x00187C1C
	public void AddChange(Vector3i _worldPos, WaterValue _data)
	{
		NetPackageWaterSet.WaterSetInfo waterSetInfo = new NetPackageWaterSet.WaterSetInfo
		{
			worldPos = _worldPos,
			waterData = _data
		};
		this.changes.Add(waterSetInfo);
	}

	// Token: 0x06003E72 RID: 15986 RVA: 0x00189A50 File Offset: 0x00187C50
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

	// Token: 0x06003E73 RID: 15987 RVA: 0x00189AA4 File Offset: 0x00187CA4
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

	// Token: 0x06003E74 RID: 15988 RVA: 0x00189B04 File Offset: 0x00187D04
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

	// Token: 0x06003E75 RID: 15989 RVA: 0x00189B74 File Offset: 0x00187D74
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

	// Token: 0x06003E76 RID: 15990 RVA: 0x00189BC6 File Offset: 0x00187DC6
	public override int GetLength()
	{
		return 2 + this.changes.Count * NetPackageWaterSet.WaterSetInfo.GetLength();
	}

	// Token: 0x0400323F RID: 12863
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderEntityId = -1;

	// Token: 0x04003240 RID: 12864
	[PublicizedFrom(EAccessModifier.Private)]
	public List<NetPackageWaterSet.WaterSetInfo> changes = new List<NetPackageWaterSet.WaterSetInfo>();

	// Token: 0x0200085C RID: 2140
	[PublicizedFrom(EAccessModifier.Private)]
	public struct WaterSetInfo
	{
		// Token: 0x06003E78 RID: 15992 RVA: 0x00189BF5 File Offset: 0x00187DF5
		public void Read(BinaryReader _br)
		{
			this.worldPos = StreamUtils.ReadVector3i(_br);
			this.waterData.Read(_br);
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x00189C0F File Offset: 0x00187E0F
		public void Write(BinaryWriter _bw)
		{
			StreamUtils.Write(_bw, this.worldPos);
			this.waterData.Write(_bw);
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x00189C29 File Offset: 0x00187E29
		public static int GetLength()
		{
			return 12 + WaterValue.SerializedLength();
		}

		// Token: 0x04003241 RID: 12865
		public Vector3i worldPos;

		// Token: 0x04003242 RID: 12866
		public WaterValue waterData;
	}
}
