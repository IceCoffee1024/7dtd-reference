using System;

namespace Platform.EOS
{
	// Token: 0x02001CC4 RID: 7364
	public class EosCreds
	{
		// Token: 0x0600DA1C RID: 55836 RVA: 0x004E1FFE File Offset: 0x004E01FE
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

		// Token: 0x0400A4D1 RID: 42193
		public const string StorageEncKey = "0000000000000000000000000000000000000000000000000000000000000000";

		// Token: 0x0400A4D2 RID: 42194
		[PublicizedFrom(EAccessModifier.Private)]
		public const string productId = "85fffb61212b491999cd7fc03eb09bf6";

		// Token: 0x0400A4D3 RID: 42195
		[PublicizedFrom(EAccessModifier.Private)]
		public const string sandboxId = "8a44365d5ccb43328b4df2f8ca199e43";

		// Token: 0x0400A4D4 RID: 42196
		[PublicizedFrom(EAccessModifier.Private)]
		public const string deploymentId = "c9ccbd00333f4dd6995beb7c75000942";

		// Token: 0x0400A4D5 RID: 42197
		[PublicizedFrom(EAccessModifier.Private)]
		public const string deploymentId_Old = "30b9e9e5f58b4f4e82930b3bef76d9e1";

		// Token: 0x0400A4D6 RID: 42198
		public static readonly EosCreds ClientCredentials = new EosCreds("85fffb61212b491999cd7fc03eb09bf6", "8a44365d5ccb43328b4df2f8ca199e43", "c9ccbd00333f4dd6995beb7c75000942", "xyza7891WBnGQuvNMiNyg6SYeYOhbA2F", "aopC/pp4xFK643dkeOOktsSiFV1IC5qQiLfJ8EJjPrw", false);

		// Token: 0x0400A4D7 RID: 42199
		public static readonly EosCreds ServerCredentials = new EosCreds("85fffb61212b491999cd7fc03eb09bf6", "8a44365d5ccb43328b4df2f8ca199e43", "c9ccbd00333f4dd6995beb7c75000942", "xyza7891nSjSAzYxhnVGWL1xKR4jAL7I", "fkCG6lR19l6KCfXFxxF1dppvCbA76qZT9IO+4eqX5QU", true);

		// Token: 0x0400A4D8 RID: 42200
		public static readonly EosCreds ServerDeviceIdCredentials = new EosCreds("85fffb61212b491999cd7fc03eb09bf6", "8a44365d5ccb43328b4df2f8ca199e43", "c9ccbd00333f4dd6995beb7c75000942", "xyza7891UbuRjRa7tG4QlJvZC6l62dwE", "Ah8Ul94L+ybtvVhqJAKPMOKKfuLu+8bPlmoZaPLFbkc", false);

		// Token: 0x0400A4D9 RID: 42201
		public readonly string ProductId;

		// Token: 0x0400A4DA RID: 42202
		public readonly string SandboxId;

		// Token: 0x0400A4DB RID: 42203
		public readonly string DeploymentId;

		// Token: 0x0400A4DC RID: 42204
		public readonly string ClientId;

		// Token: 0x0400A4DD RID: 42205
		public readonly string ClientSecret;

		// Token: 0x0400A4DE RID: 42206
		public readonly bool ServerMode;
	}
}
