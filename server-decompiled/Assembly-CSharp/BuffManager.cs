using System;

// Token: 0x0200065C RID: 1628
public class BuffManager
{
	// Token: 0x060034B6 RID: 13494 RVA: 0x0015D7B0 File Offset: 0x0015B9B0
	public static void Cleanup()
	{
		if (BuffManager.Buffs != null)
		{
			BuffManager.Buffs.Clear();
			BuffManager.Buffs = null;
		}
	}

	// Token: 0x060034B7 RID: 13495 RVA: 0x0015D7C9 File Offset: 0x0015B9C9
	public static void AddBuff(BuffClass _buffClass)
	{
		BuffManager.Buffs[_buffClass.Name] = _buffClass;
	}

	// Token: 0x060034B8 RID: 13496 RVA: 0x0015D7DC File Offset: 0x0015B9DC
	public static BuffClass GetBuff(string _name)
	{
		BuffClass buffClass;
		BuffManager.Buffs.TryGetValue(_name, out buffClass);
		return buffClass;
	}

	// Token: 0x040029DD RID: 10717
	public static CaseInsensitiveStringDictionary<BuffClass> Buffs;
}
