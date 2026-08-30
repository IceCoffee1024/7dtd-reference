using System;
using System.Collections.Generic;

// Token: 0x02000191 RID: 401
public class MaterialBlock
{
	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06000C2C RID: 3116 RVA: 0x0004C522 File Offset: 0x0004A722
	// (set) Token: 0x06000C2D RID: 3117 RVA: 0x0004C52A File Offset: 0x0004A72A
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

	// Token: 0x06000C2F RID: 3119 RVA: 0x0004C54E File Offset: 0x0004A74E
	public static void Cleanup()
	{
		MaterialBlock.materials = new Dictionary<string, MaterialBlock>();
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x0004C55C File Offset: 0x0004A75C
	public MaterialBlock()
	{
		this.Properties = new DynamicProperties();
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x0004C5C6 File Offset: 0x0004A7C6
	public MaterialBlock(string _id)
		: this()
	{
		this.id = _id;
		this.IsCollidable = true;
		this.LightOpacity = 0;
		MaterialBlock.materials[_id] = this;
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x0004C5EF File Offset: 0x0004A7EF
	public static MaterialBlock fromString(string _name)
	{
		if (!MaterialBlock.materials.ContainsKey(_name))
		{
			return null;
		}
		return MaterialBlock.materials[_name];
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x0004C60B File Offset: 0x0004A80B
	public string GetLocalizedMaterialName()
	{
		return Localization.Get("material" + this.id, false, null);
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x0004C624 File Offset: 0x0004A824
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

	// Token: 0x04000AF2 RID: 2802
	public static MaterialBlock air = new MaterialBlock();

	// Token: 0x04000AF3 RID: 2803
	public static Dictionary<string, MaterialBlock> materials = new Dictionary<string, MaterialBlock>();

	// Token: 0x04000AF4 RID: 2804
	public DynamicProperties Properties;

	// Token: 0x04000AF5 RID: 2805
	public bool StabilitySupport = true;

	// Token: 0x04000AF6 RID: 2806
	public DataItem<float> Hardness;

	// Token: 0x04000AF7 RID: 2807
	public StepSound stepSound;

	// Token: 0x04000AF8 RID: 2808
	public bool IsCollidable;

	// Token: 0x04000AF9 RID: 2809
	public int LightOpacity;

	// Token: 0x04000AFA RID: 2810
	public int StabilityGlue = 6;

	// Token: 0x04000AFB RID: 2811
	public bool IsLiquid;

	// Token: 0x04000AFC RID: 2812
	public string DamageCategory;

	// Token: 0x04000AFD RID: 2813
	public string SurfaceCategory;

	// Token: 0x04000AFE RID: 2814
	public string ParticleCategory;

	// Token: 0x04000AFF RID: 2815
	public string ParticleDestroyCategory;

	// Token: 0x04000B00 RID: 2816
	public string ForgeCategory;

	// Token: 0x04000B01 RID: 2817
	public int FertileLevel;

	// Token: 0x04000B02 RID: 2818
	public bool IsPlant;

	// Token: 0x04000B03 RID: 2819
	public float MovementFactor = 1f;

	// Token: 0x04000B04 RID: 2820
	public float Friction = 1f;

	// Token: 0x04000B05 RID: 2821
	public DataItem<int> Mass;

	// Token: 0x04000B06 RID: 2822
	public int MaxDamage;

	// Token: 0x04000B07 RID: 2823
	public int MaxIncomingDamage = int.MaxValue;

	// Token: 0x04000B08 RID: 2824
	public float Experience = 1f;

	// Token: 0x04000B09 RID: 2825
	public string id;

	// Token: 0x04000B0A RID: 2826
	public FastTags<TagGroup.Global> IgnoreDamageFromTag = FastTags<TagGroup.Global>.none;

	// Token: 0x04000B0B RID: 2827
	public bool IsGroundCover;

	// Token: 0x04000B0C RID: 2828
	public bool CanDestroy = true;

	// Token: 0x04000B0D RID: 2829
	[PublicizedFrom(EAccessModifier.Private)]
	public float explosionResistance;
}
