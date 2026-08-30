using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000CA RID: 202
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	[BaseTypeRequired(typeof(Attribute))]
	public sealed class BaseTypeRequiredAttribute : Attribute
	{
		// Token: 0x060003C7 RID: 967 RVA: 0x00006B65 File Offset: 0x00004D65
		public BaseTypeRequiredAttribute([NotNull] Type baseType)
		{
			this.BaseType = baseType;
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00006B76 File Offset: 0x00004D76
		[NotNull]
		public Type BaseType { get; }
	}
}
