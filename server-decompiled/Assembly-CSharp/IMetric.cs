using System;
using System.Text;

// Token: 0x02001546 RID: 5446
public interface IMetric
{
	// Token: 0x17001407 RID: 5127
	// (get) Token: 0x0600AABA RID: 43706
	string Header { get; }

	// Token: 0x0600AABB RID: 43707
	void AppendLastValue(StringBuilder builder);

	// Token: 0x0600AABC RID: 43708
	void Cleanup();
}
