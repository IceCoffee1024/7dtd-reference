using System;

// Token: 0x020013CD RID: 5069
public abstract class DynamicClassFactory
{
	// Token: 0x06009EE9 RID: 40681
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract object[] getTable();

	// Token: 0x06009EEA RID: 40682 RVA: 0x003BD064 File Offset: 0x003BB264
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

	// Token: 0x06009EEB RID: 40683 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicClassFactory()
	{
	}
}
