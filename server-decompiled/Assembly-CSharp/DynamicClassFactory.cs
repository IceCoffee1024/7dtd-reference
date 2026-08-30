using System;

// Token: 0x020013E8 RID: 5096
public abstract class DynamicClassFactory
{
	// Token: 0x06009FE6 RID: 40934
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract object[] getTable();

	// Token: 0x06009FE7 RID: 40935 RVA: 0x003C23A8 File Offset: 0x003C05A8
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

	// Token: 0x06009FE8 RID: 40936 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicClassFactory()
	{
	}
}
