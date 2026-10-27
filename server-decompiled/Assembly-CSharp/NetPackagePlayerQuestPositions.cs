using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000838 RID: 2104
[Preserve]
public class NetPackagePlayerQuestPositions : NetPackage
{
	// Token: 0x17000691 RID: 1681
	// (get) Token: 0x06003D5C RID: 15708 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D5D RID: 15709 RVA: 0x00187011 File Offset: 0x00185211
	public NetPackagePlayerQuestPositions Setup(int entityId, PersistentPlayerData ppd)
	{
		this.entityId = entityId;
		this.questPositions = new List<QuestPositionData>(ppd.QuestPositions);
		return this;
	}

	// Token: 0x06003D5E RID: 15710 RVA: 0x0018702C File Offset: 0x0018522C
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

	// Token: 0x06003D5F RID: 15711 RVA: 0x00187074 File Offset: 0x00185274
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

	// Token: 0x06003D60 RID: 15712 RVA: 0x001870EC File Offset: 0x001852EC
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

	// Token: 0x040031DD RID: 12765
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040031DE RID: 12766
	[PublicizedFrom(EAccessModifier.Private)]
	public List<QuestPositionData> questPositions;
}
