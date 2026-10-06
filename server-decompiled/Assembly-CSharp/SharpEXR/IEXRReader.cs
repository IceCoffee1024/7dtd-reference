using System;

namespace SharpEXR
{
	// Token: 0x020016F7 RID: 5879
	public interface IEXRReader : IDisposable
	{
		// Token: 0x0600B7BB RID: 47035
		byte ReadByte();

		// Token: 0x0600B7BC RID: 47036
		int ReadInt32();

		// Token: 0x0600B7BD RID: 47037
		uint ReadUInt32();

		// Token: 0x0600B7BE RID: 47038
		Half ReadHalf();

		// Token: 0x0600B7BF RID: 47039
		float ReadSingle();

		// Token: 0x0600B7C0 RID: 47040
		double ReadDouble();

		// Token: 0x0600B7C1 RID: 47041
		string ReadNullTerminatedString(int maxLength);

		// Token: 0x0600B7C2 RID: 47042
		string ReadString(int length);

		// Token: 0x0600B7C3 RID: 47043
		string ReadString();

		// Token: 0x0600B7C4 RID: 47044
		byte[] ReadBytes(int count);

		// Token: 0x0600B7C5 RID: 47045
		void CopyBytes(byte[] dest, int offset, int count);

		// Token: 0x170016ED RID: 5869
		// (get) Token: 0x0600B7C6 RID: 47046
		// (set) Token: 0x0600B7C7 RID: 47047
		int Position { get; set; }
	}
}
