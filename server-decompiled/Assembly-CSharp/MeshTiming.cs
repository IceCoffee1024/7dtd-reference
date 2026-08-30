using System;

// Token: 0x020003C1 RID: 961
public class MeshTiming
{
	// Token: 0x06001CCD RID: 7373 RVA: 0x000ADFA0 File Offset: 0x000AC1A0
	public string Details()
	{
		double num = this.CopyVerts + this.CopyUv + this.CopyUv2 + this.CopyUv3 + this.CopyUv4 + this.CopyColours + this.CopyTriangles + this.CopyNormals + this.CopyTangents + this.UploadMesh + this.NormalRecalc;
		return string.Format("\r\nCopyVerts: {0}\r\nCopyUv:{1}\r\nCopyUv2:{2}\r\nCopyUv3:{3}\r\nCopyUv4:{4}\r\nCopyColours:{5}\r\nCopyTriangles:{6}\r\nCopyNormals:{7}\r\nCopyTangents:{8}\r\nUploadMesh:{9}\r\nNormalRecalc:{10}\r\nTotal: {11}\r\n", new object[]
		{
			this.CopyVerts, this.CopyUv, this.CopyUv2, this.CopyUv3, this.CopyUv4, this.CopyColours, this.CopyTriangles, this.CopyNormals, this.CopyTangents, this.UploadMesh,
			this.NormalRecalc, num
		});
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x06001CCE RID: 7374 RVA: 0x000AE0B1 File Offset: 0x000AC2B1
	public double time
	{
		get
		{
			return this.GetTime();
		}
	}

	// Token: 0x06001CCF RID: 7375 RVA: 0x000AE0BC File Offset: 0x000AC2BC
	public double GetTime()
	{
		return (DateTime.Now - this.Start).TotalMilliseconds;
	}

	// Token: 0x06001CD0 RID: 7376 RVA: 0x000AE0E1 File Offset: 0x000AC2E1
	public void Reset()
	{
		this.Start = DateTime.Now;
	}

	// Token: 0x040012C3 RID: 4803
	public double CopyVerts;

	// Token: 0x040012C4 RID: 4804
	public double CopyUv;

	// Token: 0x040012C5 RID: 4805
	public double CopyUv2;

	// Token: 0x040012C6 RID: 4806
	public double CopyUv3;

	// Token: 0x040012C7 RID: 4807
	public double CopyUv4;

	// Token: 0x040012C8 RID: 4808
	public double CopyColours;

	// Token: 0x040012C9 RID: 4809
	public double CopyTriangles;

	// Token: 0x040012CA RID: 4810
	public double CopyNormals;

	// Token: 0x040012CB RID: 4811
	public double CopyTangents;

	// Token: 0x040012CC RID: 4812
	public double UploadMesh;

	// Token: 0x040012CD RID: 4813
	public double NormalRecalc;

	// Token: 0x040012CE RID: 4814
	public DateTime Start;
}
