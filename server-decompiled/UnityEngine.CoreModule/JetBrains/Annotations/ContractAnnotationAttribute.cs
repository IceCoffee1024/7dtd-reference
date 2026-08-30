using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000C7 RID: 199
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public sealed class ContractAnnotationAttribute : Attribute
	{
		// Token: 0x060003BF RID: 959 RVA: 0x00006B0D File Offset: 0x00004D0D
		public ContractAnnotationAttribute([NotNull] string contract)
			: this(contract, false)
		{
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00006B19 File Offset: 0x00004D19
		public ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
		{
			this.Contract = contract;
			this.ForceFullStates = forceFullStates;
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00006B31 File Offset: 0x00004D31
		[NotNull]
		public string Contract { get; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x00006B39 File Offset: 0x00004D39
		public bool ForceFullStates { get; }
	}
}
