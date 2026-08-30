using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000822 RID: 2082
[Preserve]
public class NetPackagePlayerQuestPositions : NetPackage
{
	// Token: 0x17000661 RID: 1633
	// (get) Token: 0x06003D1B RID: 15643 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D1C RID: 15644 RVA: 0x00183FFD File Offset: 0x001821FD
	public NetPackagePlayerQuestPositions Setup(int entityId, PersistentPlayerData ppd)
	{
		this.entityId = entityId;
		this.questPositions = new List<QuestPositionData>(ppd.QuestPositions);
		return this;
	}

	// Token: 0x06003D1D RID: 15645 RVA: 0x00184018 File Offset: 0x00182218
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

	// Token: 0x06003D1E RID: 15646 RVA: 0x00184060 File Offset: 0x00182260
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

	// Token: 0x06003D1F RID: 15647 RVA: 0x001840D8 File Offset: 0x001822D8
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

	// Token: 0x06003D20 RID: 15648 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04003157 RID: 12631
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003158 RID: 12632
	[PublicizedFrom(EAccessModifier.Private)]
	public List<QuestPositionData> questPositions;
}
