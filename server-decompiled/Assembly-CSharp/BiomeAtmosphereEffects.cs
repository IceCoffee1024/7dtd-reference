using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B0C RID: 2828
public class BiomeAtmosphereEffects
{
	// Token: 0x060053CA RID: 21450 RVA: 0x0020397C File Offset: 0x00201B7C
	public void Init(World _world)
	{
		this.world = _world;
		this.worldColorSpectrums = new AtmosphereEffect[255];
		this.worldColorSpectrums[0] = AtmosphereEffect.Load("default", null);
		foreach (KeyValuePair<uint, BiomeDefinition> keyValuePair in _world.Biomes.GetBiomeMap())
		{
			this.worldColorSpectrums[(int)keyValuePair.Value.m_Id] = AtmosphereEffect.Load(keyValuePair.Value.m_SpectrumName, this.worldColorSpectrums[0]);
		}
		this.ForceDefault = false;
	}

	// Token: 0x060053CB RID: 21451 RVA: 0x00203A2C File Offset: 0x00201C2C
	public void Reload()
	{
		this.Init(this.world);
		this.Update();
	}

	// Token: 0x060053CC RID: 21452 RVA: 0x00203A40 File Offset: 0x00201C40
	public virtual void Update()
	{
		EntityPlayerLocal primaryPlayer = this.world.GetPrimaryPlayer();
		if (primaryPlayer == null)
		{
			return;
		}
		if (this.ForceDefault)
		{
			this.currentBiomeIntensity = BiomeIntensity.Default;
			return;
		}
		Vector3i blockPosition = primaryPlayer.GetBlockPosition();
		BiomeIntensity biomeIntensity;
		if (!blockPosition.Equals(this.playerPosition) && this.world.GetBiomeIntensity(blockPosition, out biomeIntensity))
		{
			this.playerPosition = blockPosition;
			if (!this.currentBiomeIntensity.Equals(biomeIntensity))
			{
				WorldBiomes biomes = GameManager.Instance.World.Biomes;
				BiomeDefinition biomeDefinition = biomes.GetBiome(biomeIntensity.biomeId0);
				if (biomeDefinition != null)
				{
					biomeDefinition.currentPlayerIntensity = biomeIntensity.intensity0;
				}
				this.nearBiomes[0] = biomeDefinition;
				biomeDefinition = biomes.GetBiome(biomeIntensity.biomeId1);
				if (biomeDefinition != null)
				{
					biomeDefinition.currentPlayerIntensity = biomeIntensity.intensity1;
				}
				this.nearBiomes[1] = biomeDefinition;
				biomeDefinition = biomes.GetBiome(biomeIntensity.biomeId2);
				if (biomeDefinition != null)
				{
					biomeDefinition.currentPlayerIntensity = biomeIntensity.intensity1;
				}
				this.nearBiomes[2] = biomeDefinition;
				biomeDefinition = biomes.GetBiome(biomeIntensity.biomeId3);
				if (biomeDefinition != null)
				{
					biomeDefinition.currentPlayerIntensity = biomeIntensity.intensity2;
				}
				this.nearBiomes[3] = biomeDefinition;
			}
			this.currentBiomeIntensity = biomeIntensity;
		}
	}

	// Token: 0x060053CD RID: 21453 RVA: 0x00203B6B File Offset: 0x00201D6B
	public virtual Color GetSkyColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Sky);
	}

	// Token: 0x060053CE RID: 21454 RVA: 0x00203B7B File Offset: 0x00201D7B
	public virtual Color GetAmbientColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Ambient);
	}

	// Token: 0x060053CF RID: 21455 RVA: 0x00203B8B File Offset: 0x00201D8B
	public virtual Color GetSunColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Sun);
	}

	// Token: 0x060053D0 RID: 21456 RVA: 0x00203B9B File Offset: 0x00201D9B
	public virtual Color GetMoonColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Moon);
	}

	// Token: 0x060053D1 RID: 21457 RVA: 0x00203BAB File Offset: 0x00201DAB
	public virtual Color GetFogColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Fog);
	}

	// Token: 0x060053D2 RID: 21458 RVA: 0x00203BBB File Offset: 0x00201DBB
	public virtual Color GetFogFadeColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.FogFade);
	}

	// Token: 0x060053D3 RID: 21459 RVA: 0x000BD7EF File Offset: 0x000BB9EF
	public virtual Color GetCloudsColor(float _v)
	{
		return Color.white;
	}

	// Token: 0x060053D4 RID: 21460 RVA: 0x00203BCC File Offset: 0x00201DCC
	[PublicizedFrom(EAccessModifier.Private)]
	public Color getColorFromSpectrum(BiomeIntensity _bi, float _v, AtmosphereEffect.ESpecIdx _spectrumIdx)
	{
		float intensity = _bi.intensity0;
		float intensity2 = _bi.intensity1;
		float intensity3 = _bi.intensity2;
		float num = _bi.intensity3;
		num = 0f;
		return (this.worldColorSpectrums[(int)_bi.biomeId0].spectrums[(int)_spectrumIdx].GetValue(_v) * intensity + this.worldColorSpectrums[(int)_bi.biomeId1].spectrums[(int)_spectrumIdx].GetValue(_v) * intensity2 + this.worldColorSpectrums[(int)_bi.biomeId2].spectrums[(int)_spectrumIdx].GetValue(_v) * intensity3 + this.worldColorSpectrums[(int)_bi.biomeId3].spectrums[(int)_spectrumIdx].GetValue(_v) * num) / (intensity + intensity2 + intensity3 + num);
	}

	// Token: 0x040041B3 RID: 16819
	public BiomeDefinition[] nearBiomes = new BiomeDefinition[4];

	// Token: 0x040041B4 RID: 16820
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeIntensity currentBiomeIntensity = BiomeIntensity.Default;

	// Token: 0x040041B5 RID: 16821
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x040041B6 RID: 16822
	[PublicizedFrom(EAccessModifier.Private)]
	public AtmosphereEffect[] worldColorSpectrums;

	// Token: 0x040041B7 RID: 16823
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i playerPosition;

	// Token: 0x040041B8 RID: 16824
	public bool ForceDefault;
}
