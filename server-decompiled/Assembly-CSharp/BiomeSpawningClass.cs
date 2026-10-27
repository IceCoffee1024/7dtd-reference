using System;

// Token: 0x02000AF8 RID: 2808
public class BiomeSpawningClass
{
	// Token: 0x0600535E RID: 21342 RVA: 0x0020099D File Offset: 0x001FEB9D
	public static void Cleanup()
	{
		BiomeSpawningClass.list.Clear();
	}

	// Token: 0x04004130 RID: 16688
	public static DictionarySave<string, BiomeSpawnEntityGroupList> list = new DictionarySave<string, BiomeSpawnEntityGroupList>();
}
