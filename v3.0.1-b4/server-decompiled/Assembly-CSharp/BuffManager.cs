using System;

// Token: 0x0200064A RID: 1610
public class BuffManager
{
	// Token: 0x06003412 RID: 13330 RVA: 0x0015A130 File Offset: 0x00158330
	public static void Cleanup()
	{
		if (BuffManager.Buffs != null)
		{
			BuffManager.Buffs.Clear();
			BuffManager.Buffs = null;
		}
	}

	// Token: 0x06003413 RID: 13331 RVA: 0x0015A149 File Offset: 0x00158349
	public static void AddBuff(BuffClass _buffClass)
	{
		BuffManager.Buffs[_buffClass.Name] = _buffClass;
	}

	// Token: 0x06003414 RID: 13332 RVA: 0x0015A15C File Offset: 0x0015835C
	public static BuffClass GetBuff(string _name)
	{
		BuffClass buffClass;
		BuffManager.Buffs.TryGetValue(_name, out buffClass);
		return buffClass;
	}

	// Token: 0x04002956 RID: 10582
	public static CaseInsensitiveStringDictionary<BuffClass> Buffs;
}
