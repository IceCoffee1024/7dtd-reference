using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001541 RID: 5441
[Preserve]
public class vp_Attempt : vp_Event
{
	// Token: 0x0600AAF7 RID: 43767 RVA: 0x00020021 File Offset: 0x0001E221
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool AlwaysOK()
	{
		return true;
	}

	// Token: 0x0600AAF8 RID: 43768 RVA: 0x003FC7B9 File Offset: 0x003FA9B9
	public vp_Attempt(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x0600AAF9 RID: 43769 RVA: 0x003FC7C8 File Offset: 0x003FA9C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitFields()
	{
		this.m_Fields = new FieldInfo[] { base.GetType().GetField("Try") };
		base.StoreInvokerFieldNames();
		this.m_DefaultMethods = new MethodInfo[] { base.GetType().GetMethod("AlwaysOK") };
		this.m_DelegateTypes = new Type[] { typeof(vp_Attempt.Tryer) };
		this.Prefixes = new Dictionary<string, int> { { "OnAttempt_", 0 } };
		this.Try = new vp_Attempt.Tryer(vp_Attempt.AlwaysOK);
	}

	// Token: 0x0600AAFA RID: 43770 RVA: 0x003FC85B File Offset: 0x003FAA5B
	public override void Register(object t, string m, int v)
	{
		this.Try = (vp_Attempt.Tryer)Delegate.CreateDelegate(this.m_DelegateTypes[v], t, m);
		base.Refresh();
	}

	// Token: 0x0600AAFB RID: 43771 RVA: 0x003FC87D File Offset: 0x003FAA7D
	public override void Unregister(object t)
	{
		this.Try = new vp_Attempt.Tryer(vp_Attempt.AlwaysOK);
		base.Refresh();
	}

	// Token: 0x04007EFC RID: 32508
	public vp_Attempt.Tryer Try;

	// Token: 0x02001542 RID: 5442
	// (Invoke) Token: 0x0600AAFD RID: 43773
	public delegate bool Tryer();
}
