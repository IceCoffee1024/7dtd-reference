using System;

// Token: 0x02000586 RID: 1414
public class ItemWorldData
{
	// Token: 0x06002E92 RID: 11922 RVA: 0x00126ACA File Offset: 0x00124CCA
	public ItemWorldData(IGameManager _gm, ItemValue _itemValue, EntityItem _entityItem, int _belongsEntityId)
	{
		this.gameManager = _gm;
		this.world = _entityItem.world;
		this.entityItem = _entityItem;
		this.belongsEntityId = _belongsEntityId;
	}

	// Token: 0x040023E7 RID: 9191
	public IGameManager gameManager;

	// Token: 0x040023E8 RID: 9192
	public WorldBase world;

	// Token: 0x040023E9 RID: 9193
	public EntityItem entityItem;

	// Token: 0x040023EA RID: 9194
	public int belongsEntityId;
}
