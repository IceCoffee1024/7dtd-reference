using System;

// Token: 0x020010E8 RID: 4328
public interface IBindingNcalc
{
	// Token: 0x17000FD2 RID: 4050
	// (get) Token: 0x06008941 RID: 35137
	string SourceText { get; }

	// Token: 0x17000FD3 RID: 4051
	// (get) Token: 0x06008942 RID: 35138
	IXUiElement TargetElement { get; }

	// Token: 0x17000FD4 RID: 4052
	// (get) Token: 0x06008943 RID: 35139
	bool IsInitializing { get; }

	// Token: 0x06008944 RID: 35140
	void SetIndeterministic();

	// Token: 0x06008945 RID: 35141
	bool FindParameter(string _name, out object _value);

	// Token: 0x06008946 RID: 35142
	bool RegisterVariable(BindingInfoNcalc.VariableStateAbs _var);
}
