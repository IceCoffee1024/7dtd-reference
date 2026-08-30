using System;

namespace SharpEXR
{
	// Token: 0x020016CB RID: 5835
	public interface IEXRReader : IDisposable
	{
		// Token: 0x0600B664 RID: 46692
		byte ReadByte();

		// Token: 0x0600B665 RID: 46693
		int ReadInt32();

		// Token: 0x0600B666 RID: 46694
		uint ReadUInt32();

		// Token: 0x0600B667 RID: 46695
		Half ReadHalf();

		// Token: 0x0600B668 RID: 46696
		float ReadSingle();

		// Token: 0x0600B669 RID: 46697
		double ReadDouble();

		// Token: 0x0600B66A RID: 46698
		string ReadNullTerminatedString(int maxLength);

		// Token: 0x0600B66B RID: 46699
		string ReadString(int length);

		// Token: 0x0600B66C RID: 46700
		string ReadString();

		// Token: 0x0600B66D RID: 46701
		byte[] ReadBytes(int count);

		// Token: 0x0600B66E RID: 46702
		void CopyBytes(byte[] dest, int offset, int count);

		// Token: 0x1700164B RID: 5707
		// (get) Token: 0x0600B66F RID: 46703
		// (set) Token: 0x0600B670 RID: 46704
		int Position { get; set; }
	}
}
