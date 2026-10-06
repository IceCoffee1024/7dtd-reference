using System;

// Token: 0x02001408 RID: 5128
public abstract class DynamicClassFactory
{
	// Token: 0x0600A091 RID: 41105
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract object[] getTable();

	// Token: 0x0600A092 RID: 41106 RVA: 0x003C6F44 File Offset: 0x003C5144
	public object Instantiate(string _className)
	{
		Type type = Type.GetType(_className);
		if (type != null)
		{
			return Activator.CreateInstance(type);
		}
		object[] table = this.getTable();
		for (int i = 0; i < table.Length / 2; i++)
		{
			if (_className.Equals(Utils.UnCryptFromBase64((char[])table[i * 2 + 1])))
			{
				try
				{
					return Activator.CreateInstance((Type)table[i * 2]);
				}
				catch (Exception ex)
				{
					throw new Exception("Class '" + Utils.UnCryptFromBase64((char[])table[i * 2 + 1]) + "' not found! Msg: " + ex.Message);
				}
			}
		}
		return null;
	}

	// Token: 0x0600A093 RID: 41107 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicClassFactory()
	{
	}
}
