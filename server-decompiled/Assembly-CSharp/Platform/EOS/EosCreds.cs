using System;

namespace Platform.EOS
{
	// Token: 0x02001D4E RID: 7502
	public class EosCreds
	{
		// Token: 0x0600DE46 RID: 56902 RVA: 0x004F0CFA File Offset: 0x004EEEFA
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

		// Token: 0x0400A7C2 RID: 42946
		public const string StorageEncKey = "0000000000000000000000000000000000000000000000000000000000000000";

		// Token: 0x0400A7C3 RID: 42947
		[PublicizedFrom(EAccessModifier.Private)]
		public const string productId = "85fffb61212b491999cd7fc03eb09bf6";

		// Token: 0x0400A7C4 RID: 42948
		[PublicizedFrom(EAccessModifier.Private)]
		public const string sandboxId = "8a44365d5ccb43328b4df2f8ca199e43";

		// Token: 0x0400A7C5 RID: 42949
		[PublicizedFrom(EAccessModifier.Private)]
		public const string deploymentId = "c9ccbd00333f4dd6995beb7c75000942";

		// Token: 0x0400A7C6 RID: 42950
		[PublicizedFrom(EAccessModifier.Private)]
		public const string deploymentId_Old = "30b9e9e5f58b4f4e82930b3bef76d9e1";

		// Token: 0x0400A7C7 RID: 42951
		public static readonly EosCreds ClientCredentials = new EosCreds("85fffb61212b491999cd7fc03eb09bf6", "8a44365d5ccb43328b4df2f8ca199e43", "c9ccbd00333f4dd6995beb7c75000942", "xyza7891WBnGQuvNMiNyg6SYeYOhbA2F", "aopC/pp4xFK643dkeOOktsSiFV1IC5qQiLfJ8EJjPrw", false);

		// Token: 0x0400A7C8 RID: 42952
		public static readonly EosCreds ServerCredentials = new EosCreds("85fffb61212b491999cd7fc03eb09bf6", "8a44365d5ccb43328b4df2f8ca199e43", "c9ccbd00333f4dd6995beb7c75000942", "xyza7891nSjSAzYxhnVGWL1xKR4jAL7I", "fkCG6lR19l6KCfXFxxF1dppvCbA76qZT9IO+4eqX5QU", true);

		// Token: 0x0400A7C9 RID: 42953
		public static readonly EosCreds ServerDeviceIdCredentials = new EosCreds("85fffb61212b491999cd7fc03eb09bf6", "8a44365d5ccb43328b4df2f8ca199e43", "c9ccbd00333f4dd6995beb7c75000942", "xyza7891UbuRjRa7tG4QlJvZC6l62dwE", "Ah8Ul94L+ybtvVhqJAKPMOKKfuLu+8bPlmoZaPLFbkc", false);

		// Token: 0x0400A7CA RID: 42954
		public readonly string ProductId;

		// Token: 0x0400A7CB RID: 42955
		public readonly string SandboxId;

		// Token: 0x0400A7CC RID: 42956
		public readonly string DeploymentId;

		// Token: 0x0400A7CD RID: 42957
		public readonly string ClientId;

		// Token: 0x0400A7CE RID: 42958
		public readonly string ClientSecret;

		// Token: 0x0400A7CF RID: 42959
		public readonly bool ServerMode;
	}
}
