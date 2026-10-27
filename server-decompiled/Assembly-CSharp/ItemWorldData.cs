using System;

// Token: 0x0200058D RID: 1421
public class ItemWorldData
{
	// Token: 0x06002EA0 RID: 11936 RVA: 0x001276BE File Offset: 0x001258BE
	public ItemWorldData(IGameManager _gm, ItemValue _itemValue, EntityItem _entityItem, int _belongsEntityId)
	{
		this.gameManager = _gm;
		this.world = _entityItem.world;
		this.entityItem = _entityItem;
		this.belongsEntityId = _belongsEntityId;
	}

	// Token: 0x04002402 RID: 9218
	public IGameManager gameManager;

	// Token: 0x04002403 RID: 9219
	public WorldBase world;

	// Token: 0x04002404 RID: 9220
	public EntityItem entityItem;

	// Token: 0x04002405 RID: 9221
	public int belongsEntityId;
}
