using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001545 RID: 5445
[Preserve]
public abstract class vp_Event
{
	// Token: 0x170013E1 RID: 5089
	// (get) Token: 0x0600AB09 RID: 43785 RVA: 0x003FCA17 File Offset: 0x003FAC17
	public string EventName
	{
		get
		{
			return this.m_Name;
		}
	}

	// Token: 0x170013E2 RID: 5090
	// (get) Token: 0x0600AB0A RID: 43786 RVA: 0x003FCA1F File Offset: 0x003FAC1F
	[Preserve]
	public Type ArgumentType
	{
		get
		{
			return this.m_ArgumentType;
		}
	}

	// Token: 0x170013E3 RID: 5091
	// (get) Token: 0x0600AB0B RID: 43787 RVA: 0x003FCA27 File Offset: 0x003FAC27
	[Preserve]
	public Type ReturnType
	{
		get
		{
			return this.m_ReturnType;
		}
	}

	// Token: 0x0600AB0C RID: 43788
	[Preserve]
	public abstract void Register(object target, string method, int variant);

	// Token: 0x0600AB0D RID: 43789
	[Preserve]
	public abstract void Unregister(object target);

	// Token: 0x0600AB0E RID: 43790
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void InitFields();

	// Token: 0x0600AB0F RID: 43791 RVA: 0x003FCA2F File Offset: 0x003FAC2F
	[Preserve]
	public vp_Event(string name = "")
	{
		this.m_ArgumentType = this.GetArgumentType;
		this.m_ReturnType = this.GetGenericReturnType;
		this.m_Name = name;
	}

	// Token: 0x0600AB10 RID: 43792 RVA: 0x003FCA58 File Offset: 0x003FAC58
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

	// Token: 0x0600AB11 RID: 43793 RVA: 0x003FCAA0 File Offset: 0x003FACA0
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

	// Token: 0x0600AB12 RID: 43794 RVA: 0x003FCB14 File Offset: 0x003FAD14
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

	// Token: 0x0600AB13 RID: 43795 RVA: 0x003FCB8C File Offset: 0x003FAD8C
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

	// Token: 0x0600AB14 RID: 43796 RVA: 0x003FCC18 File Offset: 0x003FAE18
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

	// Token: 0x0600AB15 RID: 43797 RVA: 0x003FCC8C File Offset: 0x003FAE8C
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

	// Token: 0x0600AB16 RID: 43798 RVA: 0x003FCD54 File Offset: 0x003FAF54
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

	// Token: 0x170013E4 RID: 5092
	// (get) Token: 0x0600AB17 RID: 43799 RVA: 0x003FCE62 File Offset: 0x003FB062
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

	// Token: 0x170013E5 RID: 5093
	// (get) Token: 0x0600AB18 RID: 43800 RVA: 0x003FCE8C File Offset: 0x003FB08C
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

	// Token: 0x0600AB19 RID: 43801 RVA: 0x003FCEDC File Offset: 0x003FB0DC
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

	// Token: 0x0600AB1A RID: 43802 RVA: 0x003FCFA8 File Offset: 0x003FB1A8
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

	// Token: 0x0600AB1B RID: 43803 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Refresh()
	{
	}

	// Token: 0x04007EFE RID: 32510
	[PublicizedFrom(EAccessModifier.Protected)]
	public string m_Name;

	// Token: 0x04007EFF RID: 32511
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public Type m_ArgumentType;

	// Token: 0x04007F00 RID: 32512
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public Type m_ReturnType;

	// Token: 0x04007F01 RID: 32513
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public FieldInfo[] m_Fields;

	// Token: 0x04007F02 RID: 32514
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public Type[] m_DelegateTypes;

	// Token: 0x04007F03 RID: 32515
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public MethodInfo[] m_DefaultMethods;

	// Token: 0x04007F04 RID: 32516
	[Preserve]
	public string[] InvokerFieldNames;

	// Token: 0x04007F05 RID: 32517
	[Preserve]
	public Dictionary<string, int> Prefixes;
}
