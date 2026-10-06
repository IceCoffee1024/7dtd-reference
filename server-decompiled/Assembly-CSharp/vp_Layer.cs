using System;
using UnityEngine;

// Token: 0x020015A6 RID: 5542
public sealed class vp_Layer
{
	// Token: 0x0600AD0C RID: 44300 RVA: 0x00407A10 File Offset: 0x00405C10
	[PublicizedFrom(EAccessModifier.Private)]
	static vp_Layer()
	{
		Physics.IgnoreLayerCollision(30, 29);
		Physics.IgnoreLayerCollision(29, 29);
		Physics.IgnoreLayerCollision(22, 23);
	}

	// Token: 0x0600AD0D RID: 44301 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_Layer()
	{
	}

	// Token: 0x0600AD0E RID: 44302 RVA: 0x00407A38 File Offset: 0x00405C38
	public static void Set(GameObject obj, int layer, bool recursive = false)
	{
		if (layer < 0 || layer > 31)
		{
			Debug.LogError("vp_Layer: Attempted to set layer id out of range [0-31].");
			return;
		}
		obj.layer = layer;
		if (recursive)
		{
			foreach (object obj2 in obj.transform)
			{
				vp_Layer.Set(((Transform)obj2).gameObject, layer, true);
			}
		}
	}

	// Token: 0x0600AD0F RID: 44303 RVA: 0x00407AB4 File Offset: 0x00405CB4
	public static bool IsInMask(int layer, int layerMask)
	{
		return (layerMask & (1 << layer)) == 0;
	}

	// Token: 0x04008002 RID: 32770
	public static readonly vp_Layer instance = new vp_Layer();

	// Token: 0x04008003 RID: 32771
	public const int Default = 0;

	// Token: 0x04008004 RID: 32772
	public const int TransparentFX = 1;

	// Token: 0x04008005 RID: 32773
	public const int IgnoreRaycast = 2;

	// Token: 0x04008006 RID: 32774
	public const int Water = 4;

	// Token: 0x04008007 RID: 32775
	public const int Ragdoll = 22;

	// Token: 0x04008008 RID: 32776
	public const int PlayerDamageCollider = 23;

	// Token: 0x04008009 RID: 32777
	public const int IgnoreBullets = 24;

	// Token: 0x0400800A RID: 32778
	public const int Enemy = 25;

	// Token: 0x0400800B RID: 32779
	public const int Pickup = 26;

	// Token: 0x0400800C RID: 32780
	public const int Trigger = 27;

	// Token: 0x0400800D RID: 32781
	public const int MovableObject = 28;

	// Token: 0x0400800E RID: 32782
	public const int Debris = 29;

	// Token: 0x0400800F RID: 32783
	public const int LocalPlayer = 30;

	// Token: 0x04008010 RID: 32784
	public const int Weapon = 10;

	// Token: 0x020015A7 RID: 5543
	public static class Mask
	{
		// Token: 0x04008011 RID: 32785
		public const int BulletBlockers = -538750989;

		// Token: 0x04008012 RID: 32786
		public const int ExternalBlockers = 1084850184;

		// Token: 0x04008013 RID: 32787
		public const int CameraBlockers = 1082195968;

		// Token: 0x04008014 RID: 32788
		public const int PhysicsBlockers = 2261000;

		// Token: 0x04008015 RID: 32789
		public const int IgnoreWalkThru = -738197525;
	}
}
