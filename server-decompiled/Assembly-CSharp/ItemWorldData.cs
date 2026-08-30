using System;

// Token: 0x02000582 RID: 1410
public class ItemWorldData
{
	// Token: 0x06002E3B RID: 11835 RVA: 0x001259CA File Offset: 0x00123BCA
	public ItemWorldData(IGameManager _gm, ItemValue _itemValue, EntityItem _entityItem, int _belongsEntityId)
	{
		this.gameManager = _gm;
		this.world = _entityItem.world;
		this.entityItem = _entityItem;
		this.belongsEntityId = _belongsEntityId;
	}

	// Token: 0x040023E5 RID: 9189
	public IGameManager gameManager;

	// Token: 0x040023E6 RID: 9190
	public WorldBase world;

	// Token: 0x040023E7 RID: 9191
	public EntityItem entityItem;

	// Token: 0x040023E8 RID: 9192
	public int belongsEntityId;
}
