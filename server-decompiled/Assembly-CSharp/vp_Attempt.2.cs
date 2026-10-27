using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001577 RID: 5495
[Preserve]
public class vp_Attempt<V> : vp_Attempt
{
	// Token: 0x0600AC0B RID: 44043 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool AlwaysOK<T>(T value)
	{
		return true;
	}

	// Token: 0x0600AC0C RID: 44044 RVA: 0x00403AEF File Offset: 0x00401CEF
	public vp_Attempt(string name)
		: base(name)
	{
	}

	// Token: 0x0600AC0D RID: 44045 RVA: 0x00403AF8 File Offset: 0x00401CF8
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

	// Token: 0x0600AC0E RID: 44046 RVA: 0x00403BC0 File Offset: 0x00401DC0
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

	// Token: 0x0600AC0F RID: 44047 RVA: 0x00403C39 File Offset: 0x00401E39
	public override void Unregister(object t)
	{
		if (this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
	}

	// Token: 0x04007FC1 RID: 32705
	public new vp_Attempt<V>.Tryer<V> Try;

	// Token: 0x02001578 RID: 5496
	// (Invoke) Token: 0x0600AC11 RID: 44049
	public delegate bool Tryer<T>(T value);
}
