using System;

// Token: 0x020003BD RID: 957
public class MeshTiming
{
	// Token: 0x06001CB0 RID: 7344 RVA: 0x000AD860 File Offset: 0x000ABA60
	public string Details()
	{
		double num = this.CopyVerts + this.CopyUv + this.CopyUv2 + this.CopyUv3 + this.CopyUv4 + this.CopyColours + this.CopyTriangles + this.CopyNormals + this.CopyTangents + this.UploadMesh + this.NormalRecalc;
		return string.Format("\r\nCopyVerts: {0}\r\nCopyUv:{1}\r\nCopyUv2:{2}\r\nCopyUv3:{3}\r\nCopyUv4:{4}\r\nCopyColours:{5}\r\nCopyTriangles:{6}\r\nCopyNormals:{7}\r\nCopyTangents:{8}\r\nUploadMesh:{9}\r\nNormalRecalc:{10}\r\nTotal: {11}\r\n", new object[]
		{
			this.CopyVerts, this.CopyUv, this.CopyUv2, this.CopyUv3, this.CopyUv4, this.CopyColours, this.CopyTriangles, this.CopyNormals, this.CopyTangents, this.UploadMesh,
			this.NormalRecalc, num
		});
	}

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x06001CB1 RID: 7345 RVA: 0x000AD971 File Offset: 0x000ABB71
	public double time
	{
		get
		{
			return this.GetTime();
		}
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x000AD97C File Offset: 0x000ABB7C
	public double GetTime()
	{
		return (DateTime.Now - this.Start).TotalMilliseconds;
	}

	// Token: 0x06001CB3 RID: 7347 RVA: 0x000AD9A1 File Offset: 0x000ABBA1
	public void Reset()
	{
		this.Start = DateTime.Now;
	}

	// Token: 0x040012AE RID: 4782
	public double CopyVerts;

	// Token: 0x040012AF RID: 4783
	public double CopyUv;

	// Token: 0x040012B0 RID: 4784
	public double CopyUv2;

	// Token: 0x040012B1 RID: 4785
	public double CopyUv3;

	// Token: 0x040012B2 RID: 4786
	public double CopyUv4;

	// Token: 0x040012B3 RID: 4787
	public double CopyColours;

	// Token: 0x040012B4 RID: 4788
	public double CopyTriangles;

	// Token: 0x040012B5 RID: 4789
	public double CopyNormals;

	// Token: 0x040012B6 RID: 4790
	public double CopyTangents;

	// Token: 0x040012B7 RID: 4791
	public double UploadMesh;

	// Token: 0x040012B8 RID: 4792
	public double NormalRecalc;

	// Token: 0x040012B9 RID: 4793
	public DateTime Start;
}
