using System;

// Token: 0x020001CB RID: 459
public interface IEntityBuffsChanged
{
	// Token: 0x06000E3A RID: 3642
	void EntityBuffAdded(BuffValue _buff);

	// Token: 0x06000E3B RID: 3643
	void EntityBuffRemoved(BuffValue _buff);
}
