using System;

// Token: 0x020001CE RID: 462
public interface IEntityBuffsChanged
{
	// Token: 0x06000E3C RID: 3644
	void EntityBuffAdded(BuffValue _buff);

	// Token: 0x06000E3D RID: 3645
	void EntityBuffRemoved(BuffValue _buff);
}
