using System;

namespace SharpEXR
{
	// Token: 0x0200169A RID: 5786
	public class Channel
	{
		// Token: 0x170015BD RID: 5565
		// (get) Token: 0x0600B47B RID: 46203 RVA: 0x004349DA File Offset: 0x00432BDA
		// (set) Token: 0x0600B47C RID: 46204 RVA: 0x004349E2 File Offset: 0x00432BE2
		public string Name { get; set; }

		// Token: 0x170015BE RID: 5566
		// (get) Token: 0x0600B47D RID: 46205 RVA: 0x004349EB File Offset: 0x00432BEB
		// (set) Token: 0x0600B47E RID: 46206 RVA: 0x004349F3 File Offset: 0x00432BF3
		public PixelType Type { get; set; }

		// Token: 0x170015BF RID: 5567
		// (get) Token: 0x0600B47F RID: 46207 RVA: 0x004349FC File Offset: 0x00432BFC
		// (set) Token: 0x0600B480 RID: 46208 RVA: 0x00434A04 File Offset: 0x00432C04
		public bool Linear { get; set; }

		// Token: 0x170015C0 RID: 5568
		// (get) Token: 0x0600B481 RID: 46209 RVA: 0x00434A0D File Offset: 0x00432C0D
		// (set) Token: 0x0600B482 RID: 46210 RVA: 0x00434A15 File Offset: 0x00432C15
		public int XSampling { get; set; }

		// Token: 0x170015C1 RID: 5569
		// (get) Token: 0x0600B483 RID: 46211 RVA: 0x00434A1E File Offset: 0x00432C1E
		// (set) Token: 0x0600B484 RID: 46212 RVA: 0x00434A26 File Offset: 0x00432C26
		public int YSampling { get; set; }

		// Token: 0x170015C2 RID: 5570
		// (get) Token: 0x0600B485 RID: 46213 RVA: 0x00434A2F File Offset: 0x00432C2F
		// (set) Token: 0x0600B486 RID: 46214 RVA: 0x00434A37 File Offset: 0x00432C37
		public byte[] Reserved { get; set; }

		// Token: 0x0600B487 RID: 46215 RVA: 0x00434A40 File Offset: 0x00432C40
		public Channel(string name, PixelType type, bool linear, int xSampling, int ySampling)
			: this(name, type, linear, 0, 0, 0, xSampling, ySampling)
		{
		}

		// Token: 0x0600B488 RID: 46216 RVA: 0x00434A5D File Offset: 0x00432C5D
		public Channel(string name, PixelType type, bool linear, byte reserved0, byte reserved1, byte reserved2, int xSampling, int ySampling)
		{
			this.Name = name;
			this.Type = type;
			this.Linear = linear;
			this.Reserved = new byte[] { reserved0, reserved1, reserved2 };
		}

		// Token: 0x0600B489 RID: 46217 RVA: 0x00434A95 File Offset: 0x00432C95
		public override string ToString()
		{
			return string.Format("{0} {1} {2}", base.GetType().Name, this.Name, this.Type);
		}
	}
}
