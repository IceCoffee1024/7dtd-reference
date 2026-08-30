using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000260 RID: 608
	[NativeHeader("Runtime/Scripting/TextAsset.h")]
	public class TextAsset : Object
	{
		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x0600198E RID: 6542
		public extern byte[] bytes
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600198F RID: 6543
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern byte[] GetPreviewBytes(int maxByteCount);

		// Token: 0x06001990 RID: 6544
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateInstance([Writable] TextAsset self, string text);

		// Token: 0x06001991 RID: 6545
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern IntPtr GetDataPtr();

		// Token: 0x06001992 RID: 6546
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern long GetDataSize();

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001993 RID: 6547 RVA: 0x0002ADAC File Offset: 0x00028FAC
		public string text
		{
			get
			{
				byte[] bytes = this.bytes;
				return (bytes.Length == 0) ? string.Empty : TextAsset.DecodeString(bytes);
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001994 RID: 6548 RVA: 0x0002ADD6 File Offset: 0x00028FD6
		public long dataSize
		{
			get
			{
				return this.GetDataSize();
			}
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x0002ADE0 File Offset: 0x00028FE0
		public override string ToString()
		{
			return this.text;
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x0002ADF8 File Offset: 0x00028FF8
		public TextAsset()
			: this(TextAsset.CreateOptions.CreateNativeObject, null)
		{
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x0002AE04 File Offset: 0x00029004
		public TextAsset(string text)
			: this(TextAsset.CreateOptions.CreateNativeObject, text)
		{
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x0002AE10 File Offset: 0x00029010
		internal TextAsset(TextAsset.CreateOptions options, string text)
		{
			bool flag = options == TextAsset.CreateOptions.CreateNativeObject;
			if (flag)
			{
				TextAsset.Internal_CreateInstance(this, text);
			}
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0002AE38 File Offset: 0x00029038
		public unsafe NativeArray<T> GetData<T>() where T : struct
		{
			long dataSize = this.GetDataSize();
			long num = (long)UnsafeUtility.SizeOf<T>();
			bool flag = dataSize % num != 0L;
			if (flag)
			{
				throw new ArgumentException(string.Format("Type passed to {0} can't capture the asset data. Data size is {1} which is not a multiple of type size {2}", "GetData", dataSize, num));
			}
			long num2 = dataSize / num;
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)this.GetDataPtr(), (int)num2, Allocator.None);
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x0002AEA4 File Offset: 0x000290A4
		internal string GetPreview(int maxChars)
		{
			return TextAsset.DecodeString(this.GetPreviewBytes(maxChars * 4));
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0002AEC4 File Offset: 0x000290C4
		internal static string DecodeString(byte[] bytes)
		{
			int num = TextAsset.EncodingUtility.encodingLookup.Length;
			int i = 0;
			int num2;
			while (i < num)
			{
				byte[] key = TextAsset.EncodingUtility.encodingLookup[i].Key;
				num2 = key.Length;
				bool flag = bytes.Length >= num2;
				if (flag)
				{
					for (int j = 0; j < num2; j++)
					{
						bool flag2 = key[j] != bytes[j];
						if (flag2)
						{
							num2 = -1;
						}
					}
					bool flag3 = num2 < 0;
					if (!flag3)
					{
						try
						{
							Encoding value = TextAsset.EncodingUtility.encodingLookup[i].Value;
							return value.GetString(bytes, num2, bytes.Length - num2);
						}
						catch
						{
						}
					}
				}
				IL_00A2:
				i++;
				continue;
				goto IL_00A2;
			}
			num2 = 0;
			Encoding targetEncoding = TextAsset.EncodingUtility.targetEncoding;
			return targetEncoding.GetString(bytes, num2, bytes.Length - num2);
		}

		// Token: 0x02000261 RID: 609
		internal enum CreateOptions
		{
			// Token: 0x040008EB RID: 2283
			None,
			// Token: 0x040008EC RID: 2284
			CreateNativeObject
		}

		// Token: 0x02000262 RID: 610
		private static class EncodingUtility
		{
			// Token: 0x0600199C RID: 6556 RVA: 0x0002AFB0 File Offset: 0x000291B0
			static EncodingUtility()
			{
				Encoding encoding = new UTF32Encoding(true, true, true);
				Encoding encoding2 = new UTF32Encoding(false, true, true);
				Encoding encoding3 = new UnicodeEncoding(true, true, true);
				Encoding encoding4 = new UnicodeEncoding(false, true, true);
				Encoding encoding5 = new UTF8Encoding(true, true);
				TextAsset.EncodingUtility.encodingLookup = new KeyValuePair<byte[], Encoding>[]
				{
					new KeyValuePair<byte[], Encoding>(encoding.GetPreamble(), encoding),
					new KeyValuePair<byte[], Encoding>(encoding2.GetPreamble(), encoding2),
					new KeyValuePair<byte[], Encoding>(encoding3.GetPreamble(), encoding3),
					new KeyValuePair<byte[], Encoding>(encoding4.GetPreamble(), encoding4),
					new KeyValuePair<byte[], Encoding>(encoding5.GetPreamble(), encoding5)
				};
			}

			// Token: 0x040008ED RID: 2285
			internal static readonly KeyValuePair<byte[], Encoding>[] encodingLookup;

			// Token: 0x040008EE RID: 2286
			internal static readonly Encoding targetEncoding = Encoding.GetEncoding(Encoding.UTF8.CodePage, new EncoderReplacementFallback("\ufffd"), new DecoderReplacementFallback("\ufffd"));
		}
	}
}
