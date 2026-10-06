using System;

namespace Platform.EOS
{
	// Token: 0x02001D1D RID: 7453
	public class EosCreds
	{
		// Token: 0x0600DD32 RID: 56626 RVA: 0x004ED15A File Offset: 0x004EB35A
		[PublicizedFrom(EAccessModifier.Private)]
		public EosCreds(string _productId, string _sandboxId, string _deploymentId, string _clientId, string _clientSecret, bool _serverMode)
		{
			this.ProductId = _productId;
			this.SandboxId = _sandboxId;
			this.DeploymentId = _deploymentId;
			this.ClientId = _clientId;
			this.ClientSecret = _clientSecret;
			this.ServerMode = _serverMode;
		}

		// Token: 0x0400A6D9 RID: 42713
		public const string StorageEncKey = "0000000000000000000000000000000000000000000000000000000000000000";

		// Token: 0x0400A6DA RID: 42714
		[PublicizedFrom(EAccessModifier.Private)]
		public const string productId = "85fffb61212b491999cd7fc03eb09bf6";

		// Token: 0x0400A6DB RID: 42715
		[PublicizedFrom(EAccessModifier.Private)]
		public const string sandboxId = "8a44365d5ccb43328b4df2f8ca199e43";

		// Token: 0x0400A6DC RID: 42716
		[PublicizedFrom(EAccessModifier.Private)]
		public const string deploymentId = "c9ccbd00333f4dd6995beb7c75000942";

		// Token: 0x0400A6DD RID: 42717
		[PublicizedFrom(EAccessModifier.Private)]
		public const string deploymentId_Old = "30b9e9e5f58b4f4e82930b3bef76d9e1";

		// Token: 0x0400A6DE RID: 42718
		public static readonly EosCreds ClientCredentials = new EosCreds("85fffb61212b491999cd7fc03eb09bf6", "8a44365d5ccb43328b4df2f8ca199e43", "c9ccbd00333f4dd6995beb7c75000942", "xyza7891WBnGQuvNMiNyg6SYeYOhbA2F", "aopC/pp4xFK643dkeOOktsSiFV1IC5qQiLfJ8EJjPrw", false);

		// Token: 0x0400A6DF RID: 42719
		public static readonly EosCreds ServerCredentials = new EosCreds("85fffb61212b491999cd7fc03eb09bf6", "8a44365d5ccb43328b4df2f8ca199e43", "c9ccbd00333f4dd6995beb7c75000942", "xyza7891nSjSAzYxhnVGWL1xKR4jAL7I", "fkCG6lR19l6KCfXFxxF1dppvCbA76qZT9IO+4eqX5QU", true);

		// Token: 0x0400A6E0 RID: 42720
		public static readonly EosCreds ServerDeviceIdCredentials = new EosCreds("85fffb61212b491999cd7fc03eb09bf6", "8a44365d5ccb43328b4df2f8ca199e43", "c9ccbd00333f4dd6995beb7c75000942", "xyza7891UbuRjRa7tG4QlJvZC6l62dwE", "Ah8Ul94L+ybtvVhqJAKPMOKKfuLu+8bPlmoZaPLFbkc", false);

		// Token: 0x0400A6E1 RID: 42721
		public readonly string ProductId;

		// Token: 0x0400A6E2 RID: 42722
		public readonly string SandboxId;

		// Token: 0x0400A6E3 RID: 42723
		public readonly string DeploymentId;

		// Token: 0x0400A6E4 RID: 42724
		public readonly string ClientId;

		// Token: 0x0400A6E5 RID: 42725
		public readonly string ClientSecret;

		// Token: 0x0400A6E6 RID: 42726
		public readonly bool ServerMode;
	}
}
