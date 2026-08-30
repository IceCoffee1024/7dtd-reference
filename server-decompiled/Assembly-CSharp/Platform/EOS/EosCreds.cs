using System;

namespace Platform.EOS
{
	// Token: 0x02001CEB RID: 7403
	public class EosCreds
	{
		// Token: 0x0600DBB7 RID: 56247 RVA: 0x004E8612 File Offset: 0x004E6812
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

		// Token: 0x0400A604 RID: 42500
		public const string StorageEncKey = "0000000000000000000000000000000000000000000000000000000000000000";

		// Token: 0x0400A605 RID: 42501
		[PublicizedFrom(EAccessModifier.Private)]
		public const string productId = "85fffb61212b491999cd7fc03eb09bf6";

		// Token: 0x0400A606 RID: 42502
		[PublicizedFrom(EAccessModifier.Private)]
		public const string sandboxId = "8a44365d5ccb43328b4df2f8ca199e43";

		// Token: 0x0400A607 RID: 42503
		[PublicizedFrom(EAccessModifier.Private)]
		public const string deploymentId = "c9ccbd00333f4dd6995beb7c75000942";

		// Token: 0x0400A608 RID: 42504
		[PublicizedFrom(EAccessModifier.Private)]
		public const string deploymentId_Old = "30b9e9e5f58b4f4e82930b3bef76d9e1";

		// Token: 0x0400A609 RID: 42505
		public static readonly EosCreds ClientCredentials = new EosCreds("85fffb61212b491999cd7fc03eb09bf6", "8a44365d5ccb43328b4df2f8ca199e43", "c9ccbd00333f4dd6995beb7c75000942", "xyza7891WBnGQuvNMiNyg6SYeYOhbA2F", "aopC/pp4xFK643dkeOOktsSiFV1IC5qQiLfJ8EJjPrw", false);

		// Token: 0x0400A60A RID: 42506
		public static readonly EosCreds ServerCredentials = new EosCreds("85fffb61212b491999cd7fc03eb09bf6", "8a44365d5ccb43328b4df2f8ca199e43", "c9ccbd00333f4dd6995beb7c75000942", "xyza7891nSjSAzYxhnVGWL1xKR4jAL7I", "fkCG6lR19l6KCfXFxxF1dppvCbA76qZT9IO+4eqX5QU", true);

		// Token: 0x0400A60B RID: 42507
		public static readonly EosCreds ServerDeviceIdCredentials = new EosCreds("85fffb61212b491999cd7fc03eb09bf6", "8a44365d5ccb43328b4df2f8ca199e43", "c9ccbd00333f4dd6995beb7c75000942", "xyza7891UbuRjRa7tG4QlJvZC6l62dwE", "Ah8Ul94L+ybtvVhqJAKPMOKKfuLu+8bPlmoZaPLFbkc", false);

		// Token: 0x0400A60C RID: 42508
		public readonly string ProductId;

		// Token: 0x0400A60D RID: 42509
		public readonly string SandboxId;

		// Token: 0x0400A60E RID: 42510
		public readonly string DeploymentId;

		// Token: 0x0400A60F RID: 42511
		public readonly string ClientId;

		// Token: 0x0400A610 RID: 42512
		public readonly string ClientSecret;

		// Token: 0x0400A611 RID: 42513
		public readonly bool ServerMode;
	}
}
