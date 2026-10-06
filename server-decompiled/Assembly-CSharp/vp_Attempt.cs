using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001563 RID: 5475
[Preserve]
public class vp_Attempt : vp_Event
{
	// Token: 0x0600ABAA RID: 43946 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool AlwaysOK()
	{
		return true;
	}

	// Token: 0x0600ABAB RID: 43947 RVA: 0x0040160D File Offset: 0x003FF80D
	public vp_Attempt(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x0600ABAC RID: 43948 RVA: 0x0040161C File Offset: 0x003FF81C
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

	// Token: 0x0600ABAD RID: 43949 RVA: 0x004016AF File Offset: 0x003FF8AF
	public override void Register(object t, string m, int v)
	{
		this.Try = (vp_Attempt.Tryer)Delegate.CreateDelegate(this.m_DelegateTypes[v], t, m);
		base.Refresh();
	}

	// Token: 0x0600ABAE RID: 43950 RVA: 0x004016D1 File Offset: 0x003FF8D1
	public override void Unregister(object t)
	{
		this.Try = new vp_Attempt.Tryer(vp_Attempt.AlwaysOK);
		base.Refresh();
	}

	// Token: 0x04007F7F RID: 32639
	public vp_Attempt.Tryer Try;

	// Token: 0x02001564 RID: 5476
	// (Invoke) Token: 0x0600ABB0 RID: 43952
	public delegate bool Tryer();
}
