using System;
using System.Text;

// Token: 0x02001509 RID: 5385
public interface IMetric
{
	// Token: 0x17001389 RID: 5001
	// (get) Token: 0x0600A90A RID: 43274
	string Header { get; }

	// Token: 0x0600A90B RID: 43275
	void AppendLastValue(StringBuilder builder);

	// Token: 0x0600A90C RID: 43276
	void Cleanup();
}
