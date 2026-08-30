using System;
using System.Collections.Generic;
using System.IO;

namespace SDF
{
	// Token: 0x02001691 RID: 5777
	public class SdfFile
	{
		// Token: 0x0600B552 RID: 46418 RVA: 0x00438272 File Offset: 0x00436472
		public SdfFile(string path)
		{
			this.data = new SdfData();
			this.filePath = path;
		}

		// Token: 0x0600B553 RID: 46419 RVA: 0x0043828C File Offset: 0x0043648C
		public void Load()
		{
			try
			{
				this.valuesChanged = false;
				if (SdFile.Exists(this.filePath))
				{
					using (Stream stream = SdFile.Open(this.filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						this.data.Nodes = SdfReader.Read(stream);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error opening SDF file: " + ex.Message);
			}
		}

		// Token: 0x0600B554 RID: 46420 RVA: 0x00438314 File Offset: 0x00436514
		public void Save()
		{
			try
			{
				if (this.valuesChanged)
				{
					if (!SdDirectory.Exists(Path.GetDirectoryName(this.filePath)))
					{
						SdDirectory.CreateDirectory(Path.GetDirectoryName(this.filePath));
					}
					using (Stream stream = SdFile.Open(this.filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
					{
						SdfWriter.Write(stream, this.data.Nodes);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error opening SDF file:");
				Log.Exception(ex);
			}
		}

		// Token: 0x0600B555 RID: 46421 RVA: 0x004383AC File Offset: 0x004365AC
		public void Set(string name, int val)
		{
			this.data.Add(new SdfInt(name, val));
			this.valuesChanged = true;
		}

		// Token: 0x0600B556 RID: 46422 RVA: 0x004383C8 File Offset: 0x004365C8
		public void Set(string name, float val)
		{
			this.data.Add(new SdfFloat(name, val));
			this.valuesChanged = true;
		}

		// Token: 0x0600B557 RID: 46423 RVA: 0x004383E4 File Offset: 0x004365E4
		public void Set(string name, string val)
		{
			this.Set(name, val, false);
		}

		// Token: 0x0600B558 RID: 46424 RVA: 0x004383EF File Offset: 0x004365EF
		public void Set(string name, string val, bool isBinary)
		{
			if (!isBinary)
			{
				this.data.Add(new SdfString(name, val));
			}
			else
			{
				this.data.Add(new SdfBinary(name, val));
			}
			this.valuesChanged = true;
		}

		// Token: 0x0600B559 RID: 46425 RVA: 0x00438423 File Offset: 0x00436623
		public void Set(string name, byte[] byteArray)
		{
			this.data.Add(new SdfByteArray(name, byteArray));
		}

		// Token: 0x0600B55A RID: 46426 RVA: 0x00438438 File Offset: 0x00436638
		public void Set(string name, bool val)
		{
			this.data.Add(new SdfBool(name, val));
			this.valuesChanged = true;
		}

		// Token: 0x0600B55B RID: 46427 RVA: 0x00438454 File Offset: 0x00436654
		public float? GetFloat(string name)
		{
			return this.data.GetFloat(name);
		}

		// Token: 0x0600B55C RID: 46428 RVA: 0x00438462 File Offset: 0x00436662
		public int? GetInt(string name)
		{
			return this.data.GetInt(name);
		}

		// Token: 0x0600B55D RID: 46429 RVA: 0x00438470 File Offset: 0x00436670
		public string GetString(string name)
		{
			return this.GetString(name, false);
		}

		// Token: 0x0600B55E RID: 46430 RVA: 0x0043847A File Offset: 0x0043667A
		public string GetString(string name, bool isBinary)
		{
			if (!isBinary)
			{
				return this.data.GetString(name);
			}
			return Utils.FromBase64(this.data.GetString(name));
		}

		// Token: 0x0600B55F RID: 46431 RVA: 0x0043849D File Offset: 0x0043669D
		public byte[] GetByteArray(string name)
		{
			return this.data.GetByteArray(name);
		}

		// Token: 0x0600B560 RID: 46432 RVA: 0x004384AB File Offset: 0x004366AB
		public bool? GetBool(string name)
		{
			return this.data.GetBool(name);
		}

		// Token: 0x0600B561 RID: 46433 RVA: 0x004384B9 File Offset: 0x004366B9
		public void Remove(string name)
		{
			this.data.Remove(name);
			this.valuesChanged = true;
		}

		// Token: 0x0600B562 RID: 46434 RVA: 0x004384D0 File Offset: 0x004366D0
		public string[] GetKeys()
		{
			string[] array = new string[this.data.Nodes.Count];
			this.data.Nodes.CopyKeysTo<string, SdfTag>(array);
			return array;
		}

		// Token: 0x0600B563 RID: 46435 RVA: 0x00438508 File Offset: 0x00436708
		public string[] GetStoredGamePrefs()
		{
			string[] array = new string[this.data.Nodes.Count];
			this.data.Nodes.CopyKeysTo<string, SdfTag>(array);
			return array;
		}

		// Token: 0x0600B564 RID: 46436 RVA: 0x00438540 File Offset: 0x00436740
		public void CopyFrom(SdfFile other)
		{
			foreach (KeyValuePair<string, SdfTag> keyValuePair in other.data.Nodes)
			{
				string text;
				SdfTag sdfTag;
				keyValuePair.Deconstruct(out text, out sdfTag);
				string text2 = text;
				SdfTag sdfTag2 = sdfTag;
				this.data.Nodes[text2] = sdfTag2;
			}
			this.valuesChanged = true;
		}

		// Token: 0x040087B9 RID: 34745
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SdfData data;

		// Token: 0x040087BA RID: 34746
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string filePath;

		// Token: 0x040087BB RID: 34747
		[PublicizedFrom(EAccessModifier.Private)]
		public bool valuesChanged;
	}
}
