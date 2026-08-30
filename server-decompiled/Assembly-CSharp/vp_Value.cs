using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001570 RID: 5488
[Preserve]
public class vp_Value<V> : vp_Event
{
	// Token: 0x0600ABD7 RID: 43991 RVA: 0x004000C4 File Offset: 0x003FE2C4
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static T Empty<T>()
	{
		return default(T);
	}

	// Token: 0x0600ABD8 RID: 43992 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty<T>(T value)
	{
	}

	// Token: 0x170013E7 RID: 5095
	// (get) Token: 0x0600ABD9 RID: 43993 RVA: 0x004000DA File Offset: 0x003FE2DA
	[Preserve]
	public FieldInfo[] Fields
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_Fields;
		}
	}

	// Token: 0x0600ABDA RID: 43994 RVA: 0x003FC7B9 File Offset: 0x003FA9B9
	[Preserve]
	public vp_Value(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x0600ABDB RID: 43995 RVA: 0x004000E4 File Offset: 0x003FE2E4
	public void DoNotCallAOTCompileFix()
	{
		vp_Value<V>.Empty<V>();
		vp_Value<V>.Empty<V>(default(V));
		throw new InvalidOperationException("This method is used for AOT code generation only. Do not call it at runtime.");
	}

	// Token: 0x0600ABDC RID: 43996 RVA: 0x00400110 File Offset: 0x003FE310
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

	// Token: 0x0600ABDD RID: 43997 RVA: 0x0040025A File Offset: 0x003FE45A
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

	// Token: 0x0600ABDE RID: 43998 RVA: 0x00400284 File Offset: 0x003FE484
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

	// Token: 0x04007F25 RID: 32549
	[Preserve]
	public vp_Value<V>.Getter<V> Get;

	// Token: 0x04007F26 RID: 32550
	[Preserve]
	public vp_Value<V>.Setter<V> Set;

	// Token: 0x02001571 RID: 5489
	// (Invoke) Token: 0x0600ABE0 RID: 44000
	[Preserve]
	public delegate T Getter<T>();

	// Token: 0x02001572 RID: 5490
	// (Invoke) Token: 0x0600ABE4 RID: 44004
	[Preserve]
	public delegate void Setter<T>(T o);
}
