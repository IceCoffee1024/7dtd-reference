using System;
using UnityEngine.Scripting;

// Token: 0x02000838 RID: 2104
[Preserve]
public class NetPackageQuestEntitySpawn : NetPackage
{
	// Token: 0x06003D6E RID: 15726 RVA: 0x0018690C File Offset: 0x00184B0C
	public NetPackageQuestEntitySpawn Setup(int _entityType, int _entityThatPlaced = -1)
	{
		this.entityType = _entityType;
		this.gamestageGroup = "";
		this.entityIDQuestHolder = _entityThatPlaced;
		return this;
	}

	// Token: 0x06003D6F RID: 15727 RVA: 0x00186928 File Offset: 0x00184B28
	public NetPackageQuestEntitySpawn Setup(string _gamestageGroup, int _entityThatPlaced = -1)
	{
		this.entityType = -1;
		this.gamestageGroup = _gamestageGroup;
		this.entityIDQuestHolder = _entityThatPlaced;
		return this;
	}

	// Token: 0x06003D70 RID: 15728 RVA: 0x00186940 File Offset: 0x00184B40
	public override void read(PooledBinaryReader _reader)
	{
		this.entityType = _reader.ReadInt32();
		this.gamestageGroup = _reader.ReadString();
		this.entityIDQuestHolder = _reader.ReadInt32();
	}

	// Token: 0x06003D71 RID: 15729 RVA: 0x00186966 File Offset: 0x00184B66
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityType);
		_writer.Write(this.gamestageGroup);
		_writer.Write(this.entityIDQuestHolder);
	}

	// Token: 0x17000692 RID: 1682
	// (get) Token: 0x06003D72 RID: 15730 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D73 RID: 15731 RVA: 0x00186994 File Offset: 0x00184B94
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

	// Token: 0x040031D6 RID: 12758
	public int entityType = -1;

	// Token: 0x040031D7 RID: 12759
	public string gamestageGroup;

	// Token: 0x040031D8 RID: 12760
	public ItemValue itemValue;

	// Token: 0x040031D9 RID: 12761
	public int entityIDQuestHolder;

	// Token: 0x040031DA RID: 12762
	[PublicizedFrom(EAccessModifier.Private)]
	public static int lastClassId = -1;
}
