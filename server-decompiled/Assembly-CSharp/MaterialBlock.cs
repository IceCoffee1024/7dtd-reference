using System;
using System.Collections.Generic;

// Token: 0x02000192 RID: 402
public class MaterialBlock
{
	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000C23 RID: 3107 RVA: 0x0004C4E6 File Offset: 0x0004A6E6
	// (set) Token: 0x06000C24 RID: 3108 RVA: 0x0004C4EE File Offset: 0x0004A6EE
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

	// Token: 0x06000C26 RID: 3110 RVA: 0x0004C512 File Offset: 0x0004A712
	public static void Cleanup()
	{
		MaterialBlock.materials = new Dictionary<string, MaterialBlock>();
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x0004C520 File Offset: 0x0004A720
	public MaterialBlock()
	{
		this.Properties = new DynamicProperties();
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x0004C58A File Offset: 0x0004A78A
	public MaterialBlock(string _id)
		: this()
	{
		this.id = _id;
		this.IsCollidable = true;
		this.LightOpacity = 0;
		MaterialBlock.materials[_id] = this;
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x0004C5B3 File Offset: 0x0004A7B3
	public static MaterialBlock fromString(string _name)
	{
		if (!MaterialBlock.materials.ContainsKey(_name))
		{
			return null;
		}
		return MaterialBlock.materials[_name];
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x0004C5CF File Offset: 0x0004A7CF
	public string GetLocalizedMaterialName()
	{
		return Localization.Get("material" + this.id, false, null);
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x0004C5E8 File Offset: 0x0004A7E8
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

	// Token: 0x04000AEB RID: 2795
	public static MaterialBlock air = new MaterialBlock();

	// Token: 0x04000AEC RID: 2796
	public static Dictionary<string, MaterialBlock> materials = new Dictionary<string, MaterialBlock>();

	// Token: 0x04000AED RID: 2797
	public DynamicProperties Properties;

	// Token: 0x04000AEE RID: 2798
	public bool StabilitySupport = true;

	// Token: 0x04000AEF RID: 2799
	public DataItem<float> Hardness;

	// Token: 0x04000AF0 RID: 2800
	public StepSound stepSound;

	// Token: 0x04000AF1 RID: 2801
	public bool IsCollidable;

	// Token: 0x04000AF2 RID: 2802
	public int LightOpacity;

	// Token: 0x04000AF3 RID: 2803
	public int StabilityGlue = 6;

	// Token: 0x04000AF4 RID: 2804
	public bool IsLiquid;

	// Token: 0x04000AF5 RID: 2805
	public string DamageCategory;

	// Token: 0x04000AF6 RID: 2806
	public string SurfaceCategory;

	// Token: 0x04000AF7 RID: 2807
	public string ParticleCategory;

	// Token: 0x04000AF8 RID: 2808
	public string ParticleDestroyCategory;

	// Token: 0x04000AF9 RID: 2809
	public string ForgeCategory;

	// Token: 0x04000AFA RID: 2810
	public int FertileLevel;

	// Token: 0x04000AFB RID: 2811
	public bool IsPlant;

	// Token: 0x04000AFC RID: 2812
	public float MovementFactor = 1f;

	// Token: 0x04000AFD RID: 2813
	public float Friction = 1f;

	// Token: 0x04000AFE RID: 2814
	public DataItem<int> Mass;

	// Token: 0x04000AFF RID: 2815
	public int MaxDamage;

	// Token: 0x04000B00 RID: 2816
	public int MaxIncomingDamage = int.MaxValue;

	// Token: 0x04000B01 RID: 2817
	public float Experience = 1f;

	// Token: 0x04000B02 RID: 2818
	public string id;

	// Token: 0x04000B03 RID: 2819
	public FastTags<TagGroup.Global> IgnoreDamageFromTag = FastTags<TagGroup.Global>.none;

	// Token: 0x04000B04 RID: 2820
	public bool IsGroundCover;

	// Token: 0x04000B05 RID: 2821
	public bool CanDestroy = true;

	// Token: 0x04000B06 RID: 2822
	[PublicizedFrom(EAccessModifier.Private)]
	public float explosionResistance;
}
