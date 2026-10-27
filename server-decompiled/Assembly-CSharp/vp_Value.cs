using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x020015A4 RID: 5540
[Preserve]
public class vp_Value<V> : vp_Event
{
	// Token: 0x0600ACE2 RID: 44258 RVA: 0x0040731C File Offset: 0x0040551C
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static T Empty<T>()
	{
		return default(T);
	}

	// Token: 0x0600ACE3 RID: 44259 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty<T>(T value)
	{
	}

	// Token: 0x17001438 RID: 5176
	// (get) Token: 0x0600ACE4 RID: 44260 RVA: 0x00407332 File Offset: 0x00405532
	[Preserve]
	public FieldInfo[] Fields
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_Fields;
		}
	}

	// Token: 0x0600ACE5 RID: 44261 RVA: 0x00403A11 File Offset: 0x00401C11
	[Preserve]
	public vp_Value(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x0600ACE6 RID: 44262 RVA: 0x0040733C File Offset: 0x0040553C
	public void DoNotCallAOTCompileFix()
	{
		vp_Value<V>.Empty<V>();
		vp_Value<V>.Empty<V>(default(V));
		throw new InvalidOperationException("This method is used for AOT code generation only. Do not call it at runtime.");
	}

	// Token: 0x0600ACE7 RID: 44263 RVA: 0x00407368 File Offset: 0x00405568
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitFields()
	{
		this.m_Fields = new FieldInfo[]
		{
			base.GetType().GetField("Get"),
			base.GetType().GetField("Set")
		};
		base.StoreInvokerFieldNames();
		this.m_DelegateTypes = new Type[]
		{
			typeof(vp_Value<>.Getter<>),
			typeof(vp_Value<>.Setter<>)
		};
		this.m_DefaultMethods = new MethodInfo[]
		{
			base.GetStaticGenericMethod(base.GetType(), "Empty", typeof(void), this.m_ArgumentType),
			base.GetStaticGenericMethod(base.GetType(), "Empty", this.m_ArgumentType, typeof(void))
		};
		this.Prefixes = new Dictionary<string, int>
		{
			{ "get_OnValue_", 0 },
			{ "set_OnValue_", 1 }
		};
		if (this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
		if (this.m_DefaultMethods[1] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[1], this.m_DefaultMethods[1], base.MakeGenericType(this.m_DelegateTypes[1]));
		}
	}

	// Token: 0x0600ACE8 RID: 44264 RVA: 0x004074B2 File Offset: 0x004056B2
	[Preserve]
	public override void Register(object t, string m, int v)
	{
		if (m == null)
		{
			return;
		}
		base.SetFieldToExternalMethod(t, this.m_Fields[v], m, base.MakeGenericType(this.m_DelegateTypes[v]));
		base.Refresh();
	}

	// Token: 0x0600ACE9 RID: 44265 RVA: 0x004074DC File Offset: 0x004056DC
	[Preserve]
	public override void Unregister(object t)
	{
		if (this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
		if (this.m_DefaultMethods[1] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[1], this.m_DefaultMethods[1], base.MakeGenericType(this.m_DelegateTypes[1]));
		}
		base.Refresh();
	}

	// Token: 0x04007FE9 RID: 32745
	[Preserve]
	public vp_Value<V>.Getter<V> Get;

	// Token: 0x04007FEA RID: 32746
	[Preserve]
	public vp_Value<V>.Setter<V> Set;

	// Token: 0x020015A5 RID: 5541
	// (Invoke) Token: 0x0600ACEB RID: 44267
	[Preserve]
	public delegate T Getter<T>();

	// Token: 0x020015A6 RID: 5542
	// (Invoke) Token: 0x0600ACEF RID: 44271
	[Preserve]
	public delegate void Setter<T>(T o);
}
