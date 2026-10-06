using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

// Token: 0x0200054C RID: 1356
public struct ExplosionData
{
	// Token: 0x06002C49 RID: 11337 RVA: 0x0011919C File Offset: 0x0011739C
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

	// Token: 0x06002C4A RID: 11338 RVA: 0x00119250 File Offset: 0x00117450
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

	// Token: 0x06002C4B RID: 11339 RVA: 0x00119578 File Offset: 0x00117778
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

	// Token: 0x06002C4C RID: 11340 RVA: 0x001195C8 File Offset: 0x001177C8
	public void Read(PooledBinaryReader _br)
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

	// Token: 0x06002C4D RID: 11341 RVA: 0x001196B0 File Offset: 0x001178B0
	public void Write(PooledBinaryWriter _bw)
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

	// Token: 0x04002206 RID: 8710
	public const string PropExplosion = "Explosion";

	// Token: 0x04002207 RID: 8711
	public const string PropParticleIndex = "ParticleIndex";

	// Token: 0x04002208 RID: 8712
	public const string PropDuration = "Duration";

	// Token: 0x04002209 RID: 8713
	public const string PropRadiusBlocks = "RadiusBlocks";

	// Token: 0x0400220A RID: 8714
	public const string PropRadiusEntities = "RadiusEntities";

	// Token: 0x0400220B RID: 8715
	public const string PropBlockDamage = "BlockDamage";

	// Token: 0x0400220C RID: 8716
	public const string PropEntityDamage = "EntityDamage";

	// Token: 0x0400220D RID: 8717
	public const string PropBlockTags = "BlockTags";

	// Token: 0x0400220E RID: 8718
	public const string PropBlastPower = "BlastPower";

	// Token: 0x0400220F RID: 8719
	public const string PropBuff = "Buff";

	// Token: 0x04002210 RID: 8720
	public const string PropIgnoreHeatMap = "IgnoreHeatMap";

	// Token: 0x04002211 RID: 8721
	public const string PropDamageType = "DamageType";

	// Token: 0x04002212 RID: 8722
	public const int cMaxBlastPower = 100;

	// Token: 0x04002213 RID: 8723
	public int ParticleIndex;

	// Token: 0x04002214 RID: 8724
	public float Duration;

	// Token: 0x04002215 RID: 8725
	public float BlockRadius;

	// Token: 0x04002216 RID: 8726
	public int EntityRadius;

	// Token: 0x04002217 RID: 8727
	public int BlastPower;

	// Token: 0x04002218 RID: 8728
	public float EntityDamage;

	// Token: 0x04002219 RID: 8729
	public float BlockDamage;

	// Token: 0x0400221A RID: 8730
	public string BlockTags;

	// Token: 0x0400221B RID: 8731
	public bool IgnoreHeatMap;

	// Token: 0x0400221C RID: 8732
	public EnumDamageTypes DamageType;

	// Token: 0x0400221D RID: 8733
	public DamageMultiplier damageMultiplier;

	// Token: 0x0400221E RID: 8734
	public List<string> BuffActions;
}
