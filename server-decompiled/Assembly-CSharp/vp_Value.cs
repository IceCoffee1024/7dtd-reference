using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001592 RID: 5522
[Preserve]
public class vp_Value<V> : vp_Event
{
	// Token: 0x0600AC8A RID: 44170 RVA: 0x00404F18 File Offset: 0x00403118
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static T Empty<T>()
	{
		return default(T);
	}

	// Token: 0x0600AC8B RID: 44171 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty<T>(T value)
	{
	}

	// Token: 0x1700142C RID: 5164
	// (get) Token: 0x0600AC8C RID: 44172 RVA: 0x00404F2E File Offset: 0x0040312E
	[Preserve]
	public FieldInfo[] Fields
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_Fields;
		}
	}

	// Token: 0x0600AC8D RID: 44173 RVA: 0x0040160D File Offset: 0x003FF80D
	[Preserve]
	public vp_Value(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x0600AC8E RID: 44174 RVA: 0x00404F38 File Offset: 0x00403138
	public void DoNotCallAOTCompileFix()
	{
		vp_Value<V>.Empty<V>();
		vp_Value<V>.Empty<V>(default(V));
		throw new InvalidOperationException("This method is used for AOT code generation only. Do not call it at runtime.");
	}

	// Token: 0x0600AC8F RID: 44175 RVA: 0x00404F64 File Offset: 0x00403164
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

	// Token: 0x0600AC90 RID: 44176 RVA: 0x004050AE File Offset: 0x004032AE
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

	// Token: 0x0600AC91 RID: 44177 RVA: 0x004050D8 File Offset: 0x004032D8
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

	// Token: 0x04007FA8 RID: 32680
	[Preserve]
	public vp_Value<V>.Getter<V> Get;

	// Token: 0x04007FA9 RID: 32681
	[Preserve]
	public vp_Value<V>.Setter<V> Set;

	// Token: 0x02001593 RID: 5523
	// (Invoke) Token: 0x0600AC93 RID: 44179
	[Preserve]
	public delegate T Getter<T>();

	// Token: 0x02001594 RID: 5524
	// (Invoke) Token: 0x0600AC97 RID: 44183
	[Preserve]
	public delegate void Setter<T>(T o);
}
