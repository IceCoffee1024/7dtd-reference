using System;
using UnityEngine.Scripting;

// Token: 0x02000837 RID: 2103
[Preserve]
public class NetPackageQuestEntitySpawn : NetPackage
{
	// Token: 0x06003DB8 RID: 15800 RVA: 0x00186DD4 File Offset: 0x00184FD4
	public NetPackageQuestEntitySpawn Setup(int _entityType, int _entityThatPlaced = -1)
	{
		this.entityType = _entityType;
		this.gamestageGroup = "";
		this.entityIDQuestHolder = _entityThatPlaced;
		return this;
	}

	// Token: 0x06003DB9 RID: 15801 RVA: 0x00186DF0 File Offset: 0x00184FF0
	public NetPackageQuestEntitySpawn Setup(string _gamestageGroup, int _entityThatPlaced = -1)
	{
		this.entityType = -1;
		this.gamestageGroup = _gamestageGroup;
		this.entityIDQuestHolder = _entityThatPlaced;
		return this;
	}

	// Token: 0x06003DBA RID: 15802 RVA: 0x00186E08 File Offset: 0x00185008
	public override void read(PooledBinaryReader _reader)
	{
		this.entityType = _reader.ReadInt32();
		this.gamestageGroup = _reader.ReadString();
		this.entityIDQuestHolder = _reader.ReadInt32();
	}

	// Token: 0x06003DBB RID: 15803 RVA: 0x00186E2E File Offset: 0x0018502E
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityType);
		_writer.Write(this.gamestageGroup);
		_writer.Write(this.entityIDQuestHolder);
	}

	// Token: 0x17000673 RID: 1651
	// (get) Token: 0x06003DBC RID: 15804 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003DBD RID: 15805 RVA: 0x00186E5C File Offset: 0x0018505C
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

	// Token: 0x06003DBE RID: 15806 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x040031EF RID: 12783
	public int entityType = -1;

	// Token: 0x040031F0 RID: 12784
	public string gamestageGroup;

	// Token: 0x040031F1 RID: 12785
	public ItemValue itemValue;

	// Token: 0x040031F2 RID: 12786
	public int entityIDQuestHolder;

	// Token: 0x040031F3 RID: 12787
	[PublicizedFrom(EAccessModifier.Private)]
	public static int lastClassId = -1;
}
