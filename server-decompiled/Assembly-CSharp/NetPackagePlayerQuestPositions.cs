using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200082E RID: 2094
[Preserve]
public class NetPackagePlayerQuestPositions : NetPackage
{
	// Token: 0x1700068B RID: 1675
	// (get) Token: 0x06003D39 RID: 15673 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D3A RID: 15674 RVA: 0x00185F0D File Offset: 0x0018410D
	public NetPackagePlayerQuestPositions Setup(int entityId, PersistentPlayerData ppd)
	{
		this.entityId = entityId;
		this.questPositions = new List<QuestPositionData>(ppd.QuestPositions);
		return this;
	}

	// Token: 0x06003D3B RID: 15675 RVA: 0x00185F28 File Offset: 0x00184128
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.questPositions = new List<QuestPositionData>();
		int num = _reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.questPositions.Add(QuestPositionData.Read(_reader));
		}
	}

	// Token: 0x06003D3C RID: 15676 RVA: 0x00185F70 File Offset: 0x00184170
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write(this.questPositions.Count);
		foreach (QuestPositionData questPositionData in this.questPositions)
		{
			questPositionData.Write(_writer);
		}
	}

	// Token: 0x06003D3D RID: 15677 RVA: 0x00185FE8 File Offset: 0x001841E8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!base.ValidEntityIdForSender(this.entityId, false))
		{
			return;
		}
		PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(this.entityId);
		if (playerDataFromEntityID != null)
		{
			playerDataFromEntityID.QuestPositions.Clear();
			playerDataFromEntityID.QuestPositions.AddRange(this.questPositions);
		}
	}

	// Token: 0x040031BD RID: 12733
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040031BE RID: 12734
	[PublicizedFrom(EAccessModifier.Private)]
	public List<QuestPositionData> questPositions;
}
