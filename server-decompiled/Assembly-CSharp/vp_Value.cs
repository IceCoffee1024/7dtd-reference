using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001555 RID: 5461
[Preserve]
public class vp_Value<V> : vp_Event
{
	// Token: 0x0600AADA RID: 43738 RVA: 0x003FAD84 File Offset: 0x003F8F84
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static T Empty<T>()
	{
		return default(T);
	}

	// Token: 0x0600AADB RID: 43739 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty<T>(T value)
	{
	}

	// Token: 0x170013AE RID: 5038
	// (get) Token: 0x0600AADC RID: 43740 RVA: 0x003FAD9A File Offset: 0x003F8F9A
	[Preserve]
	public FieldInfo[] Fields
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_Fields;
		}
	}

	// Token: 0x0600AADD RID: 43741 RVA: 0x003F7479 File Offset: 0x003F5679
	[Preserve]
	public vp_Value(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x0600AADE RID: 43742 RVA: 0x003FADA4 File Offset: 0x003F8FA4
	public void DoNotCallAOTCompileFix()
	{
		vp_Value<V>.Empty<V>();
		vp_Value<V>.Empty<V>(default(V));
		throw new InvalidOperationException("This method is used for AOT code generation only. Do not call it at runtime.");
	}

	// Token: 0x0600AADF RID: 43743 RVA: 0x003FADD0 File Offset: 0x003F8FD0
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

	// Token: 0x0600AAE0 RID: 43744 RVA: 0x003FAF1A File Offset: 0x003F911A
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

	// Token: 0x0600AAE1 RID: 43745 RVA: 0x003FAF44 File Offset: 0x003F9144
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

	// Token: 0x04007E4C RID: 32332
	[Preserve]
	public vp_Value<V>.Getter<V> Get;

	// Token: 0x04007E4D RID: 32333
	[Preserve]
	public vp_Value<V>.Setter<V> Set;

	// Token: 0x02001556 RID: 5462
	// (Invoke) Token: 0x0600AAE3 RID: 43747
	[Preserve]
	public delegate T Getter<T>();

	// Token: 0x02001557 RID: 5463
	// (Invoke) Token: 0x0600AAE7 RID: 43751
	[Preserve]
	public delegate void Setter<T>(T o);
}
