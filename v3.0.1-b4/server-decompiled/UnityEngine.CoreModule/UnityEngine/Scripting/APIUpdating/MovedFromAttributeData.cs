using System;

namespace UnityEngine.Scripting.APIUpdating
{
	// Token: 0x0200031D RID: 797
	internal struct MovedFromAttributeData
	{
		// Token: 0x06002045 RID: 8261 RVA: 0x000359C4 File Offset: 0x00033BC4
		public void Set(bool autoUpdateAPI, string sourceNamespace = null, string sourceAssembly = null, string sourceClassName = null)
		{
			this.className = sourceClassName;
			this.classHasChanged = this.className != null;
			this.nameSpace = sourceNamespace;
			this.nameSpaceHasChanged = this.nameSpace != null;
			this.assembly = sourceAssembly;
			this.assemblyHasChanged = this.assembly != null;
			this.autoUdpateAPI = autoUpdateAPI;
		}

		// Token: 0x04000AB2 RID: 2738
		public string className;

		// Token: 0x04000AB3 RID: 2739
		public string nameSpace;

		// Token: 0x04000AB4 RID: 2740
		public string assembly;

		// Token: 0x04000AB5 RID: 2741
		public bool classHasChanged;

		// Token: 0x04000AB6 RID: 2742
		public bool nameSpaceHasChanged;

		// Token: 0x04000AB7 RID: 2743
		public bool assemblyHasChanged;

		// Token: 0x04000AB8 RID: 2744
		public bool autoUdpateAPI;
	}
}
