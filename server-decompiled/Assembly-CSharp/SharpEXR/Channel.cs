using System;

namespace SharpEXR
{
	// Token: 0x020016EC RID: 5868
	public class Channel
	{
		// Token: 0x170016D1 RID: 5841
		// (get) Token: 0x0600B755 RID: 46933 RVA: 0x0043EC72 File Offset: 0x0043CE72
		// (set) Token: 0x0600B756 RID: 46934 RVA: 0x0043EC7A File Offset: 0x0043CE7A
		public string Name { get; set; }

		// Token: 0x170016D2 RID: 5842
		// (get) Token: 0x0600B757 RID: 46935 RVA: 0x0043EC83 File Offset: 0x0043CE83
		// (set) Token: 0x0600B758 RID: 46936 RVA: 0x0043EC8B File Offset: 0x0043CE8B
		public PixelType Type { get; set; }

		// Token: 0x170016D3 RID: 5843
		// (get) Token: 0x0600B759 RID: 46937 RVA: 0x0043EC94 File Offset: 0x0043CE94
		// (set) Token: 0x0600B75A RID: 46938 RVA: 0x0043EC9C File Offset: 0x0043CE9C
		public bool Linear { get; set; }

		// Token: 0x170016D4 RID: 5844
		// (get) Token: 0x0600B75B RID: 46939 RVA: 0x0043ECA5 File Offset: 0x0043CEA5
		// (set) Token: 0x0600B75C RID: 46940 RVA: 0x0043ECAD File Offset: 0x0043CEAD
		public int XSampling { get; set; }

		// Token: 0x170016D5 RID: 5845
		// (get) Token: 0x0600B75D RID: 46941 RVA: 0x0043ECB6 File Offset: 0x0043CEB6
		// (set) Token: 0x0600B75E RID: 46942 RVA: 0x0043ECBE File Offset: 0x0043CEBE
		public int YSampling { get; set; }

		// Token: 0x170016D6 RID: 5846
		// (get) Token: 0x0600B75F RID: 46943 RVA: 0x0043ECC7 File Offset: 0x0043CEC7
		// (set) Token: 0x0600B760 RID: 46944 RVA: 0x0043ECCF File Offset: 0x0043CECF
		public byte[] Reserved { get; set; }

		// Token: 0x0600B761 RID: 46945 RVA: 0x0043ECD8 File Offset: 0x0043CED8
		public Channel(string name, PixelType type, bool linear, int xSampling, int ySampling)
			: this(name, type, linear, 0, 0, 0, xSampling, ySampling)
		{
		}

		// Token: 0x0600B762 RID: 46946 RVA: 0x0043ECF5 File Offset: 0x0043CEF5
		public Channel(string name, PixelType type, bool linear, byte reserved0, byte reserved1, byte reserved2, int xSampling, int ySampling)
		{
			this.Name = name;
			this.Type = type;
			this.Linear = linear;
			this.Reserved = new byte[] { reserved0, reserved1, reserved2 };
		}

		// Token: 0x0600B763 RID: 46947 RVA: 0x0043ED2D File Offset: 0x0043CF2D
		public override string ToString()
		{
			return string.Format("{0} {1} {2}", base.GetType().Name, this.Name, this.Type);
		}
	}
}
