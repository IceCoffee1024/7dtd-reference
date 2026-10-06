using System;

// Token: 0x0200057E RID: 1406
public interface IItemStackGridHolder : IHasWorldAddress
{
	// Token: 0x06002DE5 RID: 11749
	byte GetItemStackGridKey(ItemStackGrid _grid);

	// Token: 0x06002DE6 RID: 11750
	ItemStackGrid GetItemStackGrid(byte _key);
}
