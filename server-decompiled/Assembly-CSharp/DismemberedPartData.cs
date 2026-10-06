using System;
using UnityEngine;

// Token: 0x020000E3 RID: 227
public class DismemberedPartData
{
	// Token: 0x1700005E RID: 94
	// (get) Token: 0x060005B9 RID: 1465 RVA: 0x000295EA File Offset: 0x000277EA
	// (set) Token: 0x060005BA RID: 1466 RVA: 0x000295F2 File Offset: 0x000277F2
	public Vector3 rot
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x060005BB RID: 1467 RVA: 0x000295FB File Offset: 0x000277FB
	// (set) Token: 0x060005BC RID: 1468 RVA: 0x00029603 File Offset: 0x00027803
	public bool hasRotOffset
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x0002960C File Offset: 0x0002780C
	public void SetRot(Vector3 _rot)
	{
		this.hasRotOffset = true;
		this.rot = _rot;
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x060005BE RID: 1470 RVA: 0x0002961C File Offset: 0x0002781C
	// (set) Token: 0x060005BF RID: 1471 RVA: 0x00029624 File Offset: 0x00027824
	public bool Invalid { get; set; }

	// Token: 0x060005C0 RID: 1472 RVA: 0x0002962D File Offset: 0x0002782D
	public string Log()
	{
		return string.Format(" property: {0} prefabPath: {1} target: {2} damageTag {3}", new object[] { this.propertyKey, this.prefabPath, this.targetBone, this.damageTypeKey });
	}

	// Token: 0x04000636 RID: 1590
	public string propertyKey;

	// Token: 0x04000637 RID: 1591
	public string prefabPath;

	// Token: 0x04000638 RID: 1592
	public string targetBone;

	// Token: 0x04000639 RID: 1593
	public string damageTypeKey;

	// Token: 0x0400063A RID: 1594
	public bool isDetachable;

	// Token: 0x0400063B RID: 1595
	public Vector3 pos;

	// Token: 0x0400063C RID: 1596
	public Vector3 scale;

	// Token: 0x0400063D RID: 1597
	public Vector3 offset;

	// Token: 0x0400063E RID: 1598
	public bool attachToParent;

	// Token: 0x0400063F RID: 1599
	public string[] particlePaths;

	// Token: 0x04000640 RID: 1600
	public bool useMask;

	// Token: 0x04000641 RID: 1601
	public bool isLinked;

	// Token: 0x04000642 RID: 1602
	public bool scaleOutLimb;

	// Token: 0x04000643 RID: 1603
	public string solTarget;

	// Token: 0x04000644 RID: 1604
	public Vector3 solScale;

	// Token: 0x04000645 RID: 1605
	public bool hasSolScale;

	// Token: 0x04000646 RID: 1606
	public string childTargetObj;

	// Token: 0x04000647 RID: 1607
	public string insertBoneObj;

	// Token: 0x04000648 RID: 1608
	public string addScalePoint;

	// Token: 0x04000649 RID: 1609
	public string maskScaleBlend;

	// Token: 0x0400064A RID: 1610
	public string setFixedValues;

	// Token: 0x0400064B RID: 1611
	public string dismemberMatPath;
}
