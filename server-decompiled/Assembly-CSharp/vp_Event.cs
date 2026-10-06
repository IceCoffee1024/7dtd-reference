using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001567 RID: 5479
[Preserve]
public abstract class vp_Event
{
	// Token: 0x17001426 RID: 5158
	// (get) Token: 0x0600ABBC RID: 43964 RVA: 0x0040186B File Offset: 0x003FFA6B
	public string EventName
	{
		get
		{
			return this.m_Name;
		}
	}

	// Token: 0x17001427 RID: 5159
	// (get) Token: 0x0600ABBD RID: 43965 RVA: 0x00401873 File Offset: 0x003FFA73
	[Preserve]
	public Type ArgumentType
	{
		get
		{
			return this.m_ArgumentType;
		}
	}

	// Token: 0x17001428 RID: 5160
	// (get) Token: 0x0600ABBE RID: 43966 RVA: 0x0040187B File Offset: 0x003FFA7B
	[Preserve]
	public Type ReturnType
	{
		get
		{
			return this.m_ReturnType;
		}
	}

	// Token: 0x0600ABBF RID: 43967
	[Preserve]
	public abstract void Register(object target, string method, int variant);

	// Token: 0x0600ABC0 RID: 43968
	[Preserve]
	public abstract void Unregister(object target);

	// Token: 0x0600ABC1 RID: 43969
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void InitFields();

	// Token: 0x0600ABC2 RID: 43970 RVA: 0x00401883 File Offset: 0x003FFA83
	[Preserve]
	public vp_Event(string name = "")
	{
		this.m_ArgumentType = this.GetArgumentType;
		this.m_ReturnType = this.GetGenericReturnType;
		this.m_Name = name;
	}

	// Token: 0x0600ABC3 RID: 43971 RVA: 0x004018AC File Offset: 0x003FFAAC
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

	// Token: 0x0600ABC4 RID: 43972 RVA: 0x004018F4 File Offset: 0x003FFAF4
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

	// Token: 0x0600ABC5 RID: 43973 RVA: 0x00401968 File Offset: 0x003FFB68
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

	// Token: 0x0600ABC6 RID: 43974 RVA: 0x004019E0 File Offset: 0x003FFBE0
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

	// Token: 0x0600ABC7 RID: 43975 RVA: 0x00401A6C File Offset: 0x003FFC6C
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

	// Token: 0x0600ABC8 RID: 43976 RVA: 0x00401AE0 File Offset: 0x003FFCE0
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

	// Token: 0x0600ABC9 RID: 43977 RVA: 0x00401BA8 File Offset: 0x003FFDA8
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

	// Token: 0x17001429 RID: 5161
	// (get) Token: 0x0600ABCA RID: 43978 RVA: 0x00401CB6 File Offset: 0x003FFEB6
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

	// Token: 0x1700142A RID: 5162
	// (get) Token: 0x0600ABCB RID: 43979 RVA: 0x00401CE0 File Offset: 0x003FFEE0
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

	// Token: 0x0600ABCC RID: 43980 RVA: 0x00401D30 File Offset: 0x003FFF30
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

	// Token: 0x0600ABCD RID: 43981 RVA: 0x00401DFC File Offset: 0x003FFFFC
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

	// Token: 0x0600ABCE RID: 43982 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Refresh()
	{
	}

	// Token: 0x04007F81 RID: 32641
	[PublicizedFrom(EAccessModifier.Protected)]
	public string m_Name;

	// Token: 0x04007F82 RID: 32642
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public Type m_ArgumentType;

	// Token: 0x04007F83 RID: 32643
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public Type m_ReturnType;

	// Token: 0x04007F84 RID: 32644
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public FieldInfo[] m_Fields;

	// Token: 0x04007F85 RID: 32645
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public Type[] m_DelegateTypes;

	// Token: 0x04007F86 RID: 32646
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public MethodInfo[] m_DefaultMethods;

	// Token: 0x04007F87 RID: 32647
	[Preserve]
	public string[] InvokerFieldNames;

	// Token: 0x04007F88 RID: 32648
	[Preserve]
	public Dictionary<string, int> Prefixes;
}
