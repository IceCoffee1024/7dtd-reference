using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001298 RID: 4760
public class NGuiHUDText : MonoBehaviour
{
	// Token: 0x0600971A RID: 38682 RVA: 0x0038C42C File Offset: 0x0038A62C
	[PublicizedFrom(EAccessModifier.Private)]
	public static int Comparison(NGuiHUDText.Entry a, NGuiHUDText.Entry b)
	{
		if (a.movementStart < b.movementStart)
		{
			return -1;
		}
		if (a.movementStart > b.movementStart)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x170011F5 RID: 4597
	// (get) Token: 0x0600971B RID: 38683 RVA: 0x0038C44F File Offset: 0x0038A64F
	public bool isVisible
	{
		get
		{
			return this.mList.Count != 0;
		}
	}

	// Token: 0x170011F6 RID: 4598
	// (get) Token: 0x0600971C RID: 38684 RVA: 0x0038C45F File Offset: 0x0038A65F
	// (set) Token: 0x0600971D RID: 38685 RVA: 0x0038C467 File Offset: 0x0038A667
	public INGUIFont ambigiousFont
	{
		get
		{
			return this.font;
		}
		set
		{
			this.font = value;
		}
	}

	// Token: 0x0600971E RID: 38686 RVA: 0x0038C470 File Offset: 0x0038A670
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiHUDText.Entry Create()
	{
		if (this.mUnused.Count > 0)
		{
			NGuiHUDText.Entry entry = this.mUnused[this.mUnused.Count - 1];
			this.mUnused.RemoveAt(this.mUnused.Count - 1);
			entry.time = Time.realtimeSinceStartup;
			entry.label.depth = NGUITools.CalculateNextDepth(base.gameObject);
			NGUITools.SetActive(entry.label.gameObject, true);
			entry.intialOffset = default(Vector3);
			entry.curveOffset = 0f;
			this.mList.Add(entry);
			return entry;
		}
		NGuiHUDText.Entry entry2 = new NGuiHUDText.Entry();
		entry2.time = Time.realtimeSinceStartup;
		entry2.label = base.gameObject.AddWidget<UILabel>(int.MaxValue);
		entry2.label.name = string.Format("Entry_{0}_Label", this.counter);
		entry2.label.font = this.ambigiousFont;
		entry2.label.fontSize = this.fontSize;
		entry2.label.fontStyle = this.fontStyle;
		entry2.label.applyGradient = this.applyGradient;
		entry2.label.gradientTop = this.gradientTop;
		entry2.label.gradientBottom = this.gradienBottom;
		entry2.label.effectStyle = this.effect;
		entry2.label.effectColor = this.effectColor;
		entry2.label.overflowMethod = UILabel.Overflow.ResizeFreely;
		entry2.label.cachedTransform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
		entry2.isLabel = true;
		entry2.sprite = base.gameObject.AddWidget<UISprite>(int.MaxValue);
		entry2.sprite.name = string.Format("Entry_{0}_Sprite", this.counter);
		entry2.sprite.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnHeight;
		this.mList.Add(entry2);
		this.counter++;
		return entry2;
	}

	// Token: 0x0600971F RID: 38687 RVA: 0x0038C679 File Offset: 0x0038A879
	[PublicizedFrom(EAccessModifier.Private)]
	public void Delete(NGuiHUDText.Entry ent)
	{
		this.mList.Remove(ent);
		this.mUnused.Add(ent);
		NGUITools.SetActive(ent.label.gameObject, false);
	}

	// Token: 0x06009720 RID: 38688 RVA: 0x0038C6A8 File Offset: 0x0038A8A8
	public void Add(object obj, Color c, float stayDuration)
	{
		if (!base.enabled)
		{
			return;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		bool flag = false;
		float num = 0f;
		if (obj is float)
		{
			flag = true;
			num = (float)obj;
		}
		else if (obj is int)
		{
			flag = true;
			num = (float)((int)obj);
		}
		if (flag)
		{
			if (num == 0f)
			{
				return;
			}
			int i = this.mList.Count;
			while (i > 0)
			{
				NGuiHUDText.Entry entry = this.mList[--i];
				if (entry.time + 1f >= realtimeSinceStartup && entry.val != 0f)
				{
					if (entry.val < 0f && num < 0f)
					{
						entry.val += num;
						entry.label.text = Mathf.RoundToInt(entry.val).ToString();
						return;
					}
					if (entry.val > 0f && num > 0f)
					{
						entry.val += num;
						entry.label.text = "+" + Mathf.RoundToInt(entry.val).ToString();
						return;
					}
				}
			}
		}
		NGuiHUDText.Entry entry2 = this.Create();
		entry2.stay = stayDuration;
		entry2.label.color = c;
		entry2.label.alpha = 0f;
		entry2.sprite.color = c;
		entry2.val = num;
		if (flag)
		{
			entry2.label.text = ((num < 0f) ? Mathf.RoundToInt(entry2.val).ToString() : ("+" + Mathf.RoundToInt(entry2.val).ToString()));
		}
		else
		{
			entry2.label.text = obj.ToString();
		}
		this.mList.Sort(new Comparison<NGuiHUDText.Entry>(NGuiHUDText.Comparison));
	}

	// Token: 0x06009721 RID: 38689 RVA: 0x0038C89C File Offset: 0x0038AA9C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnEnable()
	{
		if (this.ambigiousFont == null)
		{
			foreach (NGUIFont nguifont in Resources.FindObjectsOfTypeAll<NGUIFont>())
			{
				if (nguifont.name.EqualsCaseInsensitive("ReferenceFont"))
				{
					this.ambigiousFont = nguifont;
					break;
				}
			}
			if (this.ambigiousFont == null)
			{
				Log.Error("NGuiHUDText font not found");
			}
		}
		this.fontStyle = this.font.dynamicFontStyle;
	}

	// Token: 0x06009722 RID: 38690 RVA: 0x0038C908 File Offset: 0x0038AB08
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnValidate()
	{
		INGUIFont ambigiousFont = this.ambigiousFont;
		if (ambigiousFont != null && ambigiousFont.isDynamic)
		{
			this.fontStyle = ambigiousFont.dynamicFontStyle;
			this.fontSize = ambigiousFont.defaultSize;
		}
	}

	// Token: 0x06009723 RID: 38691 RVA: 0x0038C940 File Offset: 0x0038AB40
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnDisable()
	{
		int i = this.mList.Count;
		while (i > 0)
		{
			NGuiHUDText.Entry entry = this.mList[--i];
			if (entry.label != null)
			{
				entry.label.enabled = false;
			}
			else
			{
				this.mList.RemoveAt(i);
			}
		}
	}

	// Token: 0x06009724 RID: 38692 RVA: 0x0038C998 File Offset: 0x0038AB98
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		float time = RealTime.time;
		Keyframe[] keys = this.offsetCurve.keys;
		Keyframe[] keys2 = this.alphaCurve.keys;
		Keyframe[] keys3 = this.scaleCurve.keys;
		float time2 = keys[keys.Length - 1].time;
		float time3 = keys2[keys2.Length - 1].time;
		float num = Mathf.Max(keys3[keys3.Length - 1].time, Mathf.Max(time2, time3));
		int i = this.mList.Count;
		while (i > 0)
		{
			NGuiHUDText.Entry entry = this.mList[--i];
			float num2 = time - entry.movementStart;
			entry.curveOffset = this.offsetCurve.Evaluate(num2);
			entry.label.alpha = this.alphaCurve.Evaluate(num2);
			float num3 = this.scaleCurve.Evaluate(time - entry.time);
			if (num3 < 0.001f)
			{
				num3 = 0.001f;
			}
			entry.label.cachedTransform.localScale = new Vector3(num3, num3, num3);
			if (num2 > num)
			{
				this.Delete(entry);
			}
			else
			{
				entry.label.enabled = true;
			}
		}
		float num4 = 0f;
		float num5 = 0f;
		for (int j = 0; j < this.mList.Count; j++)
		{
			NGuiHUDText.Entry entry2 = this.mList[j];
			if (this.verticalStack)
			{
				num5 += (float)(entry2.isLabel ? entry2.label.height : entry2.sprite.height);
				num4 = Mathf.Max(num4, (float)(entry2.isLabel ? entry2.label.width : entry2.sprite.width));
			}
			else
			{
				num4 += (float)(entry2.isLabel ? entry2.label.width : entry2.sprite.width);
				num5 = Mathf.Max(num5, (float)(entry2.isLabel ? entry2.label.height : entry2.sprite.height));
			}
		}
		if (this.verticalStack)
		{
			float num6 = 0f;
			for (int k = 0; k < this.mList.Count; k++)
			{
				NGuiHUDText.Entry entry3 = this.mList[k];
				num6 = Mathf.Max(num6, entry3.curveOffset);
				if (entry3.isLabel)
				{
					entry3.label.cachedTransform.localPosition = new Vector3(0f, num6, 0f) + entry3.intialOffset * num5;
					num6 += Mathf.Round(entry3.label.cachedTransform.localScale.y * (float)entry3.label.fontSize);
				}
				else
				{
					entry3.sprite.cachedTransform.localPosition = new Vector3(0f, num6, 0f) + entry3.intialOffset * num5;
					num6 += Mathf.Round(entry3.sprite.cachedTransform.localScale.y * (float)entry3.sprite.height);
				}
			}
			return;
		}
		float num7 = 0f;
		for (int l = 0; l < this.mList.Count; l++)
		{
			NGuiHUDText.Entry entry4 = this.mList[l];
			if (entry4.isLabel)
			{
				entry4.label.cachedTransform.localPosition = new Vector3(num7 + ((float)entry4.label.width - num4) / 2f, entry4.curveOffset, 0f) + entry4.intialOffset * num5;
				num7 += (float)entry4.label.width;
			}
			else
			{
				entry4.sprite.cachedTransform.localPosition = new Vector3(num7 + ((float)entry4.sprite.width - num4) / 2f, entry4.curveOffset, 0f) + entry4.intialOffset * num5;
				num7 += (float)entry4.sprite.width;
			}
		}
	}

	// Token: 0x06009725 RID: 38693 RVA: 0x0038CDEC File Offset: 0x0038AFEC
	public void SetEntry(int _index, string _input, bool _isSprite, INGUIAtlas _spriteAtlas = null)
	{
		if (this.mList.Count <= _index)
		{
			return;
		}
		this.mList[_index].isLabel = !_isSprite;
		if (!_isSprite)
		{
			this.mList[_index].label.text = _input;
			this.mList[_index].sprite.atlas = null;
			this.mList[_index].sprite.spriteName = string.Empty;
			return;
		}
		this.mList[_index].label.text = string.Empty;
		this.mList[_index].sprite.atlas = (string.IsNullOrEmpty(_input) ? null : _spriteAtlas);
		this.mList[_index].sprite.spriteName = _input;
	}

	// Token: 0x06009726 RID: 38694 RVA: 0x0038CEBF File Offset: 0x0038B0BF
	public void SetEntrySize(int _index, int _size)
	{
		this.mList[_index].label.fontSize = _size;
		this.mList[_index].sprite.height = _size;
	}

	// Token: 0x06009727 RID: 38695 RVA: 0x0038CEEF File Offset: 0x0038B0EF
	public void SetEntryOffset(int _index, Vector3 _offset)
	{
		this.mList[_index].intialOffset = _offset;
	}

	// Token: 0x06009728 RID: 38696 RVA: 0x0038CF03 File Offset: 0x0038B103
	public void SetEntryColor(int _index, Color _c)
	{
		this.mList[_index].label.color = _c;
		this.mList[_index].sprite.color = _c;
	}

	// Token: 0x04007110 RID: 28944
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string uiFontName = "ReferenceFont";

	// Token: 0x04007111 RID: 28945
	[HideInInspector]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public INGUIFont font;

	// Token: 0x04007112 RID: 28946
	public int fontSize = 20;

	// Token: 0x04007113 RID: 28947
	public FontStyle fontStyle;

	// Token: 0x04007114 RID: 28948
	public bool applyGradient;

	// Token: 0x04007115 RID: 28949
	public Color gradientTop = Color.white;

	// Token: 0x04007116 RID: 28950
	public Color gradienBottom = new Color(0.7f, 0.7f, 0.7f);

	// Token: 0x04007117 RID: 28951
	public UILabel.Effect effect;

	// Token: 0x04007118 RID: 28952
	public Color effectColor = Color.black;

	// Token: 0x04007119 RID: 28953
	public bool verticalStack;

	// Token: 0x0400711A RID: 28954
	public AnimationCurve offsetCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(3f, 40f)
	});

	// Token: 0x0400711B RID: 28955
	public AnimationCurve alphaCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(1f, 1f),
		new Keyframe(3f, 0f)
	});

	// Token: 0x0400711C RID: 28956
	public AnimationCurve scaleCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 1f)
	});

	// Token: 0x0400711D RID: 28957
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<NGuiHUDText.Entry> mList = new List<NGuiHUDText.Entry>();

	// Token: 0x0400711E RID: 28958
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<NGuiHUDText.Entry> mUnused = new List<NGuiHUDText.Entry>();

	// Token: 0x0400711F RID: 28959
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int counter;

	// Token: 0x02001299 RID: 4761
	[PublicizedFrom(EAccessModifier.Protected)]
	public class Entry
	{
		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x0600972A RID: 38698 RVA: 0x0038D04C File Offset: 0x0038B24C
		public float movementStart
		{
			get
			{
				return this.time + this.stay;
			}
		}

		// Token: 0x04007120 RID: 28960
		public float time;

		// Token: 0x04007121 RID: 28961
		public float stay;

		// Token: 0x04007122 RID: 28962
		public Vector3 intialOffset;

		// Token: 0x04007123 RID: 28963
		public float curveOffset;

		// Token: 0x04007124 RID: 28964
		public float val;

		// Token: 0x04007125 RID: 28965
		public UILabel label;

		// Token: 0x04007126 RID: 28966
		public UISprite sprite;

		// Token: 0x04007127 RID: 28967
		public bool isLabel;
	}
}
