using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200023B RID: 571
	[RequiredByNativeCode(GenerateProxy = true)]
	[NativeHeader("Runtime/Mono/AssemblyFullName.h")]
	internal struct AssemblyFullName
	{
		// Token: 0x0600186F RID: 6255 RVA: 0x0002892C File Offset: 0x00026B2C
		public override bool Equals(object other)
		{
			if (other is AssemblyFullName)
			{
				AssemblyFullName assemblyFullName = (AssemblyFullName)other;
				if (this.Name == assemblyFullName.Name && this.Version == assemblyFullName.Version && this.PublicKeyToken == assemblyFullName.PublicKeyToken)
				{
					return this.Culture == assemblyFullName.Culture;
				}
			}
			return false;
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x0002899C File Offset: 0x00026B9C
		public override int GetHashCode()
		{
			return HashCode.Combine<string, AssemblyVersion, string, string>(this.Name, this.Version, this.PublicKeyToken, this.Culture);
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x000289CC File Offset: 0x00026BCC
		public override string ToString()
		{
			return string.Format("{0}, Version={1}, Culture={2}, PublicKeyToken={3}", new object[]
			{
				this.Name,
				this.Version,
				string.IsNullOrEmpty(this.Culture) ? "neutral" : this.Culture,
				this.PublicKeyToken
			});
		}

		// Token: 0x040008AB RID: 2219
		[NativeName("name")]
		public string Name;

		// Token: 0x040008AC RID: 2220
		[NativeName("version")]
		public AssemblyVersion Version;

		// Token: 0x040008AD RID: 2221
		[NativeName("publicKeyToken")]
		public string PublicKeyToken;

		// Token: 0x040008AE RID: 2222
		[NativeName("culture")]
		public string Culture;
	}
}
