using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TextFx
{
	[AddComponentMenu("UI/TextFx Text", 12)]
	public class TextFxUGUI : Text, TextFxAnimationInterface
	{
		public enum UGUI_MESH_EFFECT_TYPE
		{
			None = 0,
			Shadow = 1,
			Outline = 2,
			Outline8 = 3
		}

		public class UGUITextDataHandler : TextFxAnimationManager.GuiTextDataHandler
		{
			private List<UIVertex> m_vertData;

			private int m_numBaseLetters;

			private int m_extraVertsPerLetter;

			private int m_totalVertsPerLetter;

			public int NumVerts
			{
				get
				{
					return m_vertData.Count;
				}
			}

			public int ExtraVertsPerLetter
			{
				get
				{
					return m_extraVertsPerLetter;
				}
			}

			public int NumVertsPerLetter
			{
				get
				{
					return m_totalVertsPerLetter;
				}
			}

			public UGUITextDataHandler(List<UIVertex> vertData, int numBaseLetterMeshes)
			{
				m_vertData = vertData;
				m_numBaseLetters = numBaseLetterMeshes;
				int numVerts = NumVerts;
				if (numVerts > m_numBaseLetters * 4)
				{
					m_extraVertsPerLetter = (numVerts - m_numBaseLetters * 4) / m_numBaseLetters;
				}
				else
				{
					m_extraVertsPerLetter = 0;
				}
				m_totalVertsPerLetter = ((m_numBaseLetters > 0) ? (numVerts / m_numBaseLetters) : 0);
			}

			public Vector3[] GetLetterBaseVerts(int letterIndex)
			{
				Vector3[] array = new Vector3[4];
				int num = m_numBaseLetters * m_extraVertsPerLetter;
				for (int i = 0; i < 4; i++)
				{
					if (num + letterIndex * 4 + i < m_vertData.Count)
					{
						array[i] = m_vertData[num + letterIndex * 4 + i].position;
					}
					else
					{
						array[i] = Vector3.zero;
					}
				}
				return array;
			}

			public Color[] GetLetterBaseCols(int letterIndex)
			{
				Color[] array = new Color[4];
				int num = m_numBaseLetters * m_extraVertsPerLetter;
				for (int i = 0; i < 4; i++)
				{
					if (num + letterIndex * 4 + i < m_vertData.Count)
					{
						array[i] = m_vertData[num + letterIndex * 4 + i].color;
					}
					else
					{
						array[i] = Color.clear;
					}
				}
				return array;
			}

			public Vector3[] GetLetterExtraVerts(int letterIndex)
			{
				Vector3[] array = null;
				if (m_extraVertsPerLetter > 0)
				{
					array = new Vector3[m_extraVertsPerLetter];
					int num = letterIndex * m_extraVertsPerLetter;
					for (int i = 0; i < m_extraVertsPerLetter; i++)
					{
						array[i] = m_vertData[num + i].position;
					}
				}
				return array;
			}

			public Color[] GetLetterExtraCols(int letterIndex)
			{
				Color[] array = null;
				if (m_extraVertsPerLetter > 0)
				{
					array = new Color[m_extraVertsPerLetter];
					int num = letterIndex * m_extraVertsPerLetter;
					for (int i = 0; i < m_extraVertsPerLetter; i++)
					{
						array[i] = m_vertData[num + i].color;
					}
				}
				return array;
			}
		}

		[HideInInspector]
		[SerializeField]
		private TextFxAnimationManager m_animation_manager;

		[HideInInspector]
		[SerializeField]
		private GameObject m_gameobject_reference;

		[SerializeField]
		private bool m_renderToCurve;

		[SerializeField]
		private TextFxBezierCurve m_bezierCurve;

		public UGUI_MESH_EFFECT_TYPE m_effect_type;

		public Vector2 m_effect_offset = new Vector2(1f, 1f);

		public Color m_effect_colour = Color.black;

		[HideInInspector]
		private List<UIVertex> m_cachedVerts;

		private List<UIVertex> m_currentMeshVerts = new List<UIVertex>();

		private bool m_textFxUpdateGeometryCall;

		private bool m_textFxAnimDrawCall;

		private int _numLetterMeshes;

		private UIVertex _temp_vert = default(UIVertex);

		private UIVertex[] _uiVertexQuad;

		public string AssetNameSuffix
		{
			get
			{
				return "_UGUI";
			}
		}

		public float MovementScale
		{
			get
			{
				return 26f;
			}
		}

		public int LayerOverride
		{
			get
			{
				return 5;
			}
		}

		public TEXTFX_IMPLEMENTATION TextFxImplementation
		{
			get
			{
				return TEXTFX_IMPLEMENTATION.UGUI;
			}
		}

		public TextAlignment TextAlignment
		{
			get
			{
				switch (base.alignment)
				{
				case TextAnchor.UpperCenter:
				case TextAnchor.MiddleCenter:
				case TextAnchor.LowerCenter:
					return TextAlignment.Center;
				case TextAnchor.UpperRight:
				case TextAnchor.MiddleRight:
				case TextAnchor.LowerRight:
					return TextAlignment.Right;
				default:
					return TextAlignment.Left;
				}
			}
		}

		public bool FlippedMeshVerts
		{
			get
			{
				return false;
			}
		}

		public TextFxAnimationManager AnimationManager
		{
			get
			{
				return m_animation_manager;
			}
		}

		public GameObject GameObject
		{
			get
			{
				if (m_gameobject_reference == null)
				{
					m_gameobject_reference = base.gameObject;
				}
				return m_gameobject_reference;
			}
		}

		public bool CurvePositioningEnabled
		{
			get
			{
				return true;
			}
		}

		public bool MeshEffectsSupported
		{
			get
			{
				return true;
			}
		}

		public bool RenderToCurve
		{
			get
			{
				return m_renderToCurve;
			}
			set
			{
				m_renderToCurve = value;
			}
		}

		public TextFxBezierCurve BezierCurve
		{
			get
			{
				return m_bezierCurve;
			}
			set
			{
				m_bezierCurve = value;
				m_animation_manager.CheckCurveData();
				ForceUpdateCachedVertData();
				UpdateTextFxMesh();
			}
		}

		public UnityEngine.Object ObjectInstance
		{
			get
			{
				return this;
			}
		}

		public Action OnMeshUpdateCall { get; set; }

		public int NumMeshVerts
		{
			get
			{
				return m_currentMeshVerts.Count;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (m_animation_manager == null)
			{
				m_animation_manager = new TextFxAnimationManager();
			}
			m_animation_manager.SetParentObjectReferences(base.gameObject, base.transform, this);
		}

		protected override void Start()
		{
			if (Application.isPlaying)
			{
				m_animation_manager.OnStart();
			}
		}

		private void Update()
		{
			if (Application.isPlaying && m_animation_manager.Playing)
			{
				m_animation_manager.UpdateAnimation();
				TextFxUpdateGeometry();
			}
		}

		public void ForceUpdateCachedVertData()
		{
			m_animation_manager.PopulateDefaultMeshData(true);
			for (int i = 0; i < m_animation_manager.MeshVerts.Length; i++)
			{
				UIVertex value = m_cachedVerts[i];
				value.position = m_animation_manager.MeshVerts[i];
				m_cachedVerts[i] = value;
			}
		}

		public void ForceUpdateGeometry()
		{
			UpdateGeometry();
		}

		public void UpdateTextFxMesh()
		{
			TextFxUpdateGeometry();
		}

		public void SetText(string new_text)
		{
			text = new_text;
			m_animation_manager.SetDataRebuildCallFrame();
		}

		public void SetColour(Color colour)
		{
			color = colour;
			m_animation_manager.SetDataRebuildCallFrame();
		}

		public Vector3 GetMeshVert(int index)
		{
			if (m_currentMeshVerts.Count <= index)
			{
				Debug.LogWarning("Requested vertex index '" + index + "' is out of range");
				return Vector3.zero;
			}
			return m_currentMeshVerts[index].position;
		}

		public Color GetMeshColour(int index)
		{
			if (m_currentMeshVerts.Count <= index)
			{
				Debug.LogWarning("Requested vert colour index '" + index + "' is out of range");
				return Color.white;
			}
			return m_currentMeshVerts[index].color;
		}

		private void TextFxUpdateGeometry()
		{
			m_textFxUpdateGeometryCall = true;
			UpdateGeometry();
		}

		protected override void UpdateGeometry()
		{
			if (m_textFxUpdateGeometryCall)
			{
				m_textFxAnimDrawCall = true;
			}
			else
			{
				m_textFxAnimDrawCall = false;
			}
			m_textFxUpdateGeometryCall = false;
			base.UpdateGeometry();
		}

		private void AddEffectMeshes(VertexHelper vHelper)
		{
			if (m_effect_type == UGUI_MESH_EFFECT_TYPE.None)
			{
				return;
			}
			int num = vHelper.currentVertCount / 4;
			int num2 = 1;
			switch (m_effect_type)
			{
			case UGUI_MESH_EFFECT_TYPE.Outline:
				num2 = 4;
				break;
			case UGUI_MESH_EFFECT_TYPE.Outline8:
				num2 = 8;
				break;
			}
			List<UIVertex[]> list = new List<UIVertex[]>();
			List<UIVertex[]> list2 = new List<UIVertex[]>();
			UIVertex vertex = default(UIVertex);
			UIVertex[] array = new UIVertex[4];
			UIVertex[] array2 = new UIVertex[4];
			Vector2 vector = m_effect_offset * base.fontSize / 65f;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					if (j == 0)
					{
						array = new UIVertex[4];
					}
					array2 = new UIVertex[4];
					for (int k = 0; k < 4; k++)
					{
						vHelper.PopulateUIVertex(ref vertex, i * 4 + k);
						if (j == 0)
						{
							array[k] = vertex;
						}
						int num3 = 0;
						int num4 = 0;
						switch (j)
						{
						case 0:
							num3 = 1;
							num4 = 1;
							break;
						case 1:
							num3 = -1;
							num4 = 1;
							break;
						case 2:
							num3 = 1;
							num4 = -1;
							break;
						case 3:
							num3 = -1;
							num4 = -1;
							break;
						case 4:
							num3 = -1;
							num4 = 0;
							break;
						case 5:
							num3 = 1;
							num4 = 0;
							break;
						case 6:
							num3 = 0;
							num4 = 1;
							break;
						case 7:
							num3 = 0;
							num4 = -1;
							break;
						}
						array2[k] = vertex;
						array2[k].position += new Vector3(vector.x * (float)num3, vector.y * (float)num4, 0f);
						array2[k].color = m_effect_colour;
					}
					if (j == 0)
					{
						list.Add(array);
					}
					list2.Add(array2);
				}
			}
			vHelper.Clear();
			for (int l = 0; l < num; l++)
			{
				for (int m = 0; m < num2; m++)
				{
					list2[l * num2 + m][0].color = m_effect_colour;
					list2[l * num2 + m][1].color = m_effect_colour;
					list2[l * num2 + m][2].color = m_effect_colour;
					list2[l * num2 + m][3].color = m_effect_colour;
					vHelper.AddUIVertexQuad(list2[l * num2 + m]);
				}
			}
			for (int n = 0; n < num; n++)
			{
				vHelper.AddUIVertexQuad(list[n]);
			}
		}

		protected override void OnPopulateMesh(VertexHelper vHelper)
		{
			if (base.font == null)
			{
				m_textFxAnimDrawCall = false;
				return;
			}
			if (!m_textFxAnimDrawCall || m_cachedVerts == null)
			{
				if (m_cachedVerts == null)
				{
					m_cachedVerts = new List<UIVertex>();
				}
				base.OnPopulateMesh(vHelper);
				_numLetterMeshes = vHelper.currentVertCount / 4;
				AddEffectMeshes(vHelper);
				m_cachedVerts.Clear();
				for (int i = 0; i < vHelper.currentVertCount; i++)
				{
					vHelper.PopulateUIVertex(ref _temp_vert, i);
					m_cachedVerts.Add(_temp_vert);
				}
				m_currentMeshVerts = m_cachedVerts.GetRange(0, m_cachedVerts.Count);
				m_animation_manager.UpdateText(text, new UGUITextDataHandler(m_cachedVerts, _numLetterMeshes), true);
				if (m_animation_manager.CheckCurveData())
				{
					ForceUpdateCachedVertData();
					vHelper.Clear();
					if (_uiVertexQuad == null || _uiVertexQuad.Length != 4)
					{
						_uiVertexQuad = new UIVertex[4];
					}
					for (int j = 0; j < m_cachedVerts.Count; j += 4)
					{
						_uiVertexQuad[0] = m_cachedVerts[j];
						_uiVertexQuad[1] = m_cachedVerts[j + 1];
						_uiVertexQuad[2] = m_cachedVerts[j + 2];
						_uiVertexQuad[3] = m_cachedVerts[j + 3];
						vHelper.AddUIVertexQuad(_uiVertexQuad);
						m_currentMeshVerts.Add(m_cachedVerts[j]);
						m_currentMeshVerts.Add(m_cachedVerts[j + 1]);
						m_currentMeshVerts.Add(m_cachedVerts[j + 2]);
						m_currentMeshVerts.Add(m_cachedVerts[j + 3]);
					}
				}
				if (Application.isPlaying || m_animation_manager.Playing)
				{
					m_textFxAnimDrawCall = true;
					m_animation_manager.UpdateMesh(true, true);
					OnPopulateMesh(vHelper);
					return;
				}
				m_animation_manager.PopulateDefaultMeshData(true);
			}
			else
			{
				vHelper.Clear();
				if (m_currentMeshVerts == null)
				{
					m_currentMeshVerts = new List<UIVertex>();
				}
				else
				{
					m_currentMeshVerts.Clear();
				}
				if (_uiVertexQuad == null || _uiVertexQuad.Length != 4)
				{
					_uiVertexQuad = new UIVertex[4];
				}
				for (int k = 0; k < m_cachedVerts.Count; k += 4)
				{
					_uiVertexQuad[0] = m_cachedVerts[k];
					_uiVertexQuad[1] = m_cachedVerts[k + 1];
					_uiVertexQuad[2] = m_cachedVerts[k + 2];
					_uiVertexQuad[3] = m_cachedVerts[k + 3];
					vHelper.AddUIVertexQuad(_uiVertexQuad);
					m_currentMeshVerts.Add(m_cachedVerts[k]);
					m_currentMeshVerts.Add(m_cachedVerts[k + 1]);
					m_currentMeshVerts.Add(m_cachedVerts[k + 2]);
					m_currentMeshVerts.Add(m_cachedVerts[k + 3]);
					if (m_animation_manager.MeshVerts != null && k < m_animation_manager.MeshVerts.Length)
					{
						for (int l = 0; l < 4; l++)
						{
							vHelper.PopulateUIVertex(ref _temp_vert, k + l);
							_temp_vert.position = m_animation_manager.MeshVerts[k + l];
							_temp_vert.color = m_animation_manager.MeshColours[k + l];
							vHelper.SetUIVertex(_temp_vert, k + l);
							m_currentMeshVerts[k + l] = _temp_vert;
						}
					}
				}
			}
			m_textFxAnimDrawCall = false;
			if (OnMeshUpdateCall != null)
			{
				OnMeshUpdateCall();
			}
		}

		private new void OnDidApplyAnimationProperties()
		{
			base.OnDidApplyAnimationProperties();
			if (m_animation_manager.UsingBezierCurve)
			{
				UpdateGeometry();
			}
		}
	}
}
