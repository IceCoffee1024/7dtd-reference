using System;
using System.IO;

// Token: 0x02000C30 RID: 3120
public class BiomeIntensityMap
{
	// Token: 0x06005F00 RID: 24320 RVA: 0x0000640C File Offset: 0x0000460C
	public BiomeIntensityMap()
	{
	}

	// Token: 0x06005F01 RID: 24321 RVA: 0x002595F0 File Offset: 0x002577F0
	public BiomeIntensityMap(int _w, int _h)
	{
		this.intensities = new ArrayWithOffset<BiomeIntensity>(_w, _h);
	}

	// Token: 0x06005F02 RID: 24322 RVA: 0x00259608 File Offset: 0x00257808
	public void Load(string _worldName)
	{
		try
		{
			string text = PathAbstractions.WorldsSearchPaths.GetLocation(_worldName, null, null).FullPath + "/biomeintensity.dat";
			if (!SdFile.Exists(text))
			{
				this.intensities = null;
			}
			else
			{
				using (Stream stream = SdFile.Open(text, FileMode.Open))
				{
					using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
					{
						pooledBinaryReader.SetBaseStream(stream);
						pooledBinaryReader.ReadByte();
						pooledBinaryReader.ReadByte();
						pooledBinaryReader.ReadByte();
						pooledBinaryReader.ReadByte();
						pooledBinaryReader.ReadByte();
						int num = (int)pooledBinaryReader.ReadUInt16();
						int num2 = (int)pooledBinaryReader.ReadUInt16();
						this.intensities = new ArrayWithOffset<BiomeIntensity>(num, num2);
						num /= 2;
						num2 /= 2;
						for (int i = -num; i < num; i++)
						{
							for (int j = -num2; j < num2; j++)
							{
								BiomeIntensity biomeIntensity = default(BiomeIntensity);
								biomeIntensity.Read(pooledBinaryReader);
								this.intensities[i, j] = biomeIntensity;
							}
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("Reading biome intensity map: " + ex.Message);
		}
	}

	// Token: 0x06005F03 RID: 24323 RVA: 0x00259798 File Offset: 0x00257998
	public void Save(string _worldPath)
	{
		try
		{
			using (Stream stream = SdFile.Open(_worldPath + "/biomeintensity.dat", FileMode.Create))
			{
				using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
				{
					pooledBinaryWriter.SetBaseStream(stream);
					pooledBinaryWriter.Write(66);
					pooledBinaryWriter.Write(73);
					pooledBinaryWriter.Write(73);
					pooledBinaryWriter.Write(0);
					pooledBinaryWriter.Write(1);
					int num = this.intensities.DimX;
					int num2 = this.intensities.DimY;
					pooledBinaryWriter.Write((ushort)num);
					pooledBinaryWriter.Write((ushort)num2);
					num /= 2;
					num2 /= 2;
					for (int i = -num; i < num; i++)
					{
						for (int j = -num2; j < num2; j++)
						{
							this.intensities[i, j].Write(pooledBinaryWriter);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("Writing biome intensity map: " + ex.Message);
		}
	}

	// Token: 0x06005F04 RID: 24324 RVA: 0x002598B8 File Offset: 0x00257AB8
	public void SetBiomeIntensity(int _x, int _y, BiomeIntensity _bi)
	{
		if (this.intensities != null && this.intensities.Contains(_x, _y))
		{
			this.intensities[_x, _y] = _bi;
		}
	}

	// Token: 0x06005F05 RID: 24325 RVA: 0x002598DF File Offset: 0x00257ADF
	public BiomeIntensity GetBiomeIntensity(int _x, int _y)
	{
		if (this.intensities != null && this.intensities.Contains(_x, _y))
		{
			return this.intensities[_x, _y];
		}
		return BiomeIntensity.Default;
	}

	// Token: 0x04004A53 RID: 19027
	[PublicizedFrom(EAccessModifier.Private)]
	public ArrayWithOffset<BiomeIntensity> intensities;
}
