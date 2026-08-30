using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001528 RID: 5416
[Preserve]
public class vp_Attempt<V> : vp_Attempt
{
	// Token: 0x0600AA03 RID: 43523 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool AlwaysOK<T>(T value)
	{
		return true;
	}

	// Token: 0x0600AA04 RID: 43524 RVA: 0x003F7557 File Offset: 0x003F5757
	public vp_Attempt(string name)
		: base(name)
	{
	}

	// Token: 0x0600AA05 RID: 43525 RVA: 0x003F7560 File Offset: 0x003F5760
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

	// Token: 0x0600AA06 RID: 43526 RVA: 0x003F7628 File Offset: 0x003F5828
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

	// Token: 0x0600AA07 RID: 43527 RVA: 0x003F76A1 File Offset: 0x003F58A1
	public override void Unregister(object t)
	{
		if (this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
	}

	// Token: 0x04007E24 RID: 32292
	public new vp_Attempt<V>.Tryer<V> Try;

	// Token: 0x02001529 RID: 5417
	// (Invoke) Token: 0x0600AA09 RID: 43529
	public delegate bool Tryer<T>(T value);
}
