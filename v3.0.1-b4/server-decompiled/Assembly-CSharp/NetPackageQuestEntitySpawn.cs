using System;
using UnityEngine.Scripting;

// Token: 0x0200082B RID: 2091
[Preserve]
public class NetPackageQuestEntitySpawn : NetPackage
{
	// Token: 0x06003D53 RID: 15699 RVA: 0x00184DB4 File Offset: 0x00182FB4
	public NetPackageQuestEntitySpawn Setup(int _entityType, int _entityThatPlaced = -1)
	{
		this.entityType = _entityType;
		this.gamestageGroup = "";
		this.entityIDQuestHolder = _entityThatPlaced;
		return this;
	}

	// Token: 0x06003D54 RID: 15700 RVA: 0x00184DD0 File Offset: 0x00182FD0
	public NetPackageQuestEntitySpawn Setup(string _gamestageGroup, int _entityThatPlaced = -1)
	{
		this.entityType = -1;
		this.gamestageGroup = _gamestageGroup;
		this.entityIDQuestHolder = _entityThatPlaced;
		return this;
	}

	// Token: 0x06003D55 RID: 15701 RVA: 0x00184DE8 File Offset: 0x00182FE8
	public override void read(PooledBinaryReader _reader)
	{
		this.entityType = _reader.ReadInt32();
		this.gamestageGroup = _reader.ReadString();
		this.entityIDQuestHolder = _reader.ReadInt32();
	}

	// Token: 0x06003D56 RID: 15702 RVA: 0x00184E0E File Offset: 0x0018300E
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityType);
		_writer.Write(this.gamestageGroup);
		_writer.Write(this.entityIDQuestHolder);
	}

	// Token: 0x17000667 RID: 1639
	// (get) Token: 0x06003D57 RID: 15703 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D58 RID: 15704 RVA: 0x00184E3C File Offset: 0x0018303C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (this.entityType == -1)
		{
			EntityPlayer entityPlayer = GameManager.Instance.World.GetEntity(this.entityIDQuestHolder) as EntityPlayer;
			GameStageDefinition gameStage = GameStageDefinition.GetGameStage(this.gamestageGroup);
			this.entityType = EntityGroups.GetRandomFromGroup(gameStage.GetStage(entityPlayer.PartyGameStage).GetSpawnGroup(0).groupName, ref NetPackageQuestEntitySpawn.lastClassId, null);
		}
		QuestActionSpawnEnemy.SpawnQuestEntity(this.entityType, this.entityIDQuestHolder, null);
	}

	// Token: 0x06003D59 RID: 15705 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x04003170 RID: 12656
	public int entityType = -1;

	// Token: 0x04003171 RID: 12657
	public string gamestageGroup;

	// Token: 0x04003172 RID: 12658
	public ItemValue itemValue;

	// Token: 0x04003173 RID: 12659
	public int entityIDQuestHolder;

	// Token: 0x04003174 RID: 12660
	[PublicizedFrom(EAccessModifier.Private)]
	public static int lastClassId = -1;
}
