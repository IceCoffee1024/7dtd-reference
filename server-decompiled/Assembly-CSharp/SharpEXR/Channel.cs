using System;

namespace SharpEXR
{
	// Token: 0x0200170B RID: 5899
	public class Channel
	{
		// Token: 0x17001718 RID: 5912
		// (get) Token: 0x0600B829 RID: 47145 RVA: 0x00441716 File Offset: 0x0043F916
		// (set) Token: 0x0600B82A RID: 47146 RVA: 0x0044171E File Offset: 0x0043F91E
		public string Name { get; set; }

		// Token: 0x17001719 RID: 5913
		// (get) Token: 0x0600B82B RID: 47147 RVA: 0x00441727 File Offset: 0x0043F927
		// (set) Token: 0x0600B82C RID: 47148 RVA: 0x0044172F File Offset: 0x0043F92F
		public PixelType Type { get; set; }

		// Token: 0x1700171A RID: 5914
		// (get) Token: 0x0600B82D RID: 47149 RVA: 0x00441738 File Offset: 0x0043F938
		// (set) Token: 0x0600B82E RID: 47150 RVA: 0x00441740 File Offset: 0x0043F940
		public bool Linear { get; set; }

		// Token: 0x1700171B RID: 5915
		// (get) Token: 0x0600B82F RID: 47151 RVA: 0x00441749 File Offset: 0x0043F949
		// (set) Token: 0x0600B830 RID: 47152 RVA: 0x00441751 File Offset: 0x0043F951
		public int XSampling { get; set; }

		// Token: 0x1700171C RID: 5916
		// (get) Token: 0x0600B831 RID: 47153 RVA: 0x0044175A File Offset: 0x0043F95A
		// (set) Token: 0x0600B832 RID: 47154 RVA: 0x00441762 File Offset: 0x0043F962
		public int YSampling { get; set; }

		// Token: 0x1700171D RID: 5917
		// (get) Token: 0x0600B833 RID: 47155 RVA: 0x0044176B File Offset: 0x0043F96B
		// (set) Token: 0x0600B834 RID: 47156 RVA: 0x00441773 File Offset: 0x0043F973
		public byte[] Reserved { get; set; }

		// Token: 0x0600B835 RID: 47157 RVA: 0x0044177C File Offset: 0x0043F97C
		public Channel(string name, PixelType type, bool linear, int xSampling, int ySampling)
			: this(name, type, linear, 0, 0, 0, xSampling, ySampling)
		{
		}

		// Token: 0x0600B836 RID: 47158 RVA: 0x00441799 File Offset: 0x0043F999
		public Channel(string name, PixelType type, bool linear, byte reserved0, byte reserved1, byte reserved2, int xSampling, int ySampling)
		{
			this.Name = name;
			this.Type = type;
			this.Linear = linear;
			this.Reserved = new byte[] { reserved0, reserved1, reserved2 };
		}

		// Token: 0x0600B837 RID: 47159 RVA: 0x004417D1 File Offset: 0x0043F9D1
		public override string ToString()
		{
			return string.Format("{0} {1} {2}", base.GetType().Name, this.Name, this.Type);
		}
	}
}
