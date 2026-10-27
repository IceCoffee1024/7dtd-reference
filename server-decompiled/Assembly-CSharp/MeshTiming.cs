using System;

// Token: 0x020003C6 RID: 966
public class MeshTiming
{
	// Token: 0x06001CCA RID: 7370 RVA: 0x000AE3F0 File Offset: 0x000AC5F0
	public string Details()
	{
		double num = this.CopyVerts + this.CopyUv + this.CopyUv2 + this.CopyUv3 + this.CopyUv4 + this.CopyColours + this.CopyTriangles + this.CopyNormals + this.CopyTangents + this.UploadMesh + this.NormalRecalc;
		return string.Format("\r\nCopyVerts: {0}\r\nCopyUv:{1}\r\nCopyUv2:{2}\r\nCopyUv3:{3}\r\nCopyUv4:{4}\r\nCopyColours:{5}\r\nCopyTriangles:{6}\r\nCopyNormals:{7}\r\nCopyTangents:{8}\r\nUploadMesh:{9}\r\nNormalRecalc:{10}\r\nTotal: {11}\r\n", new object[]
		{
			this.CopyVerts, this.CopyUv, this.CopyUv2, this.CopyUv3, this.CopyUv4, this.CopyColours, this.CopyTriangles, this.CopyNormals, this.CopyTangents, this.UploadMesh,
			this.NormalRecalc, num
		});
	}

	// Token: 0x17000360 RID: 864
	// (get) Token: 0x06001CCB RID: 7371 RVA: 0x000AE501 File Offset: 0x000AC701
	public double time
	{
		get
		{
			return this.GetTime();
		}
	}

	// Token: 0x06001CCC RID: 7372 RVA: 0x000AE50C File Offset: 0x000AC70C
	public double GetTime()
	{
		return (DateTime.Now - this.Start).TotalMilliseconds;
	}

	// Token: 0x06001CCD RID: 7373 RVA: 0x000AE531 File Offset: 0x000AC731
	public void Reset()
	{
		this.Start = DateTime.Now;
	}

	// Token: 0x040012CC RID: 4812
	public double CopyVerts;

	// Token: 0x040012CD RID: 4813
	public double CopyUv;

	// Token: 0x040012CE RID: 4814
	public double CopyUv2;

	// Token: 0x040012CF RID: 4815
	public double CopyUv3;

	// Token: 0x040012D0 RID: 4816
	public double CopyUv4;

	// Token: 0x040012D1 RID: 4817
	public double CopyColours;

	// Token: 0x040012D2 RID: 4818
	public double CopyTriangles;

	// Token: 0x040012D3 RID: 4819
	public double CopyNormals;

	// Token: 0x040012D4 RID: 4820
	public double CopyTangents;

	// Token: 0x040012D5 RID: 4821
	public double UploadMesh;

	// Token: 0x040012D6 RID: 4822
	public double NormalRecalc;

	// Token: 0x040012D7 RID: 4823
	public DateTime Start;
}
