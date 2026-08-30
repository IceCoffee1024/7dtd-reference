using System;
using System.Collections.Generic;

// Token: 0x0200018F RID: 399
public class MaterialBlock
{
	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06000C21 RID: 3105 RVA: 0x0004C10A File Offset: 0x0004A30A
	// (set) Token: 0x06000C22 RID: 3106 RVA: 0x0004C112 File Offset: 0x0004A312
	public float ExplosionResistance
	{
		get
		{
			return this.explosionResistance;
		}
		set
		{
			this.explosionResistance = Utils.FastClamp01(value);
		}
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x0004C136 File Offset: 0x0004A336
	public static void Cleanup()
	{
		MaterialBlock.materials = new Dictionary<string, MaterialBlock>();
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x0004C144 File Offset: 0x0004A344
	public MaterialBlock()
	{
		this.Properties = new DynamicProperties();
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x0004C1AE File Offset: 0x0004A3AE
	public MaterialBlock(string _id)
		: this()
	{
		this.id = _id;
		this.IsCollidable = true;
		this.LightOpacity = 0;
		MaterialBlock.materials[_id] = this;
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x0004C1D7 File Offset: 0x0004A3D7
	public static MaterialBlock fromString(string _name)
	{
		if (!MaterialBlock.materials.ContainsKey(_name))
		{
			return null;
		}
		return MaterialBlock.materials[_name];
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x0004C1F3 File Offset: 0x0004A3F3
	public string GetLocalizedMaterialName()
	{
		return Localization.Get("material" + this.id, false, null);
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x0004C20C File Offset: 0x0004A40C
	[PublicizedFrom(EAccessModifier.Internal)]
	public bool CheckDamageIgnore(FastTags<TagGroup.Global> _tags, EntityAlive _entity)
	{
		if (!this.IgnoreDamageFromTag.IsEmpty && _tags.Test_AnySet(this.IgnoreDamageFromTag))
		{
			if (_entity != null)
			{
				_entity.PlayOneShot("keystone_impact_overlay", false, false, false, null, 1f);
			}
			return true;
		}
		return false;
	}

	// Token: 0x04000AE5 RID: 2789
	public static MaterialBlock air = new MaterialBlock();

	// Token: 0x04000AE6 RID: 2790
	public static Dictionary<string, MaterialBlock> materials = new Dictionary<string, MaterialBlock>();

	// Token: 0x04000AE7 RID: 2791
	public DynamicProperties Properties;

	// Token: 0x04000AE8 RID: 2792
	public bool StabilitySupport = true;

	// Token: 0x04000AE9 RID: 2793
	public DataItem<float> Hardness;

	// Token: 0x04000AEA RID: 2794
	public StepSound stepSound;

	// Token: 0x04000AEB RID: 2795
	public bool IsCollidable;

	// Token: 0x04000AEC RID: 2796
	public int LightOpacity;

	// Token: 0x04000AED RID: 2797
	public int StabilityGlue = 6;

	// Token: 0x04000AEE RID: 2798
	public bool IsLiquid;

	// Token: 0x04000AEF RID: 2799
	public string DamageCategory;

	// Token: 0x04000AF0 RID: 2800
	public string SurfaceCategory;

	// Token: 0x04000AF1 RID: 2801
	public string ParticleCategory;

	// Token: 0x04000AF2 RID: 2802
	public string ParticleDestroyCategory;

	// Token: 0x04000AF3 RID: 2803
	public string ForgeCategory;

	// Token: 0x04000AF4 RID: 2804
	public int FertileLevel;

	// Token: 0x04000AF5 RID: 2805
	public bool IsPlant;

	// Token: 0x04000AF6 RID: 2806
	public float MovementFactor = 1f;

	// Token: 0x04000AF7 RID: 2807
	public float Friction = 1f;

	// Token: 0x04000AF8 RID: 2808
	public DataItem<int> Mass;

	// Token: 0x04000AF9 RID: 2809
	public int MaxDamage;

	// Token: 0x04000AFA RID: 2810
	public int MaxIncomingDamage = int.MaxValue;

	// Token: 0x04000AFB RID: 2811
	public float Experience = 1f;

	// Token: 0x04000AFC RID: 2812
	public string id;

	// Token: 0x04000AFD RID: 2813
	public FastTags<TagGroup.Global> IgnoreDamageFromTag = FastTags<TagGroup.Global>.none;

	// Token: 0x04000AFE RID: 2814
	public bool IsGroundCover;

	// Token: 0x04000AFF RID: 2815
	public bool CanDestroy = true;

	// Token: 0x04000B00 RID: 2816
	[PublicizedFrom(EAccessModifier.Private)]
	public float explosionResistance;
}
