using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200082D RID: 2093
[Preserve]
public class NetPackagePlayerQuestPositions : NetPackage
{
	// Token: 0x1700066C RID: 1644
	// (get) Token: 0x06003D7A RID: 15738 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D7B RID: 15739 RVA: 0x001863D5 File Offset: 0x001845D5
	public NetPackagePlayerQuestPositions Setup(int entityId, PersistentPlayerData ppd)
	{
		this.entityId = entityId;
		this.questPositions = new List<QuestPositionData>(ppd.QuestPositions);
		return this;
	}

	// Token: 0x06003D7C RID: 15740 RVA: 0x001863F0 File Offset: 0x001845F0
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

	// Token: 0x06003D7D RID: 15741 RVA: 0x00186438 File Offset: 0x00184638
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

	// Token: 0x06003D7E RID: 15742 RVA: 0x001864B0 File Offset: 0x001846B0
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

	// Token: 0x06003D7F RID: 15743 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x040031D6 RID: 12758
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040031D7 RID: 12759
	[PublicizedFrom(EAccessModifier.Private)]
	public List<QuestPositionData> questPositions;
}
