using System;

// Token: 0x02001126 RID: 4390
public interface IBindingNcalc
{
	// Token: 0x1700104B RID: 4171
	// (get) Token: 0x06008AE8 RID: 35560
	string SourceText { get; }

	// Token: 0x1700104C RID: 4172
	// (get) Token: 0x06008AE9 RID: 35561
	IXUiElement TargetElement { get; }

	// Token: 0x1700104D RID: 4173
	// (get) Token: 0x06008AEA RID: 35562
	bool IsInitializing { get; }

	// Token: 0x06008AEB RID: 35563
	void SetIndeterministic();

	// Token: 0x06008AEC RID: 35564
	bool FindParameter(string _name, out object _value);

	// Token: 0x06008AED RID: 35565
	bool RegisterVariable(BindingInfoNcalc.VariableStateAbs _var);
}
