using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001579 RID: 5497
[Preserve]
public abstract class vp_Event
{
	// Token: 0x17001432 RID: 5170
	// (get) Token: 0x0600AC14 RID: 44052 RVA: 0x00403C6F File Offset: 0x00401E6F
	public string EventName
	{
		get
		{
			return this.m_Name;
		}
	}

	// Token: 0x17001433 RID: 5171
	// (get) Token: 0x0600AC15 RID: 44053 RVA: 0x00403C77 File Offset: 0x00401E77
	[Preserve]
	public Type ArgumentType
	{
		get
		{
			return this.m_ArgumentType;
		}
	}

	// Token: 0x17001434 RID: 5172
	// (get) Token: 0x0600AC16 RID: 44054 RVA: 0x00403C7F File Offset: 0x00401E7F
	[Preserve]
	public Type ReturnType
	{
		get
		{
			return this.m_ReturnType;
		}
	}

	// Token: 0x0600AC17 RID: 44055
	[Preserve]
	public abstract void Register(object target, string method, int variant);

	// Token: 0x0600AC18 RID: 44056
	[Preserve]
	public abstract void Unregister(object target);

	// Token: 0x0600AC19 RID: 44057
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void InitFields();

	// Token: 0x0600AC1A RID: 44058 RVA: 0x00403C87 File Offset: 0x00401E87
	[Preserve]
	public vp_Event(string name = "")
	{
		this.m_ArgumentType = this.GetArgumentType;
		this.m_ReturnType = this.GetGenericReturnType;
		this.m_Name = name;
	}

	// Token: 0x0600AC1B RID: 44059 RVA: 0x00403CB0 File Offset: 0x00401EB0
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void StoreInvokerFieldNames()
	{
		this.InvokerFieldNames = new string[this.m_Fields.Length];
		for (int i = 0; i < this.m_Fields.Length; i++)
		{
			this.InvokerFieldNames[i] = this.m_Fields[i].Name;
		}
	}

	// Token: 0x0600AC1C RID: 44060 RVA: 0x00403CF8 File Offset: 0x00401EF8
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public Type MakeGenericType(Type type)
	{
		if (this.m_ReturnType == typeof(void))
		{
			return type.MakeGenericType(new Type[] { this.m_ArgumentType, this.m_ArgumentType });
		}
		return type.MakeGenericType(new Type[] { this.m_ArgumentType, this.m_ReturnType, this.m_ArgumentType, this.m_ReturnType });
	}

