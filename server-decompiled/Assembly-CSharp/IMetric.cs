using System;
using System.Text;

// Token: 0x02001524 RID: 5412
public interface IMetric
{
	// Token: 0x170013C2 RID: 5058
	// (get) Token: 0x0600AA07 RID: 43527
	string Header { get; }

	// Token: 0x0600AA08 RID: 43528
	void AppendLastValue(StringBuilder builder);

	// Token: 0x0600AA09 RID: 43529
	void Cleanup();
}
