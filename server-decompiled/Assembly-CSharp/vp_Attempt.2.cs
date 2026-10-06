using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001565 RID: 5477
[Preserve]
public class vp_Attempt<V> : vp_Attempt
{
	// Token: 0x0600ABB3 RID: 43955 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool AlwaysOK<T>(T value)
	{
		return true;
	}

	// Token: 0x0600ABB4 RID: 43956 RVA: 0x004016EB File Offset: 0x003FF8EB
	public vp_Attempt(string name)
		: base(name)
	{
	}

	// Token: 0x0600ABB5 RID: 43957 RVA: 0x004016F4 File Offset: 0x003FF8F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitFields()
	{
		this.m_Fields = new FieldInfo[] { base.GetType().GetField("Try") };
		base.StoreInvokerFieldNames();
		this.m_DefaultMethods = new MethodInfo[] { base.GetStaticGenericMethod(base.GetType(), "AlwaysOK", this.m_ArgumentType, typeof(bool)) };
		this.m_DelegateTypes = new Type[] { typeof(vp_Attempt<>.Tryer<>) };
		this.Prefixes = new Dictionary<string, int> { { "OnAttempt_", 0 } };
		if (this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
	}

	// Token: 0x0600ABB6 RID: 43958 RVA: 0x004017BC File Offset: 0x003FF9BC
	public override void Register(object t, string m, int v)
	{
		if (((Delegate)this.m_Fields[v].GetValue(this)).Method.Name != this.m_DefaultMethods[v].Name)
		{
			Debug.LogWarning("Warning: Event '" + base.EventName + "' of type (vp_Attempt) targets multiple methods. Events of this type must reference a single method (only the last reference will be functional).");
		}
		if (m != null)
		{
			base.SetFieldToExternalMethod(t, this.m_Fields[0], m, base.MakeGenericType(this.m_DelegateTypes[v]));
		}
	}

	// Token: 0x0600ABB7 RID: 43959 RVA: 0x00401835 File Offset: 0x003FFA35
	public override void Unregister(object t)
	{
		if (this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
	}

	// Token: 0x04007F80 RID: 32640
	public new vp_Attempt<V>.Tryer<V> Try;

	// Token: 0x02001566 RID: 5478
	// (Invoke) Token: 0x0600ABB9 RID: 43961
	public delegate bool Tryer<T>(T value);
}
