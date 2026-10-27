using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class WaterEvaporationManager : MonoBehaviour
{
	// Token: 0x06000C58 RID: 3160 RVA: 0x000027FC File Offset: 0x000009FC
	public void Start()
	{
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x000027FC File Offset: 0x000009FC
	public void Update()
	{
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x0004CB0C File Offset: 0x0004AD0C
	public static void UpdateEvaporation()
	{
		if (GameManager.Instance == null)
		{
			return;
		}
		if (GameManager.Instance.World == null)
		{
			return;
		}
		WaterEvaporationManager.vRemovalList.Clear();
		WaterEvaporationManager.uRemovalList.Clear();
		DictionaryList<ulong, WaterEvaporationManager.BlockData> dictionaryList = WaterEvaporationManager.evapWalkList;
		lock (dictionaryList)
		{
			int num = WaterEvaporationManager.evapWalkIndex;
			while (num < WaterEvaporationManager.evapWalkList.Count && num < WaterEvaporationManager.evapWalkIndex + 15)
			{
				if (GameManager.Instance == null)
				{
					return;
				}
				if (GameManager.Instance.World == null)
				{
					return;
				}
				if (GameManager.Instance.World.GetWorldTime() > WaterEvaporationManager.evapWalkList.list[num].time + 1000UL)
				{
					WaterEvaporationManager.vRemovalList.Add(WaterEvaporationManager.evapWalkList.list[num].pos);
					WaterEvaporationManager.uRemovalList.Add(WaterEvaporationManager.evapWalkList.list[num].ID);
					GameManager.Instance.World.SetBlockRPC(WaterEvaporationManager.evapWalkList.list[num].pos, BlockValue.Air);
					if (WaterEvaporationManager.waterBlockID < 0)
					{
						WaterEvaporationManager.waterBlockID = ItemClass.GetItem("Water", false).ToBlockValue(false).type;
					}
					GameManager.Instance.World.GetWBT().AddScheduledBlockUpdate(WaterEvaporationManager.evapWalkList.list[num].pos, WaterEvaporationManager.waterBlockID, 1UL);
				}
				num++;
			}
			num = 0;
			for (int i = 0; i < WaterEvaporationManager.vRemovalList.Count; i++)
			{
				Vector3i vector3i = WaterEvaporationManager.vRemovalList[i];
				WaterEvaporationManager.evapWalkList.Remove(WaterEvaporationManager.uRemovalList[num++]);
				WaterEvaporationManager.RemoveFromEvapList(vector3i);
			}
			WaterEvaporationManager.evapWalkIndex += 15;
			if (WaterEvaporationManager.evapWalkIndex >= WaterEvaporationManager.evapWalkList.Count)
			{
				WaterEvaporationManager.evapWalkIndex = 0;
			}
		}
		WaterEvaporationManager.vRemovalList.Clear();
		WaterEvaporationManager.uRemovalList.Clear();
		dictionaryList = WaterEvaporationManager.restWalkList;
		lock (dictionaryList)
		{
			int num2 = WaterEvaporationManager.restWalkIndex;
			while (num2 < WaterEvaporationManager.restWalkList.Count && num2 < WaterEvaporationManager.restWalkIndex + 15)
			{
				if (GameManager.Instance == null)
				{
					return;
				}
				if (GameManager.Instance.World == null)
				{
					return;
				}
				if (GameManager.Instance.World.GetWorldTime() > WaterEvaporationManager.restWalkList.list[num2].time + 1000UL)
				{
					WaterEvaporationManager.vRemovalList.Add(WaterEvaporationManager.restWalkList.list[num2].pos);
					WaterEvaporationManager.uRemovalList.Add(WaterEvaporationManager.restWalkList.list[num2].ID);
					BlockValue block = GameManager.Instance.World.GetBlock(WaterEvaporationManager.restWalkList.list[num2].pos.x, WaterEvaporationManager.restWalkList.list[num2].pos.y, WaterEvaporationManager.restWalkList.list[num2].pos.z);
					BlockLiquidv2.SetBlockState(ref block, BlockLiquidv2.UpdateID.Evaporate);
					GameManager.Instance.World.SetBlockRPC(WaterEvaporationManager.restWalkList.list[num2].pos, block);
					if (WaterEvaporationManager.waterBlockID < 0)
					{
						WaterEvaporationManager.waterBlockID = ItemClass.GetItem("Water", false).ToBlockValue(false).type;
					}
					GameManager.Instance.World.GetWBT().AddScheduledBlockUpdate(WaterEvaporationManager.restWalkList.list[num2].pos, WaterEvaporationManager.waterBlockID, 1UL);
				}
				num2++;
			}
			num2 = 0;
			for (int j = 0; j < WaterEvaporationManager.vRemovalList.Count; j++)
			{
				Vector3i vector3i2 = WaterEvaporationManager.vRemovalList[j];
				WaterEvaporationManager.restWalkList.Remove(WaterEvaporationManager.uRemovalList[num2++]);
				WaterEvaporationManager.RemoveFromRestList(vector3i2);
			}
			WaterEvaporationManager.restWalkIndex += 15;
			if (WaterEvaporationManager.restWalkIndex >= WaterEvaporationManager.restWalkList.Count)
			{
				WaterEvaporationManager.restWalkIndex = 0;
			}
		}
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x0004CF94 File Offset: 0x0004B194
	public static void ClearAll()
	{
		DictionaryList<int, DictionaryList<int, DictionaryList<int, WaterEvaporationManager.BlockData>>> dictionaryList = WaterEvaporationManager.restingList;
		lock (dictionaryList)
		{
			WaterEvaporationManager.restingList.Clear();
		}
		dictionaryList = WaterEvaporationManager.evaporationList;
		lock (dictionaryList)
		{
			WaterEvaporationManager.evaporationList.Clear();
		}
		DictionaryList<ulong, WaterEvaporationManager.BlockData> dictionaryList2 = WaterEvaporationManager.evapWalkList;
		lock (dictionaryList2)
		{
			WaterEvaporationManager.evapWalkList.Clear();
		}
		dictionaryList2 = WaterEvaporationManager.restWalkList;
		lock (dictionaryList2)
		{
			WaterEvaporationManager.restWalkList.Clear();
		}
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x0004D070 File Offset: 0x0004B270
	public static void AddToRestList(Vector3i _blockPos)
	{
		DictionaryList<int, DictionaryList<int, DictionaryList<int, WaterEvaporationManager.BlockData>>> dictionaryList = WaterEvaporationManager.restingList;
		lock (dictionaryList)
		{
			DictionaryList<ulong, WaterEvaporationManager.BlockData> dictionaryList2 = WaterEvaporationManager.restWalkList;
			lock (dictionaryList2)
			{
				if (WaterEvaporationManager.restingList.dict.ContainsKey(_blockPos.x))
				{
					if (WaterEvaporationManager.restingList.dict[_blockPos.x].dict.ContainsKey(_blockPos.y))
					{
						if (WaterEvaporationManager.restingList.dict[_blockPos.x].dict[_blockPos.y].dict.ContainsKey(_blockPos.z))
						{
							WaterEvaporationManager.BlockData blockData = new WaterEvaporationManager.BlockData(_blockPos);
							WaterEvaporationManager.restingList.dict[_blockPos.x].dict[_blockPos.y].dict[_blockPos.z] = blockData;
							WaterEvaporationManager.restWalkList.Add(blockData.ID, blockData);
						}
						else
						{
							WaterEvaporationManager.BlockData blockData2 = new WaterEvaporationManager.BlockData(_blockPos);
							WaterEvaporationManager.restingList.dict[_blockPos.x].dict[_blockPos.y].Add(_blockPos.z, blockData2);
							WaterEvaporationManager.restWalkList.Add(blockData2.ID, blockData2);
						}
					}
					else
					{
						WaterEvaporationManager.BlockData blockData3 = new WaterEvaporationManager.BlockData(_blockPos);
						WaterEvaporationManager.restingList.dict[_blockPos.x].Add(_blockPos.y, new DictionaryList<int, WaterEvaporationManager.BlockData>());
						WaterEvaporationManager.restingList.dict[_blockPos.x].dict[_blockPos.y].Add(_blockPos.z, blockData3);
						WaterEvaporationManager.restWalkList.Add(blockData3.ID, blockData3);
					}
				}
				else
				{
					WaterEvaporationManager.BlockData blockData4 = new WaterEvaporationManager.BlockData(_blockPos);
					WaterEvaporationManager.restingList.Add(_blockPos.x, new DictionaryList<int, DictionaryList<int, WaterEvaporationManager.BlockData>>());
					WaterEvaporationManager.restingList.dict[_blockPos.x].Add(_blockPos.y, new DictionaryList<int, WaterEvaporationManager.BlockData>());
					WaterEvaporationManager.restingList.dict[_blockPos.x].dict[_blockPos.y].Add(_blockPos.z, blockData4);
					WaterEvaporationManager.restWalkList.Add(blockData4.ID, blockData4);
				}
			}
		}
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x0004D30C File Offset: 0x0004B50C
	public static void RemoveFromRestList(Vector3i _blockPos)
	{
		DictionaryList<int, DictionaryList<int, DictionaryList<int, WaterEvaporationManager.BlockData>>> dictionaryList = WaterEvaporationManager.restingList;
		lock (dictionaryList)
		{
			if (WaterEvaporationManager.restingList.dict.ContainsKey(_blockPos.x) && WaterEvaporationManager.restingList.dict[_blockPos.x].dict.ContainsKey(_blockPos.y) && WaterEvaporationManager.restingList.dict[_blockPos.x].dict[_blockPos.y].dict.ContainsKey(_blockPos.z))
			{
				DictionaryList<ulong, WaterEvaporationManager.BlockData> dictionaryList2 = WaterEvaporationManager.restWalkList;
				lock (dictionaryList2)
				{
					WaterEvaporationManager.restWalkList.Remove(WaterEvaporationManager.restingList.dict[_blockPos.x].dict[_blockPos.y].dict[_blockPos.z].ID);
				}
				WaterEvaporationManager.restingList.dict[_blockPos.x].dict[_blockPos.y].Remove(_blockPos.z);
			}
		}
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x0004D47C File Offset: 0x0004B67C
	public static ulong GetRestTime(Vector3i _blockPos)
	{
		DictionaryList<int, DictionaryList<int, DictionaryList<int, WaterEvaporationManager.BlockData>>> dictionaryList = WaterEvaporationManager.restingList;
		lock (dictionaryList)
		{
			if (WaterEvaporationManager.restingList.dict.ContainsKey(_blockPos.x) && WaterEvaporationManager.restingList.dict[_blockPos.x].dict.ContainsKey(_blockPos.y) && WaterEvaporationManager.restingList.dict[_blockPos.x].dict[_blockPos.y].dict.ContainsKey(_blockPos.z))
			{
				return WaterEvaporationManager.restingList.dict[_blockPos.x].dict[_blockPos.y].dict[_blockPos.z].time;
			}
		}
		return 0UL;
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x0004D570 File Offset: 0x0004B770
	public static void AddToEvapList(Vector3i _blockPos)
	{
		DictionaryList<int, DictionaryList<int, DictionaryList<int, WaterEvaporationManager.BlockData>>> dictionaryList = WaterEvaporationManager.evaporationList;
		lock (dictionaryList)
		{
			DictionaryList<ulong, WaterEvaporationManager.BlockData> dictionaryList2 = WaterEvaporationManager.evapWalkList;
			lock (dictionaryList2)
			{
				if (WaterEvaporationManager.evaporationList.dict.ContainsKey(_blockPos.x))
				{
					if (WaterEvaporationManager.evaporationList.dict[_blockPos.x].dict.ContainsKey(_blockPos.y))
					{
						if (WaterEvaporationManager.evaporationList.dict[_blockPos.x].dict[_blockPos.y].dict.ContainsKey(_blockPos.z))
						{
							WaterEvaporationManager.BlockData blockData = new WaterEvaporationManager.BlockData(_blockPos);
							WaterEvaporationManager.evaporationList.dict[_blockPos.x].dict[_blockPos.y].dict[_blockPos.z] = blockData;
							WaterEvaporationManager.evapWalkList.Add(blockData.ID, blockData);
						}
						else
						{
							WaterEvaporationManager.BlockData blockData2 = new WaterEvaporationManager.BlockData(_blockPos);
							WaterEvaporationManager.evaporationList.dict[_blockPos.x].dict[_blockPos.y].Add(_blockPos.z, blockData2);
							WaterEvaporationManager.evapWalkList.Add(blockData2.ID, blockData2);
						}
					}
					else
					{
						WaterEvaporationManager.BlockData blockData3 = new WaterEvaporationManager.BlockData(_blockPos);
						WaterEvaporationManager.evaporationList.dict[_blockPos.x].Add(_blockPos.y, new DictionaryList<int, WaterEvaporationManager.BlockData>());
						WaterEvaporationManager.evaporationList.dict[_blockPos.x].dict[_blockPos.y].Add(_blockPos.z, blockData3);
						WaterEvaporationManager.evapWalkList.Add(blockData3.ID, blockData3);
					}
				}
				else
				{
					WaterEvaporationManager.BlockData blockData4 = new WaterEvaporationManager.BlockData(_blockPos);
					WaterEvaporationManager.evaporationList.Add(_blockPos.x, new DictionaryList<int, DictionaryList<int, WaterEvaporationManager.BlockData>>());
					WaterEvaporationManager.evaporationList.dict[_blockPos.x].Add(_blockPos.y, new DictionaryList<int, WaterEvaporationManager.BlockData>());
					WaterEvaporationManager.evaporationList.dict[_blockPos.x].dict[_blockPos.y].Add(_blockPos.z, blockData4);
					WaterEvaporationManager.evapWalkList.Add(blockData4.ID, blockData4);
				}
			}
		}
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x0004D80C File Offset: 0x0004BA0C
	public static void RemoveFromEvapList(Vector3i _blockPos)
	{
		DictionaryList<int, DictionaryList<int, DictionaryList<int, WaterEvaporationManager.BlockData>>> dictionaryList = WaterEvaporationManager.evaporationList;
		lock (dictionaryList)
		{
			if (WaterEvaporationManager.evaporationList.dict.ContainsKey(_blockPos.x) && WaterEvaporationManager.evaporationList.dict[_blockPos.x].dict.ContainsKey(_blockPos.y) && WaterEvaporationManager.evaporationList.dict[_blockPos.x].dict[_blockPos.y].dict.ContainsKey(_blockPos.z))
			{
				DictionaryList<ulong, WaterEvaporationManager.BlockData> dictionaryList2 = WaterEvaporationManager.evapWalkList;
				lock (dictionaryList2)
				{
					WaterEvaporationManager.evapWalkList.Remove(WaterEvaporationManager.evaporationList.dict[_blockPos.x].dict[_blockPos.y].dict[_blockPos.z].ID);
				}
				WaterEvaporationManager.evaporationList.dict[_blockPos.x].dict[_blockPos.y].Remove(_blockPos.z);
			}
		}
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x0004D97C File Offset: 0x0004BB7C
	public static ulong GetEvapTime(Vector3i _blockPos)
	{
		DictionaryList<int, DictionaryList<int, DictionaryList<int, WaterEvaporationManager.BlockData>>> dictionaryList = WaterEvaporationManager.evaporationList;
		lock (dictionaryList)
		{
			if (WaterEvaporationManager.evaporationList.dict.ContainsKey(_blockPos.x) && WaterEvaporationManager.evaporationList.dict[_blockPos.x].dict.ContainsKey(_blockPos.y) && WaterEvaporationManager.evaporationList.dict[_blockPos.x].dict[_blockPos.y].dict.ContainsKey(_blockPos.z))
			{
				return WaterEvaporationManager.evaporationList.dict[_blockPos.x].dict[_blockPos.y].dict[_blockPos.z].time;
			}
		}
		return 0UL;
	}

	// Token: 0x04000B18 RID: 2840
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int evapWalkIndex = 0;

	// Token: 0x04000B19 RID: 2841
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static ulong uniqueIndex = 0UL;

	// Token: 0x04000B1A RID: 2842
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int restWalkIndex = 0;

	// Token: 0x04000B1B RID: 2843
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static DictionaryList<ulong, WaterEvaporationManager.BlockData> evapWalkList = new DictionaryList<ulong, WaterEvaporationManager.BlockData>();

	// Token: 0x04000B1C RID: 2844
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static DictionaryList<int, DictionaryList<int, DictionaryList<int, WaterEvaporationManager.BlockData>>> evaporationList = new DictionaryList<int, DictionaryList<int, DictionaryList<int, WaterEvaporationManager.BlockData>>>();

	// Token: 0x04000B1D RID: 2845
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static DictionaryList<ulong, WaterEvaporationManager.BlockData> restWalkList = new DictionaryList<ulong, WaterEvaporationManager.BlockData>();

	// Token: 0x04000B1E RID: 2846
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static DictionaryList<int, DictionaryList<int, DictionaryList<int, WaterEvaporationManager.BlockData>>> restingList = new DictionaryList<int, DictionaryList<int, DictionaryList<int, WaterEvaporationManager.BlockData>>>();

	// Token: 0x04000B1F RID: 2847
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int waterBlockID = -1;

	// Token: 0x04000B20 RID: 2848
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<Vector3i> vRemovalList = new List<Vector3i>();

	// Token: 0x04000B21 RID: 2849
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<ulong> uRemovalList = new List<ulong>();

	// Token: 0x0200019A RID: 410
	[PublicizedFrom(EAccessModifier.Private)]
	public class BlockData
	{
		// Token: 0x06000C64 RID: 3172 RVA: 0x0004DAD2 File Offset: 0x0004BCD2
		public BlockData(Vector3i _pos)
		{
			this.time = GameManager.Instance.World.GetWorldTime();
			ulong uniqueIndex = WaterEvaporationManager.uniqueIndex;
			WaterEvaporationManager.uniqueIndex = uniqueIndex + 1UL;
			this.ID = uniqueIndex;
			this.pos = _pos;
		}

		// Token: 0x04000B22 RID: 2850
		public ulong time;

		// Token: 0x04000B23 RID: 2851
		public ulong ID;

		// Token: 0x04000B24 RID: 2852
		public Vector3i pos;
	}
}
