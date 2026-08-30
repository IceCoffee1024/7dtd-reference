using System;

namespace SharpEXR
{
	// Token: 0x020016C0 RID: 5824
	public class Channel
	{
		// Token: 0x1700162F RID: 5679
		// (get) Token: 0x0600B5FE RID: 46590 RVA: 0x0043A752 File Offset: 0x00438952
		// (set) Token: 0x0600B5FF RID: 46591 RVA: 0x0043A75A File Offset: 0x0043895A
		public string Name { get; set; }

		// Token: 0x17001630 RID: 5680
		// (get) Token: 0x0600B600 RID: 46592 RVA: 0x0043A763 File Offset: 0x00438963
		// (set) Token: 0x0600B601 RID: 46593 RVA: 0x0043A76B File Offset: 0x0043896B
		public PixelType Type { get; set; }

		// Token: 0x17001631 RID: 5681
		// (get) Token: 0x0600B602 RID: 46594 RVA: 0x0043A774 File Offset: 0x00438974
		// (set) Token: 0x0600B603 RID: 46595 RVA: 0x0043A77C File Offset: 0x0043897C
		public bool Linear { get; set; }

		// Token: 0x17001632 RID: 5682
		// (get) Token: 0x0600B604 RID: 46596 RVA: 0x0043A785 File Offset: 0x00438985
		// (set) Token: 0x0600B605 RID: 46597 RVA: 0x0043A78D File Offset: 0x0043898D
		public int XSampling { get; set; }

		// Token: 0x17001633 RID: 5683
		// (get) Token: 0x0600B606 RID: 46598 RVA: 0x0043A796 File Offset: 0x00438996
		// (set) Token: 0x0600B607 RID: 46599 RVA: 0x0043A79E File Offset: 0x0043899E
		public int YSampling { get; set; }

		// Token: 0x17001634 RID: 5684
		// (get) Token: 0x0600B608 RID: 46600 RVA: 0x0043A7A7 File Offset: 0x004389A7
		// (set) Token: 0x0600B609 RID: 46601 RVA: 0x0043A7AF File Offset: 0x004389AF
		public byte[] Reserved { get; set; }

		// Token: 0x0600B60A RID: 46602 RVA: 0x0043A7B8 File Offset: 0x004389B8
		public Channel(string name, PixelType type, bool linear, int xSampling, int ySampling)
			: this(name, type, linear, 0, 0, 0, xSampling, ySampling)
		{
		}

		// Token: 0x0600B60B RID: 46603 RVA: 0x0043A7D5 File Offset: 0x004389D5
		public Channel(string name, PixelType type, bool linear, byte reserved0, byte reserved1, byte reserved2, int xSampling, int ySampling)
		{
			this.Name = name;
			this.Type = type;
			this.Linear = linear;
			this.Reserved = new byte[] { reserved0, reserved1, reserved2 };
		}

		// Token: 0x0600B60C RID: 46604 RVA: 0x0043A80D File Offset: 0x00438A0D
		public override string ToString()
		{
			return string.Format("{0} {1} {2}", base.GetType().Name, this.Name, this.Type);
		}
	}
}
