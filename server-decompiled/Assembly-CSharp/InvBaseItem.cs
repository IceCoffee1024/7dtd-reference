using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000041 RID: 65
[Serializable]
public class InvBaseItem
{
	// Token: 0x04000208 RID: 520
	public int id16;

	// Token: 0x04000209 RID: 521
	public string name;

	// Token: 0x0400020A RID: 522
	public string description;

	// Token: 0x0400020B RID: 523
	public InvBaseItem.Slot slot;

	// Token: 0x0400020C RID: 524
	public int minItemLevel = 1;

	// Token: 0x0400020D RID: 525
	public int maxItemLevel = 50;

	// Token: 0x0400020E RID: 526
	public List<InvStat> stats = new List<InvStat>();

	// Token: 0x0400020F RID: 527
	public GameObject attachment;

	// Token: 0x04000210 RID: 528
	public Color color = Color.white;

	// Token: 0x04000211 RID: 529
	public global::UnityEngine.Object iconAtlas;

	// Token: 0x04000212 RID: 530
	public string iconName = "";

	// Token: 0x02000042 RID: 66
	public enum Slot
	{
		// Token: 0x04000214 RID: 532
		None,
		// Token: 0x04000215 RID: 533
		Weapon,
		// Token: 0x04000216 RID: 534
		Shield,
		// Token: 0x04000217 RID: 535
		Body,
		// Token: 0x04000218 RID: 536
		Shoulders,
		// Token: 0x04000219 RID: 537
		Bracers,
		// Token: 0x0400021A RID: 538
		Boots,
		// Token: 0x0400021B RID: 539
		Trinket,
		// Token: 0x0400021C RID: 540
		_LastDoNotUse
	}
}
