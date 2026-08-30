using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001526 RID: 5414
[Preserve]
public class vp_Attempt : vp_Event
{
	// Token: 0x0600A9FA RID: 43514 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool AlwaysOK()
	{
		return true;
	}

	// Token: 0x0600A9FB RID: 43515 RVA: 0x003F7479 File Offset: 0x003F5679
	public vp_Attempt(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x0600A9FC RID: 43516 RVA: 0x003F7488 File Offset: 0x003F5688
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

	// Token: 0x0600A9FD RID: 43517 RVA: 0x003F751B File Offset: 0x003F571B
	public override void Register(object t, string m, int v)
	{
		this.Try = (vp_Attempt.Tryer)Delegate.CreateDelegate(this.m_DelegateTypes[v], t, m);
		base.Refresh();
	}

	// Token: 0x0600A9FE RID: 43518 RVA: 0x003F753D File Offset: 0x003F573D
	public override void Unregister(object t)
	{
		this.Try = new vp_Attempt.Tryer(vp_Attempt.AlwaysOK);
		base.Refresh();
	}

	// Token: 0x04007E23 RID: 32291
	public vp_Attempt.Tryer Try;

	// Token: 0x02001527 RID: 5415
	// (Invoke) Token: 0x0600AA00 RID: 43520
	public delegate bool Tryer();
}
