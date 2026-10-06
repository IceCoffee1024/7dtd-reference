using System;

// Token: 0x02000655 RID: 1621
public class BuffManager
{
	// Token: 0x060034A8 RID: 13480 RVA: 0x0015CBBC File Offset: 0x0015ADBC
	public static void Cleanup()
	{
		if (BuffManager.Buffs != null)
		{
			BuffManager.Buffs.Clear();
			BuffManager.Buffs = null;
		}
	}

	// Token: 0x060034A9 RID: 13481 RVA: 0x0015CBD5 File Offset: 0x0015ADD5
	public static void AddBuff(BuffClass _buffClass)
	{
		BuffManager.Buffs[_buffClass.Name] = _buffClass;
	}

	// Token: 0x060034AA RID: 13482 RVA: 0x0015CBE8 File Offset: 0x0015ADE8
	public static BuffClass GetBuff(string _name)
	{
		BuffClass buffClass;
		BuffManager.Buffs.TryGetValue(_name, out buffClass);
		return buffClass;
	}

	// Token: 0x040029C2 RID: 10690
	public static CaseInsensitiveStringDictionary<BuffClass> Buffs;
}
