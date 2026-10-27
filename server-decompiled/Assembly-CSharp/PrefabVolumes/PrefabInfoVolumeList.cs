using System;
using System.Collections.Generic;
using System.Text;

namespace PrefabVolumes
{
	// Token: 0x0200190F RID: 6415
	public class PrefabInfoVolumeList : PrefabVolumeListAbs<PrefabInfoVolumeList, PrefabInfoVolume>
	{
		// Token: 0x0600C59A RID: 50586 RVA: 0x0048A26B File Offset: 0x0048846B
		public PrefabInfoVolumeList(Prefab _owner)
			: base(_owner)
		{
		}

		// Token: 0x170018E6 RID: 6374
		// (get) Token: 0x0600C59B RID: 50587 RVA: 0x0002F260 File Offset: 0x0002D460
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Info;
			}
		}

		// Token: 0x170018E7 RID: 6375
		// (get) Token: 0x0600C59C RID: 50588 RVA: 0x0048A274 File Offset: 0x00488474
		public override SelectionCategory SelectionCategory
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SelectionBoxManager.Instance.CategoryInfoVolume;
			}
		}

		// Token: 0x0600C59D RID: 50589 RVA: 0x0048A280 File Offset: 0x00488480
		public override void ReadFromProperties(DynamicProperties _properties)
		{
			this.List.Clear();
			Dictionary<string, string> values = _properties.Values;
			if (!values.ContainsKey("InfoVolumeSize") || !values.ContainsKey("InfoVolumeStart"))
			{
				return;
			}
			List<Vector3i> list = StringParsers.ParseList<Vector3i>(values["InfoVolumeSize"], '#', (string _s, int _start, int _end) => StringParsers.ParseVector3i(_s, _start, _end, false));
			List<Vector3i> list2 = StringParsers.ParseList<Vector3i>(values["InfoVolumeStart"], '#', (string _s, int _start, int _end) => StringParsers.ParseVector3i(_s, _start, _end, false));
			for (int i = 0; i < list2.Count; i++)
			{
				Vector3i vector3i = list2[i];
				Vector3i vector3i2 = ((i < list.Count) ? list[i] : Vector3i.one);
				PrefabInfoVolume prefabInfoVolume = new PrefabInfoVolume();
				prefabInfoVolume.Use(vector3i, vector3i2);
				this.List.Add(prefabInfoVolume);
			}
		}

		// Token: 0x0600C59E RID: 50590 RVA: 0x0048A370 File Offset: 0x00488570
		public override void WriteToProperties(DynamicProperties _properties)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (PrefabInfoVolume prefabInfoVolume in this.List)
			{
				if (prefabInfoVolume.Used)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append('#');
						stringBuilder2.Append('#');
					}
					stringBuilder.Append(prefabInfoVolume.size.ToString());
					stringBuilder2.Append(prefabInfoVolume.startPos.ToString());
				}
			}
			if (stringBuilder.Length > 0)
			{
				_properties.Values["InfoVolumeSize"] = stringBuilder.ToString();
				_properties.Values["InfoVolumeStart"] = stringBuilder2.ToString();
				return;
			}
			_properties.Values.Remove("InfoVolumeSize");
			_properties.Values.Remove("InfoVolumeStart");
		}
	}
}
