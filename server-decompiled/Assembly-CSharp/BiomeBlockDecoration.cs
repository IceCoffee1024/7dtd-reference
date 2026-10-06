using System;

// Token: 0x02000C0F RID: 3087
public class BiomeBlockDecoration
{
	// Token: 0x06005DC5 RID: 24005 RVA: 0x00248D3C File Offset: 0x00246F3C
	public BiomeBlockDecoration(string _name, float _prob, float _clusprob, bool _instantiateReferences, int _randomRotateMax, int _checkResource = 2147483647)
	{
		string[] array = _name.Split(',', StringSplitOptions.None);
		if (_instantiateReferences)
		{
			this.blockValues = new BlockValue[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				BlockValue blockValueForName = WorldBiomes.GetBlockValueForName(array[i]);
				if (_randomRotateMax > 3 && !blockValueForName.isair)
				{
					Block block = blockValueForName.Block;
					if (block.isMultiBlock && (block.multiBlockPos.dim.x > 1 || block.multiBlockPos.dim.z > 1))
					{
						Log.Error("Parsing biomes. Block with name '" + array[i] + "' supports only rotations 0-3, setting it to 3");
						_randomRotateMax = 3;
					}
				}
				this.blockValues[i] = blockValueForName;
			}
		}
		this.prob = _prob;
		this.clusterProb = _clusprob;
		this.randomRotateMax = _randomRotateMax;
		this.checkResourceOffsetY = _checkResource;
	}

	// Token: 0x06005DC6 RID: 24006 RVA: 0x00248E10 File Offset: 0x00247010
	public static byte GetRandomRotation(float _rnd, int _randomRotateMax)
	{
		byte b = (byte)(_rnd * (float)_randomRotateMax + 0.5f);
		if (b >= 4 && b <= 7)
		{
			b = b - 4 + 24;
		}
		return b;
	}

	// Token: 0x040048BA RID: 18618
	public BlockValue[] blockValues;

	// Token: 0x040048BB RID: 18619
	public float prob;

	// Token: 0x040048BC RID: 18620
	public float clusterProb;

	// Token: 0x040048BD RID: 18621
	public int randomRotateMax;

	// Token: 0x040048BE RID: 18622
	public int checkResourceOffsetY;
}
