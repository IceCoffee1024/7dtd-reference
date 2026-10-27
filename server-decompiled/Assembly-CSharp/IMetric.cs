using System;
using System.Text;

// Token: 0x02001558 RID: 5464
public interface IMetric
{
	// Token: 0x17001413 RID: 5139
	// (get) Token: 0x0600AB12 RID: 43794
	string Header { get; }

	// Token: 0x0600AB13 RID: 43795
	void AppendLastValue(StringBuilder builder);

	// Token: 0x0600AB14 RID: 43796
	void Cleanup();
}
