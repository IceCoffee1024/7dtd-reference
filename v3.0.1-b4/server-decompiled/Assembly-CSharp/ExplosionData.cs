using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

// Token: 0x02000541 RID: 1345
public struct ExplosionData
{
	// Token: 0x06002C29 RID: 11305 RVA: 0x00117358 File Offset: 0x00115558
	public ExplosionData(byte[] _explosionDataAsArr)
	{
		this.ParticleIndex = 0;
		this.Duration = 0f;
		this.BlockRadius = 0f;
		this.EntityRadius = 0;
		this.EntityDamage = 0f;
		this.BlockDamage = 0f;
		this.BlockTags = string.Empty;
		this.BlastPower = 100;
		this.damageMultiplier = null;
		this.BuffActions = null;
		this.IgnoreHeatMap = false;
		this.DamageType = EnumDamageTypes.Heat;
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			pooledBinaryReader.SetBaseStream(new MemoryStream(_explosionDataAsArr));
			this.Read(pooledBinaryReader);
		}
	}

	// Token: 0x06002C2A RID: 11306 RVA: 0x0011740C File Offset: 0x0011560C
	public ExplosionData(DynamicProperties _properties, MinEffectController _effects = null)
	{
		this.ParticleIndex = 0;
		this.Duration = 0f;
		this.BlockRadius = 0f;
		this.EntityRadius = 0;
		this.EntityDamage = 0f;
		this.BlockDamage = 0f;
		this.BlockTags = string.Empty;
		this.BlastPower = 100;
		this.damageMultiplier = null;
		this.BuffActions = null;
		this.IgnoreHeatMap = false;
		this.DamageType = EnumDamageTypes.Heat;
		if (!_properties.Classes.ContainsKey("Explosion"))
		{
			return;
		}
		DynamicProperties dynamicProperties = _properties.Classes["Explosion"];
		this.ParticleIndex = 0;
		dynamicProperties.ParseInt("ParticleIndex", ref this.ParticleIndex);
		this.Duration = 0f;
		dynamicProperties.ParseFloat("Duration", ref this.Duration);
		this.BlockRadius = 1f;
		dynamicProperties.ParseFloat("RadiusBlocks", ref this.BlockRadius);
		if (dynamicProperties.Values.ContainsKey("BlockDamage"))
		{
			this.BlockDamage = StringParsers.ParseFloat(dynamicProperties.Values["BlockDamage"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.BlockDamage = this.BlockRadius * this.BlockRadius;
		}
		this.BlockTags = string.Empty;
		dynamicProperties.ParseString("BlockTags", ref this.BlockTags);
		this.EntityRadius = 0;
		if (dynamicProperties.Values.ContainsKey("RadiusEntities"))
		{
			this.EntityRadius = (int)StringParsers.ParseFloat(dynamicProperties.Values["RadiusEntities"], 0, -1, NumberStyles.Any);
		}
		if (dynamicProperties.Values.ContainsKey("EntityDamage"))
		{
			this.EntityDamage = StringParsers.ParseFloat(dynamicProperties.Values["EntityDamage"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.EntityDamage = 20f * (float)this.EntityRadius;
		}
		this.BlastPower = 100;
		if (dynamicProperties.Values.ContainsKey("BlastPower"))
		{
			this.BlastPower = (int)StringParsers.ParseFloat(dynamicProperties.Values["BlastPower"], 0, -1, NumberStyles.Any);
		}
		this.BuffActions = null;
		dynamicProperties.ParseFloat("RadiusBlocks", ref this.BlockRadius);
		if (dynamicProperties.Values.ContainsKey("Buff"))
		{
			string[] array = dynamicProperties.Values["Buff"].Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				if (this.BuffActions == null)
				{
					this.BuffActions = new List<string>();
				}
				this.BuffActions.Add(array[i]);
			}
		}
		this.damageMultiplier = new DamageMultiplier(dynamicProperties);
		this.IgnoreHeatMap = false;
		dynamicProperties.ParseBool("IgnoreHeatMap", ref this.IgnoreHeatMap);
		this.DamageType = EnumDamageTypes.Heat;
		dynamicProperties.ParseEnum<EnumDamageTypes>("DamageType", ref this.DamageType);
		if (_effects != null)
		{
			MinEffectGroup minEffectGroup = new MinEffectGroup
			{
				OwnerTiered = false
			};
			if (!_effects.PassivesIndex.Contains(PassiveEffects.ExplosionBlockDamage))
			{
				PassiveEffect passiveEffect = PassiveEffect.CreateEmptyPassiveEffect(PassiveEffects.ExplosionBlockDamage);
				MinEffectGroup.AddPassiveEffectToGroup(minEffectGroup, passiveEffect);
			}
			if (!_effects.PassivesIndex.Contains(PassiveEffects.ExplosionEntityDamage))
			{
				PassiveEffect passiveEffect2 = PassiveEffect.CreateEmptyPassiveEffect(PassiveEffects.ExplosionEntityDamage);
				MinEffectGroup.AddPassiveEffectToGroup(minEffectGroup, passiveEffect2);
			}
			if (minEffectGroup.PassiveEffects.Count > 0)
			{
				_effects.AddEffectGroup(minEffectGroup, 0, false);
			}
		}
	}

	// Token: 0x06002C2B RID: 11307 RVA: 0x00117734 File Offset: 0x00115934
	public byte[] ToByteArray()
	{
		MemoryStream memoryStream = new MemoryStream();
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(memoryStream);
			this.Write(pooledBinaryWriter);
		}
		return memoryStream.ToArray();
	}

	// Token: 0x06002C2C RID: 11308 RVA: 0x00117784 File Offset: 0x00115984
	public void Read(BinaryReader _br)
	{
		this.ParticleIndex = (int)_br.ReadInt16();
		this.Duration = (float)_br.ReadInt16() * 0.1f;
		this.BlockRadius = (float)_br.ReadInt16() * 0.05f;
		this.EntityRadius = (int)_br.ReadInt16();
		this.BlastPower = (int)_br.ReadInt16();
		this.BlockDamage = _br.ReadSingle();
		this.EntityDamage = _br.ReadSingle();
		this.BlockTags = _br.ReadString();
		this.IgnoreHeatMap = _br.ReadBoolean();
		this.DamageType = (EnumDamageTypes)_br.ReadInt16();
		this.damageMultiplier = new DamageMultiplier();
		this.damageMultiplier.Read(_br);
		int num = (int)_br.ReadByte();
		if (num > 0)
		{
			this.BuffActions = new List<string>();
			for (int i = 0; i < num; i++)
			{
				this.BuffActions.Add(_br.ReadString());
			}
			return;
		}
		this.BuffActions = null;
	}

	// Token: 0x06002C2D RID: 11309 RVA: 0x0011786C File Offset: 0x00115A6C
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((short)this.ParticleIndex);
		_bw.Write((short)(this.Duration * 10f));
		_bw.Write((short)(this.BlockRadius * 20f));
		_bw.Write((short)this.EntityRadius);
		_bw.Write((short)this.BlastPower);
		_bw.Write(this.BlockDamage);
		_bw.Write(this.EntityDamage);
		_bw.Write(this.BlockTags);
		_bw.Write(this.IgnoreHeatMap);
		_bw.Write((short)this.DamageType);
		this.damageMultiplier.Write(_bw);
		if (this.BuffActions != null)
		{
			_bw.Write((byte)this.BuffActions.Count);
			for (int i = 0; i < this.BuffActions.Count; i++)
			{
				_bw.Write(this.BuffActions[i]);
			}
			return;
		}
		_bw.Write(0);
	}

	// Token: 0x040021CC RID: 8652
	public const string PropExplosion = "Explosion";

	// Token: 0x040021CD RID: 8653
	public const string PropParticleIndex = "ParticleIndex";

	// Token: 0x040021CE RID: 8654
	public const string PropDuration = "Duration";

	// Token: 0x040021CF RID: 8655
	public const string PropRadiusBlocks = "RadiusBlocks";

	// Token: 0x040021D0 RID: 8656
	public const string PropRadiusEntities = "RadiusEntities";

	// Token: 0x040021D1 RID: 8657
	public const string PropBlockDamage = "BlockDamage";

	// Token: 0x040021D2 RID: 8658
	public const string PropEntityDamage = "EntityDamage";

	// Token: 0x040021D3 RID: 8659
	public const string PropBlockTags = "BlockTags";

	// Token: 0x040021D4 RID: 8660
	public const string PropBlastPower = "BlastPower";

	// Token: 0x040021D5 RID: 8661
	public const string PropBuff = "Buff";

	// Token: 0x040021D6 RID: 8662
	public const string PropIgnoreHeatMap = "IgnoreHeatMap";

	// Token: 0x040021D7 RID: 8663
	public const string PropDamageType = "DamageType";

	// Token: 0x040021D8 RID: 8664
	public const int cMaxBlastPower = 100;

	// Token: 0x040021D9 RID: 8665
	public int ParticleIndex;

	// Token: 0x040021DA RID: 8666
	public float Duration;

	// Token: 0x040021DB RID: 8667
	public float BlockRadius;

	// Token: 0x040021DC RID: 8668
	public int EntityRadius;

	// Token: 0x040021DD RID: 8669
	public int BlastPower;

	// Token: 0x040021DE RID: 8670
	public float EntityDamage;

	// Token: 0x040021DF RID: 8671
	public float BlockDamage;

	// Token: 0x040021E0 RID: 8672
	public string BlockTags;

	// Token: 0x040021E1 RID: 8673
	public bool IgnoreHeatMap;

	// Token: 0x040021E2 RID: 8674
	public EnumDamageTypes DamageType;

	// Token: 0x040021E3 RID: 8675
	public DamageMultiplier damageMultiplier;

	// Token: 0x040021E4 RID: 8676
	public List<string> BuffActions;
}
