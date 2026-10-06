using System;

// Token: 0x02001116 RID: 4374
public interface IBindingNcalc
{
	// Token: 0x17001040 RID: 4160
	// (get) Token: 0x06008A94 RID: 35476
	string SourceText { get; }

	// Token: 0x17001041 RID: 4161
	// (get) Token: 0x06008A95 RID: 35477
	IXUiElement TargetElement { get; }

	// Token: 0x17001042 RID: 4162
	// (get) Token: 0x06008A96 RID: 35478
	bool IsInitializing { get; }

	// Token: 0x06008A97 RID: 35479
	void SetIndeterministic();

	// Token: 0x06008A98 RID: 35480
	bool FindParameter(string _name, out object _value);

	// Token: 0x06008A99 RID: 35481
	bool RegisterVariable(BindingInfoNcalc.VariableStateAbs _var);
}
