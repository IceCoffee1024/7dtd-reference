using System;
using System.Collections.Generic;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000136 RID: 310
[Preserve]
public class BlockPlaceholderMap
{
	// Token: 0x06000864 RID: 2148 RVA: 0x0003B73E File Offset: 0x0003993E
	public static void InitStatic()
	{
		BlockPlaceholderMap.Instance = new BlockPlaceholderMap();
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x0003B74A File Offset: 0x0003994A
	public static void Cleanup()
	{
		if (BlockPlaceholderMap.Instance != null)
		{
			BlockPlaceholderMap.Instance.Clear();
		}
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x0003B75D File Offset: 0x0003995D
	[PublicizedFrom(EAccessModifier.Private)]
	public void Clear()
	{
		this.questResetPlaceholders.Clear();
		this.placeholders.Clear();
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0003B775 File Offset: 0x00039975
	public void AddPlaceholder(BlockValue _placeholderBlockValue, BlockValue _targetValue, float _targetProb, string _biome, bool _randomRotation, string _sandboxOption)
	{
		this.addPlaceholderInternal(this.placeholders, _placeholderBlockValue, _targetValue, _targetProb, _biome, _randomRotation, _sandboxOption);
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x0003B78C File Offset: 0x0003998C
	public void AddQuestResetPlaceholder(BlockValue _placeholderBlockValue, BlockValue _targetValue, float _targetProb, string _biome, bool _randomRotation, FastTags<TagGroup.Global> questTags)
	{
		if (!this.questResetPlaceholders.ContainsKey(_placeholderBlockValue))
		{
			this.questResetPlaceholders.Add(_placeholderBlockValue, new List<BlockPlaceholderMap.QuestPlaceholderEntry>());
		}
		for (int i = 0; i < this.questResetPlaceholders[_placeholderBlockValue].Count; i++)
		{
			BlockPlaceholderMap.QuestPlaceholderEntry questPlaceholderEntry = this.questResetPlaceholders[_placeholderBlockValue][i];
			if (questPlaceholderEntry.QuestTag.Test_AnySet(questTags))
			{
				questPlaceholderEntry.PlaceholderList.Add(new BlockPlaceholderMap.PlaceholderTarget(_targetValue, _targetProb, _biome, _randomRotation, ""));
				return;
			}
		}
		BlockPlaceholderMap.QuestPlaceholderEntry questPlaceholderEntry2 = default(BlockPlaceholderMap.QuestPlaceholderEntry);
		questPlaceholderEntry2.QuestTag = questTags;
		questPlaceholderEntry2.PlaceholderList = new List<BlockPlaceholderMap.PlaceholderTarget>();
		questPlaceholderEntry2.PlaceholderList.Add(new BlockPlaceholderMap.PlaceholderTarget(_targetValue, _targetProb, _biome, _randomRotation, ""));
		this.questResetPlaceholders[_placeholderBlockValue].Add(questPlaceholderEntry2);
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x0003B85D File Offset: 0x00039A5D
	[PublicizedFrom(EAccessModifier.Private)]
	public void addPlaceholderInternal(Dictionary<BlockValue, List<BlockPlaceholderMap.PlaceholderTarget>> _map, BlockValue _placeholderBlockValue, BlockValue _targetValue, float _targetProb, string _biome, bool _randomRotation, string _sandboxOption)
	{
		if (!_map.ContainsKey(_placeholderBlockValue))
		{
			_map.Add(_placeholderBlockValue, new List<BlockPlaceholderMap.PlaceholderTarget>());
		}
		_map[_placeholderBlockValue].Add(new BlockPlaceholderMap.PlaceholderTarget(_targetValue, _targetProb, _biome, _randomRotation, _sandboxOption));
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x0003B890 File Offset: 0x00039A90
	public void AdjustData(BlockValue _blockValue)
	{
		List<BlockPlaceholderMap.PlaceholderTarget> list;
		if (this.placeholders.TryGetValue(_blockValue, out list))
		{
			list.Sort((BlockPlaceholderMap.PlaceholderTarget a, BlockPlaceholderMap.PlaceholderTarget b) => b.prob.CompareTo(a.prob));
		}
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x0003B8D2 File Offset: 0x00039AD2
	public bool IsReplaceableBlockType(BlockValue _blockValue)
	{
		return !_blockValue.isair && this.placeholders.ContainsKey(_blockValue);
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x0003B8EC File Offset: 0x00039AEC
	public BlockValue Replace(BlockValue _blockValue, GameRandom _random, int _blockX, int _blockZ, bool _useAlternate = false)
	{
		Chunk chunk = (Chunk)GameManager.Instance.World.GetChunkFromWorldPos(_blockX, _blockZ);
		return this.Replace(_blockValue, _random, chunk, _blockX, 0, _blockZ, FastTags<TagGroup.Global>.none, _useAlternate, true);
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x0003B928 File Offset: 0x00039B28
	public unsafe BlockValue Replace(BlockValue _blockValue, GameRandom _random, Chunk _chunk, int _blockX, int _blockY, int _blockZ, FastTags<TagGroup.Global> questTags, bool useAlternate = false, bool allowRandomRotation = true)
	{
		List<BlockPlaceholderMap.PlaceholderTarget> placeholderList;
		if (!this.placeholders.TryGetValue(_blockValue, out placeholderList))
		{
			return _blockValue;
		}
		bool ischild = _blockValue.ischild;
		Vector3i parent = _blockValue.parent;
		BlockValue blockValue = _blockValue;
		GameRandom gameRandom = _random;
		if (gameRandom == null)
		{
			Vector3i vector3i = _chunk.GetWorldPos() + new Vector3i(_blockX, _blockY, _blockZ);
			if (ischild)
			{
				vector3i += parent;
			}
			gameRandom = Utils.RandomFromSeedOnPos(vector3i.x, vector3i.y, vector3i.z, GameManager.Instance.World.Seed);
		}
		if (useAlternate && this.questResetPlaceholders.ContainsKey(_blockValue))
		{
			List<BlockPlaceholderMap.QuestPlaceholderEntry> list = this.questResetPlaceholders[_blockValue];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].QuestTag.Test_AnySet(questTags))
				{
					placeholderList = list[i].PlaceholderList;
					break;
				}
			}
		}
		int count = placeholderList.Count;
		Span<int> span;
		int num;
		float num2;
		string text;
		int j;
		checked
		{
			span = new Span<int>(stackalloc byte[unchecked((UIntPtr)count) * 4], count);
			num = 0;
			num2 = 0f;
			text = null;
			j = 0;
		}
		while (j < placeholderList.Count)
		{
			BlockPlaceholderMap.PlaceholderTarget placeholderTarget = placeholderList[j];
			if (placeholderTarget.biomeName == null)
			{
				goto IL_0164;
			}
			if (text == null)
			{
				byte biomeId = _chunk.GetBiomeId(World.toBlockXZ(_blockX), World.toBlockXZ(_blockZ));
				text = GameManager.Instance.World.Biomes.GetBiome(biomeId).m_sBiomeName;
			}
			if (placeholderTarget.biomeName.EqualsCaseInsensitive(text))
			{
				goto IL_0164;
			}
			IL_01B4:
			j++;
			continue;
			IL_0164:
			if (placeholderTarget.sandboxOption == global::SandboxOptions.SandboxOptions.Max || SandboxOptionManager.GetOptionType(placeholderTarget.sandboxOption) != BaseSandboxOption.OptionTypes.Bool || SandboxOptionManager.GetBool(placeholderTarget.sandboxOption) != placeholderTarget.invertSandbox)
			{
				*span[num] = j;
				num++;
				num2 += placeholderTarget.Prob;
				goto IL_01B4;
			}
			goto IL_01B4;
		}
		if (num > 0 && num2 > 0f)
		{
			num--;
			int num3 = *span[num];
			if (num > 0)
			{
				float num4 = gameRandom.RandomFloat * num2;
				for (int k = 0; k < num; k++)
				{
					int num5 = *span[k];
					float prob = placeholderList[num5].Prob;
					if (num4 < prob)
					{
						num3 = num5;
						break;
					}
					num4 -= prob;
				}
			}
			blockValue.type = placeholderList[num3].blockValue.type;
			if (allowRandomRotation && placeholderList[num3].isRandomRotation)
			{
				byte b;
				if (blockValue.Block.shape.Has45DegreeRotations)
				{
					b = (byte)gameRandom.RandomRange(8);
					if (b > 3)
					{
						b += 20;
					}
				}
				else
				{
					b = (byte)gameRandom.RandomRange(4);
				}
				blockValue.rotation = b;
			}
			else
			{
				blockValue.rotation = _blockValue.rotation;
			}
		}
		if (blockValue.Equals(_blockValue))
		{
			blockValue = BlockValue.Air;
		}
		if (_random == null)
		{
			GameRandomManager.Instance.FreeGameRandom(gameRandom);
		}
		if (ischild)
		{
			blockValue.ischild = true;
			blockValue.parent = parent;
		}
		return blockValue;
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x0003BC2C File Offset: 0x00039E2C
	public BlockValue Replace(BlockValueRef _bvRef, BlockValue _blockValue, GameRandom _random, bool _useAlternate = false)
	{
		Chunk chunk = (Chunk)GameManager.Instance.World.GetChunkSync(_bvRef);
		return this.Replace(_bvRef, _blockValue, _random, chunk, FastTags<TagGroup.Global>.none, _useAlternate, true);
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x0003BC64 File Offset: 0x00039E64
	public BlockValue Replace(BlockValueRef _bvRef, BlockValue _blockValue, GameRandom _random, Chunk _chunk, FastTags<TagGroup.Global> questTags, bool useAlternate = false, bool allowRandomRotation = true)
	{
		BlockValue blockValue;
		switch (_bvRef.Type)
		{
		case BlockValueRefType.None:
			blockValue = BlockValue.Air;
			break;
		case BlockValueRefType.Block:
			blockValue = this.Replace(_blockValue, _random, _chunk, _bvRef.BlockPosition.x, _bvRef.BlockPosition.y, _bvRef.BlockPosition.z, questTags, useAlternate, allowRandomRotation);
			break;
		case BlockValueRefType.Prop:
			blockValue = BlockValue.Air;
			break;
		default:
			throw new ArgumentOutOfRangeException("Type");
		}
		return blockValue;
	}

	// Token: 0x04000957 RID: 2391
	public static BlockPlaceholderMap Instance;

	// Token: 0x04000958 RID: 2392
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<BlockValue, List<BlockPlaceholderMap.PlaceholderTarget>> placeholders = new Dictionary<BlockValue, List<BlockPlaceholderMap.PlaceholderTarget>>();

	// Token: 0x04000959 RID: 2393
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<BlockValue, List<BlockPlaceholderMap.QuestPlaceholderEntry>> questResetPlaceholders = new Dictionary<BlockValue, List<BlockPlaceholderMap.QuestPlaceholderEntry>>();

	// Token: 0x02000137 RID: 311
	[PublicizedFrom(EAccessModifier.Private)]
	public struct PlaceholderTarget
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0003BCFC File Offset: 0x00039EFC
		public float Prob
		{
			get
			{
				if (this.sandboxOption == global::SandboxOptions.SandboxOptions.Max || SandboxOptionManager.GetOptionType(this.sandboxOption) != BaseSandboxOption.OptionTypes.Float)
				{
					return this.prob;
				}
				if (this.invertSandbox)
				{
					return 1f - SandboxOptionManager.GetFloat(this.sandboxOption);
				}
				return SandboxOptionManager.GetFloat(this.sandboxOption);
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0003BD50 File Offset: 0x00039F50
		public PlaceholderTarget(BlockValue _blockValue, float _prob, string _biomeName, bool _isRandomRotation, string _sandboxOption)
		{
			this.blockValue = _blockValue;
			this.prob = _prob;
			this.biomeName = _biomeName;
			this.isRandomRotation = _isRandomRotation;
			this.invertSandbox = false;
			this.sandboxOption = global::SandboxOptions.SandboxOptions.Max;
			if (_sandboxOption != "")
			{
				if (_sandboxOption.StartsWith('!'))
				{
					this.invertSandbox = true;
					_sandboxOption = _sandboxOption.Remove(0, 1);
				}
				this.sandboxOption = Enum.Parse<global::SandboxOptions.SandboxOptions>(_sandboxOption);
			}
		}

		// Token: 0x0400095A RID: 2394
		public readonly BlockValue blockValue;

		// Token: 0x0400095B RID: 2395
		public readonly float prob;

		// Token: 0x0400095C RID: 2396
		public readonly string biomeName;

		// Token: 0x0400095D RID: 2397
		public readonly bool isRandomRotation;

		// Token: 0x0400095E RID: 2398
		public readonly global::SandboxOptions.SandboxOptions sandboxOption;

		// Token: 0x0400095F RID: 2399
		public readonly bool invertSandbox;
	}

	// Token: 0x02000138 RID: 312
	[PublicizedFrom(EAccessModifier.Private)]
	public struct QuestPlaceholderEntry
	{
		// Token: 0x04000960 RID: 2400
		public FastTags<TagGroup.Global> QuestTag;

		// Token: 0x04000961 RID: 2401
		public List<BlockPlaceholderMap.PlaceholderTarget> PlaceholderList;
	}
}
