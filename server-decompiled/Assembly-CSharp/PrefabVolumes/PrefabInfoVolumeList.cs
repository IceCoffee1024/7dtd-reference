using System;
using System.Collections.Generic;
using System.Text;

namespace PrefabVolumes
{
	// Token: 0x020018DE RID: 6366
	public class PrefabInfoVolumeList : PrefabVolumeListAbs<PrefabInfoVolumeList, PrefabInfoVolume>
	{
		// Token: 0x0600C486 RID: 50310 RVA: 0x004868BF File Offset: 0x00484ABF
		public PrefabInfoVolumeList(Prefab _owner)
			: base(_owner)
		{
		}

		// Token: 0x1700189E RID: 6302
		// (get) Token: 0x0600C487 RID: 50311 RVA: 0x0002F260 File Offset: 0x0002D460
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Info;
			}
		}

		// Token: 0x1700189F RID: 6303
		// (get) Token: 0x0600C488 RID: 50312 RVA: 0x004868C8 File Offset: 0x00484AC8
		public override SelectionCategory SelectionCategory
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SelectionBoxManager.Instance.CategoryInfoVolume;
			}
		}

		// Token: 0x0600C489 RID: 50313 RVA: 0x004868D4 File Offset: 0x00484AD4
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

		// Token: 0x0600C48A RID: 50314 RVA: 0x004869C4 File Offset: 0x00484BC4
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
