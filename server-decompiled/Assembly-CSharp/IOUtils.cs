using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using Force.Crc32;

// Token: 0x02001424 RID: 5156
public static class IOUtils
{
	// Token: 0x0600A210 RID: 41488 RVA: 0x003CBFA0 File Offset: 0x003CA1A0
	public static byte[] CalcHashSync(string _filename, string _algorithmName = "MD5")
	{
		byte[] array;
		using (HashAlgorithm hashAlgorithm = HashAlgorithm.Create(_algorithmName))
		{
			using (Stream stream = SdFile.OpenRead(_filename))
			{
				array = hashAlgorithm.ComputeHash(stream);
			}
		}
		return array;
	}

	// Token: 0x0600A211 RID: 41489 RVA: 0x003CBFF8 File Offset: 0x003CA1F8
	public static IEnumerator CalcHashCoroutine(string _filename, Action<byte[]> _resultCallback, int _maxTimePerFrameMs = 5, byte[] _buffer = null, string _algorithmName = "MD5")
	{
		MicroStopwatch msw = new MicroStopwatch(true);
		using (HashAlgorithm hashAlgo = HashAlgorithm.Create(_algorithmName))
		{
			using (Stream stream = SdFile.OpenRead(_filename))
			{
				if (_buffer == null || _buffer.Length < 8192)
				{
					_buffer = new byte[32768];
				}
				int bufferSize = _buffer.Length;
				int bytesRead;
				do
				{
					bytesRead = stream.Read(_buffer, 0, bufferSize);
					if (bytesRead > 0)
					{
						hashAlgo.TransformBlock(_buffer, 0, bytesRead, null, 0);
					}
					if (msw.ElapsedMilliseconds >= (long)_maxTimePerFrameMs)
					{
						yield return null;
						msw.ResetAndRestart();
					}
				}
				while (bytesRead > 0);
				hashAlgo.TransformFinalBlock(_buffer, 0, 0);
				_resultCallback(hashAlgo.Hash);
			}
			Stream stream = null;
		}
		HashAlgorithm hashAlgo = null;
		yield break;
		yield break;
	}

	// Token: 0x0600A212 RID: 41490 RVA: 0x003CC024 File Offset: 0x003CA224
	public static IEnumerator CalcCrcCoroutine(string _filename, Action<uint> _resultCallback, int _maxTimePerFrameMs = 5, byte[] _buffer = null)
	{
		MicroStopwatch msw = new MicroStopwatch(true);
		using (Stream stream = SdFile.OpenRead(_filename))
		{
			if (_buffer == null || _buffer.Length < 8192)
			{
				_buffer = new byte[32768];
			}
			int bufferSize = _buffer.Length;
			uint crc = 0U;
			int bytesRead;
			do
			{
				bytesRead = stream.Read(_buffer, 0, bufferSize);
				if (bytesRead > 0)
				{
					crc = Crc32Algorithm.Append(crc, _buffer, 0, bytesRead);
				}
				if (msw.ElapsedMilliseconds >= (long)_maxTimePerFrameMs)
				{
					yield return null;
					msw.ResetAndRestart();
				}
			}
			while (bytesRead > 0);
			_resultCallback(crc);
		}
		Stream stream = null;
		yield break;
		yield break;
	}

	// Token: 0x0600A213 RID: 41491 RVA: 0x003CC048 File Offset: 0x003CA248
	public static uint HashUint(this Crc32Algorithm _crcInstance)
	{
		if (_crcInstance == null)
		{
			throw new ArgumentNullException("_crcInstance");
		}
		byte[] hash = _crcInstance.Hash;
		return (uint)(((int)hash[0] << 24) | ((int)hash[1] << 16) | ((int)hash[2] << 8) | (int)hash[3]);
	}

	// Token: 0x040079BC RID: 31164
	[PublicizedFrom(EAccessModifier.Private)]
	public const int minBufferSize = 8192;

	// Token: 0x040079BD RID: 31165
	public const int DefaultBufferSize = 32768;
}
