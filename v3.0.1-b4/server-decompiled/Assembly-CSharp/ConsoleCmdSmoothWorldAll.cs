using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000298 RID: 664
[Preserve]
public class ConsoleCmdSmoothWorldAll : ConsoleCmdAbstract
{
	// Token: 0x06001362 RID: 4962 RVA: 0x000763BB File Offset: 0x000745BB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "smoothworldall", "swa" };
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x000763D4 File Offset: 0x000745D4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		string text = GamePrefs.GetString(EnumGamePrefs.GameWorld);
		int num = 5;
		bool flag = false;
		int num2 = 0;
		if (_params.Count > 0 && !StringParsers.TryParseSInt32(_params[0], out num))
		{
			num2 = 1;
			text = _params[0];
		}
		if (_params.Count > num2 && !StringParsers.TryParseSInt32(_params[num2], out num))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("The given value for passes is not a valid integer");
			return;
		}
		if (_params.Count > num2 + 1)
		{
			flag = _params[num2 + 1].EqualsCaseInsensitive("noregion");
		}
		MicroStopwatch microStopwatch = new MicroStopwatch();
		PathAbstractions.AbstractedLocation location = PathAbstractions.WorldsSearchPaths.GetLocation(text, null, null);
		float[,] array;
		if (!HeightMapUtils.TryGetHeightDataFromResources(text, out array))
		{
			array = HeightMapUtils.GetHeightDataFromWorld(location.FullPath, true);
		}
		int length = array.GetLength(0);
		int length2 = array.GetLength(1);
		long num3 = microStopwatch.ElapsedMilliseconds;
		Log.Out(string.Concat(new string[]
		{
			"Loading took ",
			num3.ToString(),
			"ms (",
			length.ToString(),
			"/",
			length2.ToString(),
			")"
		}));
		array = HeightMapUtils.SmoothTerrain(num, array);
		Log.Out("Smoothing took " + ((float)(microStopwatch.ElapsedMilliseconds - num3) / 1000f).ToCultureInvariantString() + "s");
		num3 = microStopwatch.ElapsedMilliseconds;
		this.writeHeightDataBinary(location, array);
		if (flag)
		{
			float[] array2 = new float[length * length2];
			for (int i = 0; i < length2; i++)
			{
				for (int j = 0; j < length; j++)
				{
					array2[j + i * length] = array[j, i];
				}
			}
			HeightMapUtils.SaveHeightMapRAW(location.FullPath + "/dtm.raw", length, length2, array2);
		}
		else
		{
			this.writeRegionFiles(location.FullPath, array);
		}
		Log.Out("Writing took " + ((float)(microStopwatch.ElapsedMilliseconds - num3) / 1000f).ToCultureInvariantString() + "s");
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x000765F8 File Offset: 0x000747F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Applies some batched smoothing commands.";
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x000765FF File Offset: 0x000747FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n  1. smoothworldall [passes] [noregion]\n  2. smoothworldall <worldname> [passes] [noregion]\nArguments:\n  passes: Integer number, overriding the default of 5 passes\n  noregion: If passed in literally it will write the resulting heightmap\n            to the dtm.raw instead of updating all of the worlds region files\n  worldname: If specified the command is applied to the given world instead of the currently loaded world";
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x00076608 File Offset: 0x00074808
	[PublicizedFrom(EAccessModifier.Private)]
	public void writeHeightDataBinary(PathAbstractions.AbstractedLocation _worldLocation, float[,] heightData)
	{
		try
		{
			using (Stream stream = SdFile.Open(_worldLocation.FullPath + "/heightinfo.dtm", FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
				{
					pooledBinaryWriter.SetBaseStream(stream);
					int length = heightData.GetLength(0);
					int length2 = heightData.GetLength(1);
					pooledBinaryWriter.Write(length);
					pooledBinaryWriter.Write(length2);
					for (int i = 0; i < length2; i++)
					{
						for (int j = 0; j < length; j++)
						{
							pooledBinaryWriter.Write((ushort)(heightData[j, i] * 255f));
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x000766E0 File Offset: 0x000748E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void writeRegionFiles(string worldPath, float[,] heightData)
	{
		int length = heightData.GetLength(0);
		int length2 = heightData.GetLength(1);
		string text = worldPath + "/Region";
		RegionFileManager regionFileManager = new RegionFileManager(text, text, 0, true);
		for (int i = -length / 2; i < length / 2; i += 16)
		{
			for (int j = -length2 / 2; j < length2 / 2; j += 16)
			{
				Chunk chunk = new Chunk();
				chunk.X = i >> 4;
				chunk.Z = j >> 4;
				this.fillChunk(chunk, heightData, Constants.cTerrainBlockValue);
				chunk.ResetStability();
				chunk.RefreshSunlight();
				chunk.NeedsDecoration = false;
				regionFileManager.AddChunkSync(chunk, false);
			}
			if (i % 64 == 0)
			{
				regionFileManager.MakePersistent(null, true);
				regionFileManager.WaitSaveDone();
				regionFileManager.Cleanup();
				regionFileManager = new RegionFileManager(text, text, 0, true);
			}
		}
		regionFileManager.MakePersistent(null, true);
		regionFileManager.WaitSaveDone();
		regionFileManager.Cleanup();
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x000767D0 File Offset: 0x000749D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void fillChunk(Chunk _chunk, float[,] heightData, BlockValue _fillValue)
	{
		int length = heightData.GetLength(0);
		int length2 = heightData.GetLength(1);
		int num = _chunk.X << 4;
		int num2 = _chunk.Z << 4;
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				int num3 = i + num + length / 2;
				int num4 = j + num2 + length2 / 2;
				float num5 = heightData[num3, num4] + 0.5f;
				int num6 = (int)num5;
				for (int k = 0; k <= num6; k++)
				{
					_chunk.SetBlockRaw(i, k, j, _fillValue);
					_chunk.SetDensity(i, k, j, MarchingCubes.DensityTerrain);
				}
				float num7 = num5 - (float)num6;
				_chunk.SetDensity(i, num6, j, (sbyte)((float)MarchingCubes.DensityTerrain * num7));
				_chunk.SetDensity(i, num6 + 1, j, (sbyte)((float)MarchingCubes.DensityAir * (1f - num7)));
				for (int l = num6 + 2; l < 256; l++)
				{
					_chunk.SetDensity(i, l, j, MarchingCubes.DensityAir);
				}
				_chunk.SetHeight(i, j, (byte)num6);
				_chunk.SetTerrainHeight(i, j, (byte)num6);
			}
		}
		_chunk.OnDecorated();
	}
}
