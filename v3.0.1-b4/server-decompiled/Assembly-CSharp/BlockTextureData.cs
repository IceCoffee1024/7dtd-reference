using System;

// Token: 0x02000157 RID: 343
public class BlockTextureData
{
	// Token: 0x06000976 RID: 2422 RVA: 0x00041AC2 File Offset: 0x0003FCC2
	public static void InitStatic()
	{
		BlockTextureData.list = new BlockTextureData[256];
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x00041AD3 File Offset: 0x0003FCD3
	public void Init()
	{
		BlockTextureData.list[this.ID] = this;
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x00041AE2 File Offset: 0x0003FCE2
	public static void Cleanup()
	{
		BlockTextureData.list = null;
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x00041AEC File Offset: 0x0003FCEC
	public static BlockTextureData GetDataByTextureID(int textureID)
	{
		for (int i = 0; i < BlockTextureData.list.Length; i++)
		{
			if (BlockTextureData.list[i] != null && (int)BlockTextureData.list[i].TextureID == textureID)
			{
				return BlockTextureData.list[i];
			}
		}
		return null;
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00041B2C File Offset: 0x0003FD2C
	public bool GetLocked(EntityPlayerLocal player)
	{
		if (this.LockedByPerk != "")
		{
			ProgressionValue progressionValue = player.Progression.GetProgressionValue(this.LockedByPerk);
			if (progressionValue != null && progressionValue.CalculatedLevel(player) >= (int)this.RequiredLevel)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040009C7 RID: 2503
	public static BlockTextureData[] list;

	// Token: 0x040009C8 RID: 2504
	public int ID;

	// Token: 0x040009C9 RID: 2505
	public ushort TextureID;

	// Token: 0x040009CA RID: 2506
	public string Name;

	// Token: 0x040009CB RID: 2507
	public string LocalizedName;

	// Token: 0x040009CC RID: 2508
	public string Group;

	// Token: 0x040009CD RID: 2509
	public ushort PaintCost;

	// Token: 0x040009CE RID: 2510
	public bool Hidden;

	// Token: 0x040009CF RID: 2511
	public byte SortIndex = byte.MaxValue;

	// Token: 0x040009D0 RID: 2512
	public string LockedByPerk = "";

	// Token: 0x040009D1 RID: 2513
	public ushort RequiredLevel;
}
