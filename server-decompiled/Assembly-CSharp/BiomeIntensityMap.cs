using System;
using System.IO;

// Token: 0x02000C54 RID: 3156
public class BiomeIntensityMap
{
	// Token: 0x06005FA3 RID: 24483 RVA: 0x0000640C File Offset: 0x0000460C
	public BiomeIntensityMap()
	{
	}

	// Token: 0x06005FA4 RID: 24484 RVA: 0x0025F79C File Offset: 0x0025D99C
	public BiomeIntensityMap(int _w, int _h)
	{
		this.intensities = new ArrayWithOffset<BiomeIntensity>(_w, _h);
	}

	// Token: 0x06005FA5 RID: 24485 RVA: 0x0025F7B4 File Offset: 0x0025D9B4
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

	// Token: 0x06005FA6 RID: 24486 RVA: 0x0025F944 File Offset: 0x0025DB44
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

	// Token: 0x06005FA7 RID: 24487 RVA: 0x0025FA64 File Offset: 0x0025DC64
	public void SetBiomeIntensity(int _x, int _y, BiomeIntensity _bi)
	{
		if (this.intensities != null && this.intensities.Contains(_x, _y))
		{
			this.intensities[_x, _y] = _bi;
		}
	}

	// Token: 0x06005FA8 RID: 24488 RVA: 0x0025FA8B File Offset: 0x0025DC8B
	public BiomeIntensity GetBiomeIntensity(int _x, int _y)
	{
		if (this.intensities != null && this.intensities.Contains(_x, _y))
		{
			return this.intensities[_x, _y];
		}
		return BiomeIntensity.Default;
	}

	// Token: 0x04004B2F RID: 19247
	[PublicizedFrom(EAccessModifier.Private)]
	public ArrayWithOffset<BiomeIntensity> intensities;
}
