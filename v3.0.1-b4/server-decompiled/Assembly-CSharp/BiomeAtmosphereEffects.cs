using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AE8 RID: 2792
public class BiomeAtmosphereEffects
{
	// Token: 0x06005329 RID: 21289 RVA: 0x001FDAAC File Offset: 0x001FBCAC
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

	// Token: 0x0600532A RID: 21290 RVA: 0x001FDB5C File Offset: 0x001FBD5C
	public void Reload()
	{
		this.Init(this.world);
		this.Update();
	}

	// Token: 0x0600532B RID: 21291 RVA: 0x001FDB70 File Offset: 0x001FBD70
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

	// Token: 0x0600532C RID: 21292 RVA: 0x001FDC9B File Offset: 0x001FBE9B
	public virtual Color GetSkyColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Sky);
	}

	// Token: 0x0600532D RID: 21293 RVA: 0x001FDCAB File Offset: 0x001FBEAB
	public virtual Color GetAmbientColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Ambient);
	}

	// Token: 0x0600532E RID: 21294 RVA: 0x001FDCBB File Offset: 0x001FBEBB
	public virtual Color GetSunColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Sun);
	}

	// Token: 0x0600532F RID: 21295 RVA: 0x001FDCCB File Offset: 0x001FBECB
	public virtual Color GetMoonColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Moon);
	}

	// Token: 0x06005330 RID: 21296 RVA: 0x001FDCDB File Offset: 0x001FBEDB
	public virtual Color GetFogColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.Fog);
	}

	// Token: 0x06005331 RID: 21297 RVA: 0x001FDCEB File Offset: 0x001FBEEB
	public virtual Color GetFogFadeColorSpectrum(float _v)
	{
		return this.getColorFromSpectrum(this.currentBiomeIntensity, _v, AtmosphereEffect.ESpecIdx.FogFade);
	}

	// Token: 0x06005332 RID: 21298 RVA: 0x000BCED3 File Offset: 0x000BB0D3
	public virtual Color GetCloudsColor(float _v)
	{
		return Color.white;
	}

	// Token: 0x06005333 RID: 21299 RVA: 0x001FDCFC File Offset: 0x001FBEFC
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

	// Token: 0x040040E0 RID: 16608
	public BiomeDefinition[] nearBiomes = new BiomeDefinition[4];

	// Token: 0x040040E1 RID: 16609
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeIntensity currentBiomeIntensity = BiomeIntensity.Default;

	// Token: 0x040040E2 RID: 16610
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x040040E3 RID: 16611
	[PublicizedFrom(EAccessModifier.Private)]
	public AtmosphereEffect[] worldColorSpectrums;

	// Token: 0x040040E4 RID: 16612
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i playerPosition;

	// Token: 0x040040E5 RID: 16613
	public bool ForceDefault;
}
