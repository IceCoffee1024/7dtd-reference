using System;

// Token: 0x020001CD RID: 461
public interface IEntityBuffsChanged
{
	// Token: 0x06000E45 RID: 3653
	void EntityBuffAdded(BuffValue _buff);

	// Token: 0x06000E46 RID: 3654
	void EntityBuffRemoved(BuffValue _buff);
}
