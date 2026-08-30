using System;

// Token: 0x0200057D RID: 1405
public class ItemWorldData
{
	// Token: 0x06002E0D RID: 11789 RVA: 0x001248DA File Offset: 0x00122ADA
	public ItemWorldData(IGameManager _gm, ItemValue _itemValue, EntityItem _entityItem, int _belongsEntityId)
	{
		this.gameManager = _gm;
		this.world = _entityItem.world;
		this.entityItem = _entityItem;
		this.belongsEntityId = _belongsEntityId;
	}

	// Token: 0x040023B1 RID: 9137
	public IGameManager gameManager;

	// Token: 0x040023B2 RID: 9138
	public WorldBase world;

	// Token: 0x040023B3 RID: 9139
	public EntityItem entityItem;

	// Token: 0x040023B4 RID: 9140
	public int belongsEntityId;
}
