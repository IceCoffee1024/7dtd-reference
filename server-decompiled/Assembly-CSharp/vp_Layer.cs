using System;
using UnityEngine;

// Token: 0x02001584 RID: 5508
public sealed class vp_Layer
{
	// Token: 0x0600AC59 RID: 44121 RVA: 0x00402BBC File Offset: 0x00400DBC
	[PublicizedFrom(EAccessModifier.Private)]
	static vp_Layer()
	{
		Physics.IgnoreLayerCollision(30, 29);
		Physics.IgnoreLayerCollision(29, 29);
		Physics.IgnoreLayerCollision(22, 23);
	}

	// Token: 0x0600AC5A RID: 44122 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_Layer()
	{
	}

	// Token: 0x0600AC5B RID: 44123 RVA: 0x00402BE4 File Offset: 0x00400DE4
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

	// Token: 0x0600AC5C RID: 44124 RVA: 0x00402C60 File Offset: 0x00400E60
	public static bool IsInMask(int layer, int layerMask)
	{
		return (layerMask & (1 << layer)) == 0;
	}

	// Token: 0x04007F7F RID: 32639
	public static readonly vp_Layer instance = new vp_Layer();

	// Token: 0x04007F80 RID: 32640
	public const int Default = 0;

	// Token: 0x04007F81 RID: 32641
	public const int TransparentFX = 1;

	// Token: 0x04007F82 RID: 32642
	public const int IgnoreRaycast = 2;

	// Token: 0x04007F83 RID: 32643
	public const int Water = 4;

	// Token: 0x04007F84 RID: 32644
	public const int Ragdoll = 22;

	// Token: 0x04007F85 RID: 32645
	public const int PlayerDamageCollider = 23;

	// Token: 0x04007F86 RID: 32646
	public const int IgnoreBullets = 24;

	// Token: 0x04007F87 RID: 32647
	public const int Enemy = 25;

	// Token: 0x04007F88 RID: 32648
	public const int Pickup = 26;

	// Token: 0x04007F89 RID: 32649
	public const int Trigger = 27;

	// Token: 0x04007F8A RID: 32650
	public const int MovableObject = 28;

	// Token: 0x04007F8B RID: 32651
	public const int Debris = 29;

	// Token: 0x04007F8C RID: 32652
	public const int LocalPlayer = 30;

	// Token: 0x04007F8D RID: 32653
	public const int Weapon = 10;

	// Token: 0x02001585 RID: 5509
	public static class Mask
	{
		// Token: 0x04007F8E RID: 32654
		public const int BulletBlockers = -538750989;

		// Token: 0x04007F8F RID: 32655
		public const int ExternalBlockers = 1084850184;

		// Token: 0x04007F90 RID: 32656
		public const int CameraBlockers = 1082195968;

		// Token: 0x04007F91 RID: 32657
		public const int PhysicsBlockers = 2261000;

		// Token: 0x04007F92 RID: 32658
		public const int IgnoreWalkThru = -738197525;
	}
}
