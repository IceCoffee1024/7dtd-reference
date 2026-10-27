using System;

// Token: 0x02000C1A RID: 3098
public class BiomeBlockDecoration
{
	// Token: 0x06005DEA RID: 24042 RVA: 0x0024A264 File Offset: 0x00248464
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

	// Token: 0x06005DEB RID: 24043 RVA: 0x0024A338 File Offset: 0x00248538
	public static byte GetRandomRotation(float _rnd, int _randomRotateMax)
	{
		byte b = (byte)(_rnd * (float)_randomRotateMax + 0.5f);
		if (b >= 4 && b <= 7)
		{
			b = b - 4 + 24;
		}
		return b;
	}

	// Token: 0x040048DF RID: 18655
	public BlockValue[] blockValues;

	// Token: 0x040048E0 RID: 18656
	public float prob;

	// Token: 0x040048E1 RID: 18657
	public float clusterProb;

	// Token: 0x040048E2 RID: 18658
	public int randomRotateMax;

	// Token: 0x040048E3 RID: 18659
	public int checkResourceOffsetY;
}
