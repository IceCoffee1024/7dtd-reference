using System;

namespace SharpEXR
{
	// Token: 0x02001716 RID: 5910
	public interface IEXRReader : IDisposable
	{
		// Token: 0x0600B88F RID: 47247
		byte ReadByte();

		// Token: 0x0600B890 RID: 47248
		int ReadInt32();

		// Token: 0x0600B891 RID: 47249
		uint ReadUInt32();

		// Token: 0x0600B892 RID: 47250
		Half ReadHalf();

		// Token: 0x0600B893 RID: 47251
		float ReadSingle();

		// Token: 0x0600B894 RID: 47252
		double ReadDouble();

		// Token: 0x0600B895 RID: 47253
		string ReadNullTerminatedString(int maxLength);

		// Token: 0x0600B896 RID: 47254
		string ReadString(int length);

		// Token: 0x0600B897 RID: 47255
		string ReadString();

		// Token: 0x0600B898 RID: 47256
		byte[] ReadBytes(int count);

		// Token: 0x0600B899 RID: 47257
		void CopyBytes(byte[] dest, int offset, int count);

		// Token: 0x17001734 RID: 5940
		// (get) Token: 0x0600B89A RID: 47258
		// (set) Token: 0x0600B89B RID: 47259
		int Position { get; set; }
	}
}
