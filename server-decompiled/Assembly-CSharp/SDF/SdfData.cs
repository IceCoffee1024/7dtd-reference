using System;
using System.Collections.Generic;

namespace SDF
{
	// Token: 0x020016C7 RID: 5831
	public class SdfData
	{
		// Token: 0x0600B6D2 RID: 46802 RVA: 0x0043CEE0 File Offset: 0x0043B0E0
		public SdfData()
		{
			this.Nodes = new Dictionary<string, SdfTag>();
		}

		// Token: 0x0600B6D3 RID: 46803 RVA: 0x0043CEF4 File Offset: 0x0043B0F4
		public bool Add(SdfTag sdfTag)
		{
			if (this.Nodes.ContainsKey(sdfTag.Name))
			{
				this.Nodes[sdfTag.Name].Value = sdfTag.Value;
			}
			else
			{
				this.Nodes.Add(sdfTag.Name, sdfTag);
			}
			return true;
		}

		// Token: 0x0600B6D4 RID: 46804 RVA: 0x0043CF45 File Offset: 0x0043B145
		public bool Remove(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return false;
			}
			this.Nodes.Remove(tagName);
			return true;
		}

		// Token: 0x0600B6D5 RID: 46805 RVA: 0x0043CF68 File Offset: 0x0043B168
		public int? GetInt(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.Int)
			{
				return null;
			}
			return new int?(Convert.ToInt32(this.Nodes[tagName].Value));
		}

		// Token: 0x0600B6D6 RID: 46806 RVA: 0x0043CFC8 File Offset: 0x0043B1C8
		public float? GetFloat(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.Float)
			{
				return null;
			}
			return new float?((float)this.Nodes[tagName].Value);
		}

		// Token: 0x0600B6D7 RID: 46807 RVA: 0x0043D026 File Offset: 0x0043B226
		public string GetString(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.String)
			{
				return null;
			}
			return this.Nodes[tagName].Value.ToString();
		}

		// Token: 0x0600B6D8 RID: 46808 RVA: 0x0043D064 File Offset: 0x0043B264
		public bool? GetBool(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.Bool)
			{
				return null;
			}
			return new bool?((bool)this.Nodes[tagName].Value);
		}

		// Token: 0x0600B6D9 RID: 46809 RVA: 0x0043D0C2 File Offset: 0x0043B2C2
		public string GetBinary(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.Binary)
			{
				return null;
			}
			return (string)this.Nodes[tagName].Value;
		}

		// Token: 0x0600B6DA RID: 46810 RVA: 0x0043D100 File Offset: 0x0043B300
		public byte[] GetByteArray(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				throw new KeyNotFoundException();
			}
			if (this.Nodes[tagName].TagType == SdfTagType.ByteArray)
			{
				throw new InvalidCastException();
			}
			return (byte[])this.Nodes[tagName].Value;
		}

		// Token: 0x04008884 RID: 34948
		public Dictionary<string, SdfTag> Nodes;
	}
}
