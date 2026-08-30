using System;
using System.Collections.Generic;

namespace SDF
{
	// Token: 0x0200169B RID: 5787
	public class SdfData
	{
		// Token: 0x0600B57B RID: 46459 RVA: 0x004389C0 File Offset: 0x00436BC0
		public SdfData()
		{
			this.Nodes = new Dictionary<string, SdfTag>();
		}

		// Token: 0x0600B57C RID: 46460 RVA: 0x004389D4 File Offset: 0x00436BD4
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

		// Token: 0x0600B57D RID: 46461 RVA: 0x00438A25 File Offset: 0x00436C25
		public bool Remove(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return false;
			}
			this.Nodes.Remove(tagName);
			return true;
		}

		// Token: 0x0600B57E RID: 46462 RVA: 0x00438A48 File Offset: 0x00436C48
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

		// Token: 0x0600B57F RID: 46463 RVA: 0x00438AA8 File Offset: 0x00436CA8
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

		// Token: 0x0600B580 RID: 46464 RVA: 0x00438B06 File Offset: 0x00436D06
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

		// Token: 0x0600B581 RID: 46465 RVA: 0x00438B44 File Offset: 0x00436D44
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

		// Token: 0x0600B582 RID: 46466 RVA: 0x00438BA2 File Offset: 0x00436DA2
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

		// Token: 0x0600B583 RID: 46467 RVA: 0x00438BE0 File Offset: 0x00436DE0
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

		// Token: 0x040087BF RID: 34751
		public Dictionary<string, SdfTag> Nodes;
	}
}
