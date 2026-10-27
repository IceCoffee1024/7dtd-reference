using System;
using System.IO;
using UnityEngine;

// Token: 0x02001450 RID: 5200
public interface IBinaryReaderOrWriter
{
	// Token: 0x1700134A RID: 4938
	// (get) Token: 0x0600A2D9 RID: 41689
	Stream BaseStream { get; }

	// Token: 0x0600A2DA RID: 41690
	bool ReadWrite(bool _value);

	// Token: 0x0600A2DB RID: 41691
	byte ReadWrite(byte _value);

	// Token: 0x0600A2DC RID: 41692
	sbyte ReadWrite(sbyte _value);

	// Token: 0x0600A2DD RID: 41693
	char ReadWrite(char _value);

	// Token: 0x0600A2DE RID: 41694
	short ReadWrite(short _value);

	// Token: 0x0600A2DF RID: 41695
	ushort ReadWrite(ushort _value);

	// Token: 0x0600A2E0 RID: 41696
	int ReadWrite(int _value);

	// Token: 0x0600A2E1 RID: 41697
	uint ReadWrite(uint _value);

	// Token: 0x0600A2E2 RID: 41698
	long ReadWrite(long _value);

	// Token: 0x0600A2E3 RID: 41699
	ulong ReadWrite(ulong _value);

	// Token: 0x0600A2E4 RID: 41700
	float ReadWrite(float _value);

	// Token: 0x0600A2E5 RID: 41701
	double ReadWrite(double _value);

	// Token: 0x0600A2E6 RID: 41702
	decimal ReadWrite(decimal _value);

	// Token: 0x0600A2E7 RID: 41703
	string ReadWrite(string _value);

	// Token: 0x0600A2E8 RID: 41704
	void ReadWrite(byte[] _buffer, int _index, int _count);

	// Token: 0x0600A2E9 RID: 41705
	Vector3 ReadWrite(Vector3 _value);
}
