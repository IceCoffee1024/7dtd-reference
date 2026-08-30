using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001543 RID: 5443
[Preserve]
public class vp_Attempt<V> : vp_Attempt
{
	// Token: 0x0600AB00 RID: 43776 RVA: 0x00020021 File Offset: 0x0001E221
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool AlwaysOK<T>(T value)
	{
		return true;
	}

	// Token: 0x0600AB01 RID: 43777 RVA: 0x003FC897 File Offset: 0x003FAA97
	public vp_Attempt(string name)
		: base(name)
	{
	}

	// Token: 0x0600AB02 RID: 43778 RVA: 0x003FC8A0 File Offset: 0x003FAAA0
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

	// Token: 0x0600AB03 RID: 43779 RVA: 0x003FC968 File Offset: 0x003FAB68
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

	// Token: 0x0600AB04 RID: 43780 RVA: 0x003FC9E1 File Offset: 0x003FABE1
	public override void Unregister(object t)
	{
		if (this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
	}

	// Token: 0x04007EFD RID: 32509
	public new vp_Attempt<V>.Tryer<V> Try;

	// Token: 0x02001544 RID: 5444
	// (Invoke) Token: 0x0600AB06 RID: 43782
	public delegate bool Tryer<T>(T value);
}
