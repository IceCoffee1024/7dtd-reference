using System;
using UnityEngine;

// Token: 0x02001569 RID: 5481
public sealed class vp_Layer
{
	// Token: 0x0600AB5C RID: 43868 RVA: 0x003FD87C File Offset: 0x003FBA7C
	[PublicizedFrom(EAccessModifier.Private)]
	static vp_Layer()
	{
		Physics.IgnoreLayerCollision(30, 29);
		Physics.IgnoreLayerCollision(29, 29);
		Physics.IgnoreLayerCollision(22, 23);
	}

	// Token: 0x0600AB5D RID: 43869 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_Layer()
	{
	}

	// Token: 0x0600AB5E RID: 43870 RVA: 0x003FD8A4 File Offset: 0x003FBAA4
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

	// Token: 0x0600AB5F RID: 43871 RVA: 0x003FD920 File Offset: 0x003FBB20
	public static bool IsInMask(int layer, int layerMask)
	{
		return (layerMask & (1 << layer)) == 0;
	}

	// Token: 0x04007EA6 RID: 32422
	public static readonly vp_Layer instance = new vp_Layer();

	// Token: 0x04007EA7 RID: 32423
	public const int Default = 0;

	// Token: 0x04007EA8 RID: 32424
	public const int TransparentFX = 1;

	// Token: 0x04007EA9 RID: 32425
	public const int IgnoreRaycast = 2;

	// Token: 0x04007EAA RID: 32426
	public const int Water = 4;

	// Token: 0x04007EAB RID: 32427
	public const int Ragdoll = 22;

	// Token: 0x04007EAC RID: 32428
	public const int PlayerDamageCollider = 23;

	// Token: 0x04007EAD RID: 32429
	public const int IgnoreBullets = 24;

	// Token: 0x04007EAE RID: 32430
	public const int Enemy = 25;

	// Token: 0x04007EAF RID: 32431
	public const int Pickup = 26;

	// Token: 0x04007EB0 RID: 32432
	public const int Trigger = 27;

	// Token: 0x04007EB1 RID: 32433
	public const int MovableObject = 28;

	// Token: 0x04007EB2 RID: 32434
	public const int Debris = 29;

	// Token: 0x04007EB3 RID: 32435
	public const int LocalPlayer = 30;

	// Token: 0x04007EB4 RID: 32436
	public const int Weapon = 10;

	// Token: 0x0200156A RID: 5482
	public static class Mask
	{
		// Token: 0x04007EB5 RID: 32437
		public const int BulletBlockers = -538750989;

		// Token: 0x04007EB6 RID: 32438
		public const int ExternalBlockers = 1084850184;

		// Token: 0x04007EB7 RID: 32439
		public const int CameraBlockers = 1082195968;

		// Token: 0x04007EB8 RID: 32440
		public const int PhysicsBlockers = 2261000;

		// Token: 0x04007EB9 RID: 32441
		public const int IgnoreWalkThru = -738197525;
	}
}