	// Token: 0x0600AC1D RID: 44061 RVA: 0x00403D6C File Offset: 0x00401F6C
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetFieldToExternalMethod(object target, FieldInfo field, string method, Type type)
	{
		Delegate @delegate = Delegate.CreateDelegate(type, target, method, false, false);
		if (@delegate == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Error (",
				(this != null) ? this.ToString() : null,
				") Failed to bind: ",
				(target != null) ? target.ToString() : null,
				" -> ",
				method,
				"."
			}));
			return;
		}
		field.SetValue(this, @delegate);
	}

	// Token: 0x0600AC1E RID: 44062 RVA: 0x00403DE4 File Offset: 0x00401FE4
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void AddExternalMethodToField(object target, FieldInfo field, string method, Type type)
	{
		Delegate @delegate = Delegate.Combine((Delegate)field.GetValue(this), Delegate.CreateDelegate(type, target, method, false, false));
		if (@delegate == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Error (",
				(this != null) ? this.ToString() : null,
				") Failed to bind: ",
				(target != null) ? target.ToString() : null,
				" -> ",
				method,
				"."
			}));
			return;
		}
		field.SetValue(this, @delegate);
	}

	// Token: 0x0600AC1F RID: 44063 RVA: 0x00403E70 File Offset: 0x00402070
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetFieldToLocalMethod(FieldInfo field, MethodInfo method, Type type)
	{
		if (method == null)
		{
			return;
		}
		Delegate @delegate = Delegate.CreateDelegate(type, method);
		if (@delegate == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Error (",
				(this != null) ? this.ToString() : null,
				") Failed to bind: ",
				(method != null) ? method.ToString() : null,
				"."
			}));
			return;
		}
		field.SetValue(this, @delegate);
	}

	// Token: 0x0600AC20 RID: 44064 RVA: 0x00403EE4 File Offset: 0x004020E4
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void RemoveExternalMethodFromField(object target, FieldInfo field)
	{
		List<Delegate> list = new List<Delegate>(((Delegate)field.GetValue(this)).GetInvocationList());
		if (list == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Error (",
				(this != null) ? this.ToString() : null,
				") Failed to remove: ",
				(target != null) ? target.ToString() : null,
				" -> ",
				field.Name,
				"."
			}));
			return;
		}
		for (int i = list.Count - 1; i > -1; i--)
		{
			if (list[i].Target == target)
			{
				list.Remove(list[i]);
			}
		}
		if (list != null)
		{
			field.SetValue(this, Delegate.Combine(list.ToArray()));
		}
	}

	// Token: 0x0600AC21 RID: 44065 RVA: 0x00403FAC File Offset: 0x004021AC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public MethodInfo GetStaticGenericMethod(Type e, string name, Type parameterType, Type returnType)
	{
		foreach (MethodInfo methodInfo in e.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			if (!(methodInfo == null) && !(methodInfo.Name != name))
			{
				MethodInfo methodInfo2;
				if (this.GetGenericReturnType == typeof(void))
				{
					methodInfo2 = methodInfo.MakeGenericMethod(new Type[] { this.m_ArgumentType });
				}
				else
				{
					methodInfo2 = methodInfo.MakeGenericMethod(new Type[] { this.m_ArgumentType, this.m_ReturnType });
				}
				if (methodInfo2.GetParameters().Length <= 1 && (methodInfo2.GetParameters().Length != 1 || !(parameterType == typeof(void))) && (methodInfo2.GetParameters().Length != 0 || !(parameterType != typeof(void))) && (methodInfo2.GetParameters().Length != 1 || !(methodInfo2.GetParameters()[0].ParameterType != parameterType)) && !(returnType != methodInfo2.ReturnType))
				{
					return methodInfo2;
				}
			}
		}
		return null;
	}

	// Token: 0x17001435 RID: 5173
	// (get) Token: 0x0600AC22 RID: 44066 RVA: 0x004040BA File Offset: 0x004022BA
	[Preserve]
	public Type GetArgumentType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!base.GetType().IsGenericType)
			{
				return typeof(void);
			}
			return base.GetType().GetGenericArguments()[0];
		}
	}

	// Token: 0x17001436 RID: 5174
	// (get) Token: 0x0600AC23 RID: 44067 RVA: 0x004040E4 File Offset: 0x004022E4
	[Preserve]
	public Type GetGenericReturnType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!base.GetType().IsGenericType)
			{
				return typeof(void);
			}
			if (base.GetType().GetGenericArguments().Length != 2)
			{
				return typeof(void);
			}
			return base.GetType().GetGenericArguments()[1];
		}
	}

	// Token: 0x0600AC24 RID: 44068 RVA: 0x00404134 File Offset: 0x00402334
	[Preserve]
	public Type GetParameterType(int index)
	{
		if (!base.GetType().IsGenericType)
		{
			return typeof(void);
		}
		if (index > this.m_Fields.Length - 1)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Error: (",
				(this != null) ? this.ToString() : null,
				") Event '",
				this.EventName,
				"' only supports ",
				this.m_Fields.Length.ToString(),
				" indices. 'GetParameterType' referenced index ",
				index.ToString(),
				"."
			}));
		}
		if (this.m_DelegateTypes[index].GetMethod("Invoke").GetParameters().Length == 0)
		{
			return typeof(void);
		}
		return this.m_ArgumentType;
	}

	// Token: 0x0600AC25 RID: 44069 RVA: 0x00404200 File Offset: 0x00402400
	[Preserve]
	public Type GetReturnType(int index)
	{
		if (index > this.m_Fields.Length - 1)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Error: (",
				(this != null) ? this.ToString() : null,
				") Event '",
				this.EventName,
				"' only supports ",
				this.m_Fields.Length.ToString(),
				" indices. 'GetReturnType' referenced index ",
				index.ToString(),
				"."
			}));
			return null;
		}
		if (base.GetType().GetGenericArguments().Length > 1)
		{
			return this.GetGenericReturnType;
		}
		Type returnType = this.m_DelegateTypes[index].GetMethod("Invoke").ReturnType;
		if (returnType.IsGenericParameter)
		{
			return this.m_ArgumentType;
		}
		return returnType;
	}

	// Token: 0x0600AC26 RID: 44070 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Refresh()
	{
	}

	// Token: 0x04007FC2 RID: 32706
	[PublicizedFrom(EAccessModifier.Protected)]
	public string m_Name;

	// Token: 0x04007FC3 RID: 32707
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public Type m_ArgumentType;

	// Token: 0x04007FC4 RID: 32708
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public Type m_ReturnType;

	// Token: 0x04007FC5 RID: 32709
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public FieldInfo[] m_Fields;

	// Token: 0x04007FC6 RID: 32710
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public Type[] m_DelegateTypes;

	// Token: 0x04007FC7 RID: 32711
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public MethodInfo[] m_DefaultMethods;

	// Token: 0x04007FC8 RID: 32712
	[Preserve]
	public string[] InvokerFieldNames;

	// Token: 0x04007FC9 RID: 32713
	[Preserve]
	public Dictionary<string, int> Prefixes;
}
