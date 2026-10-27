using System;

// Token: 0x02000585 RID: 1413
public interface IItemStackGridHolder : IHasWorldAddress
{
	// Token: 0x06002DF1 RID: 11761
	byte GetItemStackGridKey(ItemStackGrid _grid);

	// Token: 0x06002DF2 RID: 11762
	ItemStackGrid GetItemStackGrid(byte _key);
}
