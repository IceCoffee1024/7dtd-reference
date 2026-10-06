using System;
using System.IO;
using UnityEngine;

// Token: 0x0200143E RID: 5182
public interface IBinaryReaderOrWriter
{
	// Token: 0x1700133E RID: 4926
	// (get) Token: 0x0600A281 RID: 41601
	Stream BaseStream { get; }

	// Token: 0x0600A282 RID: 41602
	bool ReadWrite(bool _value);

	// Token: 0x0600A283 RID: 41603
	byte ReadWrite(byte _value);

	// Token: 0x0600A284 RID: 41604
	sbyte ReadWrite(sbyte _value);

	// Token: 0x0600A285 RID: 41605
	char ReadWrite(char _value);

	// Token: 0x0600A286 RID: 41606
	short ReadWrite(short _value);

	// Token: 0x0600A287 RID: 41607
	ushort ReadWrite(ushort _value);

	// Token: 0x0600A288 RID: 41608
	int ReadWrite(int _value);

	// Token: 0x0600A289 RID: 41609
	uint ReadWrite(uint _value);

	// Token: 0x0600A28A RID: 41610
	long ReadWrite(long _value);

	// Token: 0x0600A28B RID: 41611
	ulong ReadWrite(ulong _value);

	// Token: 0x0600A28C RID: 41612
	float ReadWrite(float _value);

	// Token: 0x0600A28D RID: 41613
	double ReadWrite(double _value);

	// Token: 0x0600A28E RID: 41614
	decimal ReadWrite(decimal _value);

	// Token: 0x0600A28F RID: 41615
	string ReadWrite(string _value);

	// Token: 0x0600A290 RID: 41616
	void ReadWrite(byte[] _buffer, int _index, int _count);

	// Token: 0x0600A291 RID: 41617
	Vector3 ReadWrite(Vector3 _value);
}
