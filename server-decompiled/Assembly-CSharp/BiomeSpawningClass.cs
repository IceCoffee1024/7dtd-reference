using System;

// Token: 0x02000AED RID: 2797
public class BiomeSpawningClass
{
	// Token: 0x06005339 RID: 21305 RVA: 0x001FF419 File Offset: 0x001FD619
	public static void Cleanup()
	{
		BiomeSpawningClass.list.Clear();
	}

	// Token: 0x0400410B RID: 16651
	public static DictionarySave<string, BiomeSpawnEntityGroupList> list = new DictionarySave<string, BiomeSpawnEntityGroupList>();
}
