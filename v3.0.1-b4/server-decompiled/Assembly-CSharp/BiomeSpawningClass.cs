using System;

// Token: 0x02000AC9 RID: 2761
public class BiomeSpawningClass
{
	// Token: 0x0600529A RID: 21146 RVA: 0x001F9721 File Offset: 0x001F7921
	public static void Cleanup()
	{
		BiomeSpawningClass.list.Clear();
	}

	// Token: 0x04004039 RID: 16441
	public static DictionarySave<string, BiomeSpawnEntityGroupList> list = new DictionarySave<string, BiomeSpawnEntityGroupList>();
}
