using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B17 RID: 2839
public class BiomeAtmosphereEffects
{
	// Token: 0x060053EF RID: 21487 RVA: 0x00204F00 File Offset: 0x00203100
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

	// Token: 0x060053F0 RID: 21488 RVA: 0x00204FB0 File Offset: 0x002031B0
	public void Reload()
	{
		this.Init(this.world);
		this.Update();
	}

	// Token: 0x060053F1 RID: 21489 RVA: 0x00204FC4 File Offset: 0x002031C4
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

	// Token: 0x060053F2 RID: 21490 RVA: 0x002050EF File Offset: 0x002032EF
	public virtual Color GetSkyColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Sky);
	}

	// Token: 0x060053F3 RID: 21491 RVA: 0x002050FF File Offset: 0x002032FF
	public virtual Color GetAmbientColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Ambient);
	}

	// Token: 0x060053F4 RID: 21492 RVA: 0x0020510F File Offset: 0x0020330F
	public virtual Color GetSunColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Sun);
	}

	// Token: 0x060053F5 RID: 21493 RVA: 0x0020511F File Offset: 0x0020331F
	public virtual Color GetMoonColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Moon);
	}

	// Token: 0x060053F6 RID: 21494 RVA: 0x0020512F File Offset: 0x0020332F
	public virtual Color GetFogColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Fog);
	}

	// Token: 0x060053F7 RID: 21495 RVA: 0x0020513F File Offset: 0x0020333F
	public virtual Color GetFogFadeColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.FogFade);
	}

	// Token: 0x060053F8 RID: 21496 RVA: 0x000BDB9B File Offset: 0x000BBD9B
	public virtual Color GetCloudsColor(float _v)
	{
		return Color.white;
	}

	// Token: 0x060053F9 RID: 21497 RVA: 0x00205150 File Offset: 0x00203350
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

	// Token: 0x040041D8 RID: 16856
	public BiomeDefinition[] nearBiomes = new BiomeDefinition[4];

	// Token: 0x040041D9 RID: 16857
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeIntensity currentBiomeIntensity = BiomeIntensity.Default;

	// Token: 0x040041DA RID: 16858
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x040041DB RID: 16859
	[PublicizedFrom(EAccessModifier.Private)]
	public AtmosphereEffect[] worldColorSpectrums;

	// Token: 0x040041DC RID: 16860
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i playerPosition;

	// Token: 0x040041DD RID: 16861
	public bool ForceDefault;
}
