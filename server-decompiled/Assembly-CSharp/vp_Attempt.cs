using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001575 RID: 5493
[Preserve]
public class vp_Attempt : vp_Event
{
	// Token: 0x0600AC02 RID: 44034 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool AlwaysOK()
	{
		return true;
	}

	// Token: 0x0600AC03 RID: 44035 RVA: 0x00403A11 File Offset: 0x00401C11
	public vp_Attempt(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x0600AC04 RID: 44036 RVA: 0x00403A20 File Offset: 0x00401C20
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

	// Token: 0x0600AC05 RID: 44037 RVA: 0x00403AB3 File Offset: 0x00401CB3
	public override void Register(object t, string m, int v)
	{
		this.Try = (vp_Attempt.Tryer)Delegate.CreateDelegate(this.m_DelegateTypes[v], t, m);
		base.Refresh();
	}

	// Token: 0x0600AC06 RID: 44038 RVA: 0x00403AD5 File Offset: 0x00401CD5
	public override void Unregister(object t)
	{
		this.Try = new vp_Attempt.Tryer(vp_Attempt.AlwaysOK);
		base.Refresh();
	}

	// Token: 0x04007FC0 RID: 32704
	public vp_Attempt.Tryer Try;

	// Token: 0x02001576 RID: 5494
	// (Invoke) Token: 0x0600AC08 RID: 44040
	public delegate bool Tryer();
}
