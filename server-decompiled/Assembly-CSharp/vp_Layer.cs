using System;
using UnityEngine;

// Token: 0x020015B8 RID: 5560
public sealed class vp_Layer
{
	// Token: 0x0600AD64 RID: 44388 RVA: 0x00409E14 File Offset: 0x00408014
	[PublicizedFrom(EAccessModifier.Private)]
	static vp_Layer()
	{
		Physics.IgnoreLayerCollision(30, 29);
		Physics.IgnoreLayerCollision(29, 29);
		Physics.IgnoreLayerCollision(22, 23);
	}

	// Token: 0x0600AD65 RID: 44389 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_Layer()
	{
	}

	// Token: 0x0600AD66 RID: 44390 RVA: 0x00409E3C File Offset: 0x0040803C
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

	// Token: 0x0600AD67 RID: 44391 RVA: 0x00409EB8 File Offset: 0x004080B8
	public static bool IsInMask(int layer, int layerMask)
	{
		return (layerMask & (1 << layer)) == 0;
	}

	// Token: 0x04008043 RID: 32835
	public static readonly vp_Layer instance = new vp_Layer();

	// Token: 0x04008044 RID: 32836
	public const int Default = 0;

	// Token: 0x04008045 RID: 32837
	public const int TransparentFX = 1;

	// Token: 0x04008046 RID: 32838
	public const int IgnoreRaycast = 2;

	// Token: 0x04008047 RID: 32839
	public const int Water = 4;

	// Token: 0x04008048 RID: 32840
	public const int Ragdoll = 22;

	// Token: 0x04008049 RID: 32841
	public const int PlayerDamageCollider = 23;

	// Token: 0x0400804A RID: 32842
	public const int IgnoreBullets = 24;

	// Token: 0x0400804B RID: 32843
	public const int Enemy = 25;

	// Token: 0x0400804C RID: 32844
	public const int Pickup = 26;

	// Token: 0x0400804D RID: 32845
	public const int Trigger = 27;

	// Token: 0x0400804E RID: 32846
	public const int MovableObject = 28;

	// Token: 0x0400804F RID: 32847
	public const int Debris = 29;

	// Token: 0x04008050 RID: 32848
	public const int LocalPlayer = 30;

	// Token: 0x04008051 RID: 32849
	public const int Weapon = 10;

	// Token: 0x020015B9 RID: 5561
	public static class Mask
	{
		// Token: 0x04008052 RID: 32850
		public const int BulletBlockers = -538750989;

		// Token: 0x04008053 RID: 32851
		public const int ExternalBlockers = 1084850184;

		// Token: 0x04008054 RID: 32852
		public const int CameraBlockers = 1082195968;

		// Token: 0x04008055 RID: 32853
		public const int PhysicsBlockers = 2261000;

		// Token: 0x04008056 RID: 32854
		public const int IgnoreWalkThru = -738197525;
	}
}
