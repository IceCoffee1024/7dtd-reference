using System;
using UnityEngine.Scripting;

// Token: 0x02000842 RID: 2114
[Preserve]
public class NetPackageQuestEntitySpawn : NetPackage
{
	// Token: 0x06003D91 RID: 15761 RVA: 0x00187A10 File Offset: 0x00185C10
	public NetPackageQuestEntitySpawn Setup(int _entityType, int _entityThatPlaced = -1)
	{
		this.entityType = _entityType;
		this.gamestageGroup = "";
		this.entityIDQuestHolder = _entityThatPlaced;
		return this;
	}

	// Token: 0x06003D92 RID: 15762 RVA: 0x00187A2C File Offset: 0x00185C2C
	public NetPackageQuestEntitySpawn Setup(string _gamestageGroup, int _entityThatPlaced = -1)
	{
		this.entityType = -1;
		this.gamestageGroup = _gamestageGroup;
		this.entityIDQuestHolder = _entityThatPlaced;
		return this;
	}

	// Token: 0x06003D93 RID: 15763 RVA: 0x00187A44 File Offset: 0x00185C44
	public override void read(PooledBinaryReader _reader)
	{
		this.entityType = _reader.ReadInt32();
		this.gamestageGroup = _reader.ReadString();
		this.entityIDQuestHolder = _reader.ReadInt32();
	}

	// Token: 0x06003D94 RID: 15764 RVA: 0x00187A6A File Offset: 0x00185C6A
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityType);
		_writer.Write(this.gamestageGroup);
		_writer.Write(this.entityIDQuestHolder);
	}

	// Token: 0x17000698 RID: 1688
	// (get) Token: 0x06003D95 RID: 15765 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D96 RID: 15766 RVA: 0x00187A98 File Offset: 0x00185C98
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

	// Token: 0x040031F6 RID: 12790
	public int entityType = -1;

	// Token: 0x040031F7 RID: 12791
	public string gamestageGroup;

	// Token: 0x040031F8 RID: 12792
	public ItemValue itemValue;

	// Token: 0x040031F9 RID: 12793
	public int entityIDQuestHolder;

	// Token: 0x040031FA RID: 12794
	[PublicizedFrom(EAccessModifier.Private)]
	public static int lastClassId = -1;
}
