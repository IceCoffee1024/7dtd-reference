using System;

// Token: 0x0200141A RID: 5146
public abstract class DynamicClassFactory
{
	// Token: 0x0600A0E9 RID: 41193
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract object[] getTable();

	// Token: 0x0600A0EA RID: 41194 RVA: 0x003C9348 File Offset: 0x003C7548
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

	// Token: 0x0600A0EB RID: 41195 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicClassFactory()
	{
	}
}
