using System;
using System.IO;

// Token: 0x02000C5F RID: 3167
public class BiomeIntensityMap
{
	// Token: 0x06005FC8 RID: 24520 RVA: 0x0000640C File Offset: 0x0000460C
	public BiomeIntensityMap()
	{
	}

	// Token: 0x06005FC9 RID: 24521 RVA: 0x00260CC4 File Offset: 0x0025EEC4
	public BiomeIntensityMap(int _w, int _h)
	{
		this.intensities = new ArrayWithOffset<BiomeIntensity>(_w, _h);
	}

	// Token: 0x06005FCA RID: 24522 RVA: 0x00260CDC File Offset: 0x0025EEDC
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

	// Token: 0x06005FCB RID: 24523 RVA: 0x00260E6C File Offset: 0x0025F06C
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

	// Token: 0x06005FCC RID: 24524 RVA: 0x00260F8C File Offset: 0x0025F18C
	public void SetBiomeIntensity(int _x, int _y, BiomeIntensity _bi)
	{
		if (this.intensities != null && this.intensities.Contains(_x, _y))
		{
			this.intensities[_x, _y] = _bi;
		}
	}

	// Token: 0x06005FCD RID: 24525 RVA: 0x00260FB3 File Offset: 0x0025F1B3
	public BiomeIntensity GetBiomeIntensity(int _x, int _y)
	{
		if (this.intensities != null && this.intensities.Contains(_x, _y))
		{
			return this.intensities[_x, _y];
		}
		return BiomeIntensity.Default;
	}

	// Token: 0x04004B54 RID: 19284
	[PublicizedFrom(EAccessModifier.Private)]
	public ArrayWithOffset<BiomeIntensity> intensities;
}
