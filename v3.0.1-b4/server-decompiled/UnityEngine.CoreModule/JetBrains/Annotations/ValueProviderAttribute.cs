using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000C4 RID: 196
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
	public sealed class ValueProviderAttribute : Attribute
	{
		// Token: 0x060003B9 RID: 953 RVA: 0x00006ADB File Offset: 0x00004CDB
		public ValueProviderAttribute([NotNull] string name)
		{
			this.Name = name;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00006AEC File Offset: 0x00004CEC
		[NotNull]
		public string Name { get; }
	}
}
