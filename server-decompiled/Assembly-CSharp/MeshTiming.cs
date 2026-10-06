using System;

// Token: 0x020003C4 RID: 964
public class MeshTiming
{
	// Token: 0x06001CC6 RID: 7366 RVA: 0x000AE1E0 File Offset: 0x000AC3E0
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
	// (get) Token: 0x06001CC7 RID: 7367 RVA: 0x000AE2F1 File Offset: 0x000AC4F1
	public double time
	{
		get
		{
			return this.GetTime();
		}
	}

	// Token: 0x06001CC8 RID: 7368 RVA: 0x000AE2FC File Offset: 0x000AC4FC
	public double GetTime()
	{
		return (DateTime.Now - this.Start).TotalMilliseconds;
	}

	// Token: 0x06001CC9 RID: 7369 RVA: 0x000AE321 File Offset: 0x000AC521
	public void Reset()
	{
		this.Start = DateTime.Now;
	}

	// Token: 0x040012BC RID: 4796
	public double CopyVerts;

	// Token: 0x040012BD RID: 4797
	public double CopyUv;

	// Token: 0x040012BE RID: 4798
	public double CopyUv2;

	// Token: 0x040012BF RID: 4799
	public double CopyUv3;

	// Token: 0x040012C0 RID: 4800
	public double CopyUv4;

	// Token: 0x040012C1 RID: 4801
	public double CopyColours;

	// Token: 0x040012C2 RID: 4802
	public double CopyTriangles;

	// Token: 0x040012C3 RID: 4803
	public double CopyNormals;

	// Token: 0x040012C4 RID: 4804
	public double CopyTangents;

	// Token: 0x040012C5 RID: 4805
	public double UploadMesh;

	// Token: 0x040012C6 RID: 4806
	public double NormalRecalc;

	// Token: 0x040012C7 RID: 4807
	public DateTime Start;
}
