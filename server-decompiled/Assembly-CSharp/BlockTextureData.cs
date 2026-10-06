using System;

// Token: 0x0200015A RID: 346
public class BlockTextureData
{
	// Token: 0x06000986 RID: 2438 RVA: 0x00041E02 File Offset: 0x00040002
	public static void InitStatic()
	{
		BlockTextureData.list = new BlockTextureData[256];
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x00041E13 File Offset: 0x00040013
	public void Init()
	{
		BlockTextureData.list[this.ID] = this;
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x00041E22 File Offset: 0x00040022
	public static void Cleanup()
	{
		BlockTextureData.list = null;
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x00041E2C File Offset: 0x0004002C
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

	// Token: 0x0600098A RID: 2442 RVA: 0x00041E6C File Offset: 0x0004006C
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

	// Token: 0x040009CC RID: 2508
	public static BlockTextureData[] list;

	// Token: 0x040009CD RID: 2509
	public int ID;

	// Token: 0x040009CE RID: 2510
	public ushort TextureID;

	// Token: 0x040009CF RID: 2511
	public string Name;

	// Token: 0x040009D0 RID: 2512
	public string LocalizedName;

	// Token: 0x040009D1 RID: 2513
	public string Group;

	// Token: 0x040009D2 RID: 2514
	public ushort PaintCost;

	// Token: 0x040009D3 RID: 2515
	public bool Hidden;

	// Token: 0x040009D4 RID: 2516
	public byte SortIndex = byte.MaxValue;

	// Token: 0x040009D5 RID: 2517
	public string LockedByPerk = "";

	// Token: 0x040009D6 RID: 2518
	public ushort RequiredLevel;
}
