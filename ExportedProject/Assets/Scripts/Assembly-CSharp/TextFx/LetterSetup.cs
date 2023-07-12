using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public class LetterSetup
	{
		public enum VertexPosition
		{
			TopLeft = 0,
			TopRight = 1,
			BottomRight = 2,
			BottomLeft = 3
		}

		[NonSerialized]
		private TextFxAnimationManager m_animation_manager_ref;

		[NonSerialized]
		private LetterAction m_current_letter_action;

		[SerializeField]
		private Vector3[] m_base_vertices;

		[SerializeField]
		private Vector3[] m_base_extra_vertices;

		[SerializeField]
		private bool m_using_curved_data;

		[SerializeField]
		private Vector3[] m_curve_base_vertices;

		[SerializeField]
		private Vector3[] m_curve_base_extra_vertices;

		[SerializeField]
		private VertexColour m_base_colours;

		[SerializeField]
		private Color[] m_base_extra_colours;

		[SerializeField]
		private int[] m_base_indexes;

		[SerializeField]
		private bool m_visible_character = true;

		[SerializeField]
		private bool m_stub_instance;

		[SerializeField]
		private int m_mesh_index = -1;

		[SerializeField]
		private float m_width;

		[SerializeField]
		private float m_height;

		public AnimationProgressionVariables m_progression_variables;

		private AnimationStateVariables m_anim_state_vars;

		private float m_action_timer;

		private float m_action_delay;

		private bool m_ignore_action_delay;

		private float m_action_duration;

		private AnimatePerOptions m_last_animate_per;

		[SerializeField]
		private Vector3[] m_current_animated_vertices;

		[SerializeField]
		private Color[] m_current_animated_colours;

		private Vector3 m_local_scale_from;

		private Vector3 m_local_scale_to;

		private Vector3 m_global_scale_from;

		private Vector3 m_global_scale_to;

		private Vector3 m_local_rotation_from;

		private Vector3 m_local_rotation_to;

		private Vector3 m_global_rotation_from;

		private Vector3 m_global_rotation_to;

		private Vector3 m_position_from;

		private Vector3 m_position_to;

		private Vector3 m_anchor_offset_from;

		private Vector3 m_anchor_offset_to;

		private VertexColour m_colour_from;

		private VertexColour m_colour_to;

		private VertexColour m_letter_colour;

		private Vector3 m_letter_position;

		private Vector3 m_letter_scale;

		private Vector3 m_letter_global_scale;

		private Vector3 m_anchor_offset;

		private Quaternion m_letter_rotation;

		private Quaternion m_letter_global_rotation;

		private LETTER_ANIMATION_STATE m_current_state;

		private bool m_flippedVerts;

		private ContinueType m_continueType;

		private Action<float> m_continueLetterCallback;

		private int m_continueActionIndexTrigger = -1;

		private int m_continuedLoopStartIndex = -1;

		private bool m_fastTrackLoops;

		[SerializeField]
		private Vector3 m_anchor_offset_middle_center;

		[SerializeField]
		private Vector3 m_active_anchor_offset_upper_left;

		[SerializeField]
		private Vector3 m_active_anchor_offset_upper_center;

		[SerializeField]
		private Vector3 m_active_anchor_offset_upper_right;

		[SerializeField]
		private Vector3 m_active_anchor_offset_middle_left;

		[SerializeField]
		private Vector3 m_active_anchor_offset_middle_center;

		[SerializeField]
		private Vector3 m_active_anchor_offset_middle_right;

		[SerializeField]
		private Vector3 m_active_anchor_offset_lower_left;

		[SerializeField]
		private Vector3 m_active_anchor_offset_lower_center;

		[SerializeField]
		private Vector3 m_active_anchor_offset_lower_right;

		private Quaternion m_rotationOffsetQuat;

		private Quaternion m_rotationOffsetQuatInverse;

		private int prev_action_idx;

		private float prev_delay;

		private float current_action_delay;

		private bool altered_delay;

		private float old_action_delay;

		public Vector3[] BaseVertices
		{
			get
			{
				return (!m_using_curved_data) ? m_base_vertices : m_curve_base_vertices;
			}
		}

		public Vector3 BaseVerticesTL
		{
			get
			{
				return ((!m_using_curved_data) ? m_base_vertices : m_curve_base_vertices)[m_base_indexes[0]];
			}
		}

		public Vector3 BaseVerticesTR
		{
			get
			{
				return ((!m_using_curved_data) ? m_base_vertices : m_curve_base_vertices)[m_base_indexes[1]];
			}
		}

		public Vector3 BaseVerticesBR
		{
			get
			{
				return ((!m_using_curved_data) ? m_base_vertices : m_curve_base_vertices)[m_base_indexes[2]];
			}
		}

		public Vector3 BaseVerticesBL
		{
			get
			{
				return ((!m_using_curved_data) ? m_base_vertices : m_curve_base_vertices)[m_base_indexes[3]];
			}
		}

		public Color LetterColourTL
		{
			get
			{
				return m_letter_colour.top_left;
			}
		}

		public Color LetterColourTR
		{
			get
			{
				return m_letter_colour.top_right;
			}
		}

		public Color LetterColourBR
		{
			get
			{
				return m_letter_colour.bottom_right;
			}
		}

		public Color LetterColourBL
		{
			get
			{
				return m_letter_colour.bottom_left;
			}
		}

		private int NumExtraVerts
		{
			get
			{
				return (m_current_animated_vertices.Length != 4) ? (m_current_animated_vertices.Length - 4) : 0;
			}
		}

		private bool AnimatedMeshDataAvailable
		{
			get
			{
				return m_current_animated_vertices != null && m_current_animated_vertices.Length > 0;
			}
		}

		public Color LetterMeshColourTL
		{
			get
			{
				return (!AnimatedMeshDataAvailable) ? Color.white : m_current_animated_colours[NumExtraVerts + m_base_indexes[0]];
			}
		}

		public Color LetterMeshColourTR
		{
			get
			{
				return (!AnimatedMeshDataAvailable) ? Color.white : m_current_animated_colours[NumExtraVerts + m_base_indexes[1]];
			}
		}

		public Color LetterMeshColourBR
		{
			get
			{
				return (!AnimatedMeshDataAvailable) ? Color.white : m_current_animated_colours[NumExtraVerts + m_base_indexes[2]];
			}
		}

		public Color LetterMeshColourBL
		{
			get
			{
				return (!AnimatedMeshDataAvailable) ? Color.white : m_current_animated_colours[NumExtraVerts + m_base_indexes[3]];
			}
		}

		public Vector3[] BaseExtraVertices
		{
			get
			{
				return (!m_using_curved_data) ? m_base_extra_vertices : m_curve_base_extra_vertices;
			}
		}

		public Vector3 BaseVerticesCenter
		{
			get
			{
				return m_anchor_offset_middle_center;
			}
		}

		public Vector3 ActiveBaseVerticesCenter
		{
			get
			{
				return m_active_anchor_offset_middle_center;
			}
		}

		public VertexColour BaseColour
		{
			get
			{
				return m_base_colours;
			}
		}

		public Color[] BaseExtraColours
		{
			get
			{
				return m_base_extra_colours;
			}
		}

		public AnimationStateVariables AnimStateVars
		{
			get
			{
				return m_anim_state_vars;
			}
		}

		public List<ActionLoopCycle> ActiveLoopCycles
		{
			get
			{
				return m_anim_state_vars.ActiveLoopCycles;
			}
		}

		public bool WaitingToSync
		{
			get
			{
				return m_anim_state_vars.m_waiting_to_sync;
			}
		}

		public int ActionIndex
		{
			get
			{
				return m_anim_state_vars.m_action_index;
			}
		}

		public bool InReverse
		{
			get
			{
				return m_anim_state_vars.m_reverse;
			}
		}

		public int ActionProgress
		{
			get
			{
				return m_anim_state_vars.m_action_index_progress;
			}
		}

		public bool Active
		{
			get
			{
				return m_anim_state_vars.m_active;
			}
			set
			{
				m_anim_state_vars.m_active = value;
			}
		}

		public int LetterIdx
		{
			get
			{
				return (m_progression_variables == null) ? (-1) : m_progression_variables.LetterValue;
			}
		}

		public bool VisibleCharacter
		{
			get
			{
				return m_visible_character;
			}
			set
			{
				m_visible_character = value;
			}
		}

		public bool StubInstance
		{
			get
			{
				return m_stub_instance;
			}
		}

		public int MeshIndex
		{
			get
			{
				return m_mesh_index;
			}
		}

		public LETTER_ANIMATION_STATE CurrentAnimationState
		{
			get
			{
				return m_current_state;
			}
		}

		public Vector3[] CurrentAnimatedVerts
		{
			get
			{
				return m_current_animated_vertices;
			}
		}

		public Color[] CurrentAnimatedCols
		{
			get
			{
				return m_current_animated_colours;
			}
		}

		public float Width
		{
			get
			{
				return m_width;
			}
		}

		public float Height
		{
			get
			{
				return m_height;
			}
		}

		public float WidthScaled
		{
			get
			{
				return m_width / m_animation_manager_ref.MovementScale;
			}
		}

		public float HeightScaled
		{
			get
			{
				return m_height / m_animation_manager_ref.MovementScale;
			}
		}

		public Vector3 Center
		{
			get
			{
				return ((m_animation_manager_ref == null) ? Quaternion.identity : m_animation_manager_ref.Transform.rotation) * Vector3.Scale(CenterLocal, (m_animation_manager_ref == null) ? Vector3.one : m_animation_manager_ref.Transform.lossyScale) + ((m_animation_manager_ref == null) ? Vector3.zero : m_animation_manager_ref.Transform.position);
			}
		}

		public Vector3 CenterLocal
		{
			get
			{
				if (m_current_animated_vertices != null && m_current_animated_vertices.Length == 4)
				{
					return (m_current_animated_vertices[0] + m_current_animated_vertices[1] + m_current_animated_vertices[2] + m_current_animated_vertices[3]) / 4f;
				}
				return m_active_anchor_offset_middle_center;
			}
		}

		public Vector3 TopLeft
		{
			get
			{
				return ((m_animation_manager_ref == null) ? Quaternion.identity : m_animation_manager_ref.Transform.rotation) * Vector3.Scale(TopLeftLocal, (m_animation_manager_ref == null) ? Vector3.one : m_animation_manager_ref.Transform.lossyScale) + ((m_animation_manager_ref == null) ? Vector3.zero : m_animation_manager_ref.Transform.position);
			}
		}

		public Vector3 TopLeftLocal
		{
			get
			{
				return GetAnimatedVertexPosition(VertexPosition.TopLeft);
			}
		}

		public Vector3 TopRight
		{
			get
			{
				return ((m_animation_manager_ref == null) ? Quaternion.identity : m_animation_manager_ref.Transform.rotation) * Vector3.Scale(TopRightLocal, (m_animation_manager_ref == null) ? Vector3.one : m_animation_manager_ref.Transform.lossyScale) + ((m_animation_manager_ref == null) ? Vector3.zero : m_animation_manager_ref.Transform.position);
			}
		}

		public Vector3 TopRightLocal
		{
			get
			{
				return GetAnimatedVertexPosition(VertexPosition.TopRight);
			}
		}

		public Vector3 BottomLeft
		{
			get
			{
				return ((m_animation_manager_ref == null) ? Quaternion.identity : m_animation_manager_ref.Transform.rotation) * Vector3.Scale(BottomLeftLocal, (m_animation_manager_ref == null) ? Vector3.one : m_animation_manager_ref.Transform.lossyScale) + ((m_animation_manager_ref == null) ? Vector3.zero : m_animation_manager_ref.Transform.position);
			}
		}

		public Vector3 BottomLeftLocal
		{
			get
			{
				return GetAnimatedVertexPosition(VertexPosition.BottomLeft);
			}
		}

		public Vector3 BottomRight
		{
			get
			{
				return ((m_animation_manager_ref == null) ? Quaternion.identity : m_animation_manager_ref.Transform.rotation) * Vector3.Scale(BottomRightLocal, (m_animation_manager_ref == null) ? Vector3.one : m_animation_manager_ref.Transform.lossyScale) + ((m_animation_manager_ref == null) ? Vector3.zero : m_animation_manager_ref.Transform.position);
			}
		}

		public Vector3 BottomRightLocal
		{
			get
			{
				return GetAnimatedVertexPosition(VertexPosition.BottomRight);
			}
		}

		public Quaternion Rotation
		{
			get
			{
				return ((m_animation_manager_ref == null) ? Quaternion.identity : m_animation_manager_ref.Transform.rotation) * m_letter_rotation;
			}
		}

		public Quaternion RotationLocal
		{
			get
			{
				return m_letter_rotation;
			}
		}

		public Vector3 Scale
		{
			get
			{
				return Vector3.Scale(m_letter_scale, (m_animation_manager_ref == null) ? Vector3.one : m_animation_manager_ref.Transform.lossyScale);
			}
		}

		public Vector3 ScaleLocal
		{
			get
			{
				return m_letter_scale;
			}
		}

		public Vector3 Normal
		{
			get
			{
				return Vector3.Cross(m_current_animated_vertices[1] - m_current_animated_vertices[0], m_current_animated_vertices[2] - m_current_animated_vertices[0]);
			}
		}

		public Vector3 UpVector
		{
			get
			{
				return GetAnimatedVertexPosition(VertexPosition.TopLeft) - GetAnimatedVertexPosition(VertexPosition.BottomLeft);
			}
		}

		public LetterSetup(TextFxAnimationManager animation_manager_ref, LetterSetup cloneFrom = null)
		{
			if (cloneFrom == null)
			{
				m_anim_state_vars = default(AnimationStateVariables);
				m_anim_state_vars.ActiveLoopCycles = new List<ActionLoopCycle>();
			}
			else
			{
				m_anim_state_vars = cloneFrom.AnimStateVars.Clone();
			}
			m_animation_manager_ref = animation_manager_ref;
			m_flippedVerts = m_animation_manager_ref.AnimationInterface.FlippedMeshVerts;
		}

		public Color BaseMeshColour(int meshVertIndex)
		{
			return m_base_colours[m_base_indexes[meshVertIndex]];
		}

		private Vector3 GetAnimatedVertexPosition(VertexPosition position)
		{
			switch (position)
			{
			case VertexPosition.TopLeft:
				return (m_current_animated_vertices == null) ? m_base_vertices[m_base_indexes[0]] : m_current_animated_vertices[m_base_indexes[0]];
			case VertexPosition.TopRight:
				return (m_current_animated_vertices == null) ? m_base_vertices[m_base_indexes[1]] : m_current_animated_vertices[m_base_indexes[1]];
			case VertexPosition.BottomRight:
				return (m_current_animated_vertices == null) ? m_base_vertices[m_base_indexes[2]] : m_current_animated_vertices[m_base_indexes[2]];
			case VertexPosition.BottomLeft:
				return (m_current_animated_vertices == null) ? m_base_vertices[m_base_indexes[3]] : m_current_animated_vertices[m_base_indexes[3]];
			default:
				return Vector3.zero;
			}
		}

		public void SetWordLineIndex(int word_idx, int line_num)
		{
			if (m_progression_variables == null)
			{
				m_progression_variables = new AnimationProgressionVariables(-1, word_idx, line_num);
				return;
			}
			m_progression_variables.WordValue = word_idx;
			m_progression_variables.LineValue = line_num;
		}

		public void SetAsStubInstance()
		{
			m_stub_instance = true;
			m_mesh_index = -1;
		}

		public void SetLetterData(Vector3[] mesh_verts, Color[] mesh_cols, Vector3[] extra_mesh_verts, Color[] extra_mesh_cols, int mesh_index)
		{
			m_base_vertices = mesh_verts;
			m_base_extra_vertices = extra_mesh_verts;
			m_base_colours = new VertexColour(Color.white);
			if (mesh_cols != null)
			{
				m_base_colours[0] = mesh_cols[0];
				m_base_colours[1] = mesh_cols[1];
				m_base_colours[2] = mesh_cols[2];
				m_base_colours[3] = mesh_cols[3];
			}
			m_base_extra_colours = extra_mesh_cols;
			m_stub_instance = false;
			m_using_curved_data = false;
			m_mesh_index = mesh_index;
			m_rotationOffsetQuat = Quaternion.identity;
			PreCalculateMeshInformation(m_flippedVerts);
		}

		public void OffsetLetterData(Vector3 positionOffset, Vector3 rotationOffset)
		{
			Vector3 vector = (m_base_vertices[0] + m_base_vertices[1] + m_base_vertices[2] + m_base_vertices[3]) / 4f;
			m_rotationOffsetQuat = Quaternion.Euler(rotationOffset);
			m_rotationOffsetQuatInverse = Quaternion.Inverse(m_rotationOffsetQuat);
			m_curve_base_vertices = new Vector3[4];
			for (int i = 0; i < m_base_vertices.Length; i++)
			{
				m_curve_base_vertices[i] = m_rotationOffsetQuat * (m_base_vertices[i] - vector) + positionOffset;
			}
			if (m_base_extra_vertices != null)
			{
				m_curve_base_extra_vertices = new Vector3[m_base_extra_vertices.Length];
				for (int j = 0; j < m_base_extra_vertices.Length; j++)
				{
					m_curve_base_extra_vertices[j] = m_rotationOffsetQuat * (m_base_extra_vertices[j] - vector) + positionOffset;
				}
			}
			m_using_curved_data = true;
			Vector3 vector2 = (m_curve_base_vertices[m_base_indexes[1]] - m_curve_base_vertices[m_base_indexes[0]]) / 2f;
			Vector3 vector3 = (m_curve_base_vertices[m_base_indexes[0]] - m_curve_base_vertices[m_base_indexes[3]]) / 2f;
			m_active_anchor_offset_middle_center = (m_curve_base_vertices[0] + m_curve_base_vertices[1] + m_curve_base_vertices[2] + m_curve_base_vertices[3]) / 4f;
			m_active_anchor_offset_upper_left = m_active_anchor_offset_middle_center - vector2 + vector3;
			m_active_anchor_offset_upper_center = m_active_anchor_offset_middle_center + vector3;
			m_active_anchor_offset_upper_right = m_active_anchor_offset_middle_center + vector2 + vector3;
			m_active_anchor_offset_middle_left = m_active_anchor_offset_middle_center - vector2;
			m_active_anchor_offset_middle_right = m_active_anchor_offset_middle_center + vector2;
			m_active_anchor_offset_lower_left = m_active_anchor_offset_middle_center - vector2 - vector3;
			m_active_anchor_offset_lower_center = m_active_anchor_offset_middle_center - vector3;
			m_active_anchor_offset_lower_right = m_active_anchor_offset_middle_center + vector2 - vector3;
		}

		public void ClearOffsetData()
		{
			m_using_curved_data = false;
			m_curve_base_vertices = null;
			m_curve_base_extra_vertices = null;
			m_rotationOffsetQuat = Quaternion.identity;
			m_rotationOffsetQuatInverse = Quaternion.identity;
			if (!StubInstance && m_visible_character)
			{
				PreCalculateMeshInformation(m_flippedVerts);
			}
		}

		private void PreCalculateMeshInformation(bool flippedVerts = false)
		{
			m_base_indexes = new int[4];
			List<float> list = new List<float>();
			List<float> list2 = new List<float>();
			List<int> list3 = new List<int>();
			List<int> list4 = new List<int>();
			for (int i = 0; i < 4; i++)
			{
				if (i == 0)
				{
					list.Add(m_base_vertices[0].x);
					list3.Add(0);
					list2.Add(m_base_vertices[0].y);
					list4.Add(0);
					continue;
				}
				if (m_base_vertices[i].x <= list[0])
				{
					list.Insert(0, m_base_vertices[i].x);
					list3.Insert(0, i);
				}
				else
				{
					list.Add(m_base_vertices[i].x);
					list3.Add(i);
				}
				if (m_base_vertices[i].y <= list2[0])
				{
					list2.Insert(0, m_base_vertices[i].y);
					list4.Insert(0, i);
				}
				else
				{
					list2.Add(m_base_vertices[i].y);
					list4.Add(i);
				}
			}
			m_base_indexes[0] = ((list3[0] != list4[2] && list3[0] != list4[3]) ? list3[1] : list3[0]);
			m_base_indexes[1] = ((list3[2] != list4[2] && list3[2] != list4[3]) ? list3[3] : list3[2]);
			m_base_indexes[2] = ((list3[2] != list4[0] && list3[2] != list4[1]) ? list3[3] : list3[2]);
			m_base_indexes[3] = ((list3[0] != list4[0] && list3[0] != list4[1]) ? list3[1] : list3[0]);
			float num = 0f;
			float num2 = 0f;
			num = (m_base_vertices[m_base_indexes[1]].x - m_base_vertices[m_base_indexes[0]].x) / 2f;
			num2 = (m_base_vertices[m_base_indexes[0]].y - m_base_vertices[m_base_indexes[3]].y) / 2f;
			m_active_anchor_offset_middle_center = (m_anchor_offset_middle_center = (m_base_vertices[0] + m_base_vertices[1] + m_base_vertices[2] + m_base_vertices[3]) / 4f);
			m_active_anchor_offset_upper_left = m_anchor_offset_middle_center + new Vector3(0f - num, num2, 0f);
			m_active_anchor_offset_upper_center = m_anchor_offset_middle_center + new Vector3(0f, num2, 0f);
			m_active_anchor_offset_upper_right = m_anchor_offset_middle_center + new Vector3(num, num2, 0f);
			m_active_anchor_offset_middle_left = m_anchor_offset_middle_center + new Vector3(0f - num, 0f, 0f);
			m_active_anchor_offset_middle_right = m_anchor_offset_middle_center + new Vector3(num, 0f, 0f);
			m_active_anchor_offset_lower_left = m_anchor_offset_middle_center + new Vector3(0f - num, 0f - num2, 0f);
			m_active_anchor_offset_lower_center = m_anchor_offset_middle_center + new Vector3(0f, 0f - num2, 0f);
			m_active_anchor_offset_lower_right = m_anchor_offset_middle_center + new Vector3(num, 0f - num2, 0f);
			m_width = num * 2f;
			m_height = num2 * 2f;
			m_base_colours = new VertexColour(m_base_colours[m_base_indexes[0]], m_base_colours[m_base_indexes[1]], m_base_colours[m_base_indexes[2]], m_base_colours[m_base_indexes[3]]);
			if (m_base_extra_colours == null)
			{
				return;
			}
			Color[] array = new Color[m_base_extra_colours.Length];
			for (int j = 0; j < m_base_extra_colours.Length / 4; j++)
			{
				for (int k = 0; k < 4; k++)
				{
					array[j * 4 + k] = m_base_extra_colours[j * 4 + m_base_indexes[k]];
				}
			}
			m_base_extra_colours = array;
		}

		private Vector3 GetAnchorOffset(TextfxTextAnchor letter_anchor)
		{
			switch (letter_anchor)
			{
			case TextfxTextAnchor.UpperLeft:
				return m_active_anchor_offset_upper_left;
			case TextfxTextAnchor.UpperCenter:
				return m_active_anchor_offset_upper_center;
			case TextfxTextAnchor.UpperRight:
				return m_active_anchor_offset_upper_right;
			case TextfxTextAnchor.MiddleLeft:
				return m_active_anchor_offset_middle_left;
			case TextfxTextAnchor.MiddleCenter:
				return m_active_anchor_offset_middle_center;
			case TextfxTextAnchor.MiddleRight:
				return m_active_anchor_offset_middle_right;
			case TextfxTextAnchor.LowerLeft:
				return m_active_anchor_offset_lower_left;
			case TextfxTextAnchor.LowerCenter:
				return m_active_anchor_offset_lower_center;
			case TextfxTextAnchor.LowerRight:
				return m_active_anchor_offset_lower_right;
			default:
				return m_active_anchor_offset_middle_center;
			}
		}

		public void Reset(LetterAnimation animation, int starting_action_index = 0)
		{
			m_anim_state_vars.Reset(starting_action_index);
			m_current_letter_action = null;
			m_continueType = ContinueType.None;
			m_fastTrackLoops = false;
			m_current_state = LETTER_ANIMATION_STATE.PLAYING;
			if (animation.NumLoops > 0)
			{
				UpdateLoopList(animation);
			}
		}

		public void SetMeshState(TextFxAnimationManager animation_manager_ref, int action_idx, float action_progress, LetterAnimation animation, AnimatePerOptions animate_per, ref Vector3[] mesh_verts, ref Color[] mesh_colours)
		{
			if (m_stub_instance)
			{
				return;
			}
			m_animation_manager_ref = animation_manager_ref;
			if (action_idx >= 0 && action_idx < animation.NumActions && animation.GetAction(action_idx).m_action_type == ACTION_TYPE.ANIM_SEQUENCE)
			{
				CacheLetterActionStartEndStates(animation.GetAction(action_idx), animation, animate_per);
				SetupMesh(animation.GetAction(action_idx), Mathf.Clamp(action_progress, 0f, 1f), Mathf.Clamp(action_progress, 0f, 1f), ref mesh_verts, ref mesh_colours);
				return;
			}
			int index = 0;
			if (m_base_extra_vertices != null)
			{
				BaseExtraVertices.CopyTo(mesh_verts, 0);
				index = m_base_extra_vertices.Length;
			}
			BaseVertices.CopyTo(mesh_verts, index);
			if (m_base_extra_colours != null)
			{
				m_base_extra_colours.CopyTo(mesh_colours, 0);
			}
			new Color[4]
			{
				m_base_colours[m_base_indexes[0]],
				m_base_colours[m_base_indexes[1]],
				m_base_colours[m_base_indexes[2]],
				m_base_colours[m_base_indexes[3]]
			}.CopyTo(mesh_colours, index);
		}

		private void SetNextActionIndex(LetterAnimation animation)
		{
			m_anim_state_vars.m_action_index_progress++;
			int num = 0;
			while (num < m_anim_state_vars.ActiveLoopCycles.Count)
			{
				ActionLoopCycle actionLoopCycle = m_anim_state_vars.ActiveLoopCycles[num];
				if ((actionLoopCycle.m_loop_type != 0 || m_anim_state_vars.m_action_index != actionLoopCycle.m_end_action_idx) && (actionLoopCycle.m_loop_type != LOOP_TYPE.LOOP_REVERSE || ((!m_anim_state_vars.m_reverse || m_anim_state_vars.m_action_index != actionLoopCycle.m_start_action_idx) && (m_anim_state_vars.m_reverse || m_anim_state_vars.m_action_index != actionLoopCycle.m_end_action_idx))))
				{
					break;
				}
				bool flag = actionLoopCycle.m_loop_type == LOOP_TYPE.LOOP || m_anim_state_vars.m_reverse;
				if (flag)
				{
					actionLoopCycle.m_number_of_loops--;
				}
				if (actionLoopCycle.m_loop_type == LOOP_TYPE.LOOP_REVERSE)
				{
					m_anim_state_vars.m_reverse = !m_anim_state_vars.m_reverse;
				}
				actionLoopCycle.FirstPass = false;
				if (flag && actionLoopCycle.m_number_of_loops == 0)
				{
					m_anim_state_vars.ActiveLoopCycles.RemoveAt(num);
					num--;
					if (actionLoopCycle.m_loop_type == LOOP_TYPE.LOOP_REVERSE)
					{
						if (!actionLoopCycle.m_finish_at_end)
						{
							m_anim_state_vars.m_action_index = actionLoopCycle.m_end_action_idx;
						}
						else
						{
							m_anim_state_vars.m_action_index = actionLoopCycle.m_start_action_idx - 1;
						}
					}
					num++;
					continue;
				}
				if (actionLoopCycle.m_number_of_loops < 0)
				{
					actionLoopCycle.m_number_of_loops = -1;
				}
				if (actionLoopCycle.m_loop_type == LOOP_TYPE.LOOP)
				{
					m_anim_state_vars.m_action_index = actionLoopCycle.m_start_action_idx;
				}
				return;
			}
			m_anim_state_vars.m_action_index += ((!m_anim_state_vars.m_reverse) ? 1 : (-1));
			if (m_anim_state_vars.m_action_index >= animation.NumActions)
			{
				m_anim_state_vars.m_active = false;
				m_anim_state_vars.m_action_index = animation.NumActions - 1;
			}
		}

		private void UpdateLoopList(LetterAnimation animation)
		{
			for (int i = 0; i < animation.NumLoops; i++)
			{
				ActionLoopCycle loop = animation.GetLoop(i);
				if (loop.m_start_action_idx != m_anim_state_vars.m_action_index)
				{
					continue;
				}
				int spanWidth = loop.SpanWidth;
				int num = 0;
				foreach (ActionLoopCycle activeLoopCycle in m_anim_state_vars.ActiveLoopCycles)
				{
					if (loop.m_start_action_idx == activeLoopCycle.m_start_action_idx && loop.m_end_action_idx == activeLoopCycle.m_end_action_idx)
					{
						num = -1;
						break;
					}
					if (spanWidth < activeLoopCycle.SpanWidth)
					{
						break;
					}
					num++;
				}
				if (num >= 0)
				{
					loop = loop.Clone(i);
					if (m_fastTrackLoops)
					{
						loop.m_number_of_loops = 1;
					}
					m_anim_state_vars.ActiveLoopCycles.Insert(num, loop);
				}
			}
		}

		public void ContinueAction(float animation_timer, LetterAnimation animation, AnimatePerOptions animate_per)
		{
			if (!m_anim_state_vars.m_waiting_to_sync)
			{
				return;
			}
			m_anim_state_vars.m_break_delay = 0f;
			m_anim_state_vars.m_waiting_to_sync = false;
			m_anim_state_vars.m_timer_offset = animation_timer;
			int action_index = m_anim_state_vars.m_action_index;
			SetNextActionIndex(animation);
			if (m_anim_state_vars.m_active)
			{
				if (!m_anim_state_vars.m_reverse && m_anim_state_vars.m_action_index_progress > m_anim_state_vars.m_action_index)
				{
					animation.GetAction(m_anim_state_vars.m_action_index).SoftReset(animation.GetAction(action_index), m_progression_variables, animate_per);
				}
				if (action_index != m_anim_state_vars.m_action_index)
				{
					UpdateLoopList(animation);
				}
			}
		}

		public void ContinueFromCurrentToAction(LetterAnimation animation, int action_state_to_continue_to, bool use_start_state, int action_index_to_continue_with, AnimatePerOptions animate_per, float anim_timer, float continueDuration, int action_index_progress, int deepestLoopDepth, ContinueType continueType, bool trimInterimLoops, int[] lowestActiveLoopIterations)
		{
			m_continueLetterCallback = delegate(float timer)
			{
				LetterAction action = animation.GetAction(action_state_to_continue_to);
				m_anim_state_vars.m_timer_offset = timer;
				m_current_letter_action = null;
				m_anim_state_vars.m_waiting_to_sync = false;
				m_anim_state_vars.m_reverse = false;
				m_action_delay = 0f;
				m_action_duration = ((continueType != ContinueType.Instant) ? 0f : continueDuration);
				m_anim_state_vars.m_action_index = action_index_to_continue_with;
				m_anim_state_vars.m_prev_action_index = m_anim_state_vars.m_action_index;
				m_anim_state_vars.m_action_index_progress = action_index_progress;
				CacheCurrentStateToAction(action, animation, use_start_state, animate_per);
				if (deepestLoopDepth == 0)
				{
					ActiveLoopCycles.Clear();
				}
				else
				{
					ActiveLoopCycles.RemoveRange(0, ActiveLoopCycles.Count - (deepestLoopDepth - 1));
				}
			};
			m_continueType = continueType;
			if (m_continueType == ContinueType.EndOfLoop && deepestLoopDepth > 0 && ActiveLoopCycles.Count >= deepestLoopDepth)
			{
				ActionLoopCycle actionLoopCycle = ActiveLoopCycles[ActiveLoopCycles.Count - deepestLoopDepth];
				m_continuedLoopStartIndex = actionLoopCycle.m_start_action_idx;
				m_continueActionIndexTrigger = actionLoopCycle.m_end_action_idx;
				if (deepestLoopDepth == ActiveLoopCycles.Count)
				{
					ActiveLoopCycles[0].m_number_of_loops = 1 + (ActiveLoopCycles[0].m_number_of_loops - lowestActiveLoopIterations[ActiveLoopCycles[0].m_active_loop_index]);
				}
				if (!trimInterimLoops)
				{
					return;
				}
				m_fastTrackLoops = true;
				if (deepestLoopDepth < ActiveLoopCycles.Count)
				{
					for (int i = 0; i < ActiveLoopCycles.Count - deepestLoopDepth; i++)
					{
						ActionLoopCycle actionLoopCycle2 = ActiveLoopCycles[i];
						actionLoopCycle2.m_number_of_loops = 1 + (actionLoopCycle2.m_number_of_loops - lowestActiveLoopIterations[actionLoopCycle2.m_active_loop_index]);
					}
				}
			}
			else if (continueType == ContinueType.Instant)
			{
				m_continueLetterCallback(anim_timer);
				m_continueLetterCallback = null;
				m_current_state = LETTER_ANIMATION_STATE.CONTINUING;
			}
		}

		private void SetCurrentLetterAction(LetterAnimation animation, int action_index, AnimatePerOptions animate_per)
		{
			LetterAction current_letter_action = m_current_letter_action;
			m_current_letter_action = animation.GetAction(action_index);
			if (m_current_letter_action.m_action_type == ACTION_TYPE.ANIM_SEQUENCE)
			{
				m_action_delay = Mathf.Max(m_current_letter_action.m_delay_progression.GetValue(m_progression_variables, m_last_animate_per, m_current_letter_action.m_delay_with_white_space_influence), 0f);
			}
			m_action_duration = Mathf.Max(m_current_letter_action.m_duration_progression.GetValue(m_progression_variables, m_last_animate_per), 0f);
			if (m_anim_state_vars.ActiveLoopCycles != null && m_anim_state_vars.ActiveLoopCycles.Count > 0 && m_anim_state_vars.ActiveLoopCycles[0].m_delay_first_only && !m_anim_state_vars.ActiveLoopCycles[0].FirstPass && m_current_letter_action.m_delay_progression.Progression != 0)
			{
				m_ignore_action_delay = true;
			}
			else
			{
				m_ignore_action_delay = false;
			}
			if (!m_anim_state_vars.m_reverse)
			{
				m_current_letter_action.SoftReset(current_letter_action, m_progression_variables, animate_per, m_anim_state_vars.m_action_index == 0);
			}
			else if (m_anim_state_vars.m_reverse)
			{
				m_current_letter_action.SoftResetStarts(current_letter_action, m_progression_variables, animate_per);
			}
			CacheLetterActionStartEndStates(m_current_letter_action, animation, animate_per);
		}

		public void SetPlayingState()
		{
			m_current_state = LETTER_ANIMATION_STATE.PLAYING;
		}

		private void CallContinueCallback(float timer)
		{
			m_continueLetterCallback(timer);
			m_continueLetterCallback = null;
			m_continueType = ContinueType.None;
			m_fastTrackLoops = false;
			m_current_state = LETTER_ANIMATION_STATE.CONTINUING;
		}

		public bool AnimateMesh(TextFxAnimationManager animation_manager_ref, bool force_render, float timer, int lowest_action_progress, LetterAnimation animation, AnimatePerOptions animate_per, float delta_time, ref Vector3[] mesh_verts, ref Color[] mesh_colours)
		{
			if (m_animation_manager_ref == null)
			{
				m_animation_manager_ref = animation_manager_ref;
			}
			if (m_current_state == LETTER_ANIMATION_STATE.CONTINUING_FINISHED)
			{
				m_anim_state_vars.m_timer_offset = timer;
				return false;
			}
			m_last_animate_per = animate_per;
			if (animation.NumActions > 0 && m_anim_state_vars.m_action_index < animation.NumActions && (m_anim_state_vars.m_active || force_render))
			{
				if (m_anim_state_vars.m_action_index != m_anim_state_vars.m_prev_action_index || (m_current_state != LETTER_ANIMATION_STATE.CONTINUING && m_current_letter_action == null))
				{
					SetCurrentLetterAction(animation, m_anim_state_vars.m_action_index, animate_per);
					if (m_anim_state_vars.m_action_index != m_anim_state_vars.m_prev_action_index)
					{
						m_anim_state_vars.m_started_action = false;
					}
				}
				m_anim_state_vars.m_prev_action_index = m_anim_state_vars.m_action_index;
				if (force_render)
				{
					SetupMesh(m_current_letter_action, m_anim_state_vars.m_action_progress, m_anim_state_vars.m_linear_progress, ref mesh_verts, ref mesh_colours);
				}
				if (m_anim_state_vars.m_waiting_to_sync)
				{
					if (m_continueType == ContinueType.EndOfLoop && m_anim_state_vars.m_action_index == m_continuedLoopStartIndex && m_current_letter_action.m_action_type != ACTION_TYPE.BREAK)
					{
						CallContinueCallback(timer);
						return false;
					}
					if (m_current_letter_action.m_action_type == ACTION_TYPE.BREAK)
					{
						if (!force_render && m_anim_state_vars.m_break_delay > 0f)
						{
							m_anim_state_vars.m_break_delay -= delta_time;
							if (m_anim_state_vars.m_break_delay <= 0f || m_fastTrackLoops)
							{
								if (m_continueType == ContinueType.EndOfLoop && m_anim_state_vars.m_action_index == m_continueActionIndexTrigger)
								{
									CallContinueCallback(timer);
									return false;
								}
								ContinueAction(timer, animation, animate_per);
								m_current_state = LETTER_ANIMATION_STATE.PLAYING;
								return false;
							}
						}
						m_current_state = ((m_anim_state_vars.m_break_delay != 0f) ? LETTER_ANIMATION_STATE.WAITING : LETTER_ANIMATION_STATE.WAITING_INFINITE);
						return false;
					}
					if (lowest_action_progress < m_anim_state_vars.m_action_index_progress)
					{
						m_current_state = LETTER_ANIMATION_STATE.PLAYING;
						return false;
					}
					if (!force_render)
					{
						m_anim_state_vars.m_waiting_to_sync = false;
						m_anim_state_vars.m_timer_offset = timer;
					}
				}
				else if (!force_render && m_current_letter_action != null && (m_current_letter_action.m_action_type == ACTION_TYPE.BREAK || (!m_anim_state_vars.m_reverse && !m_ignore_action_delay && m_current_letter_action.m_force_same_start_time && lowest_action_progress < m_anim_state_vars.m_action_index_progress)))
				{
					m_anim_state_vars.m_waiting_to_sync = true;
					m_anim_state_vars.m_break_delay = Mathf.Max(m_current_letter_action.m_duration_progression.GetValue(m_progression_variables, animate_per), 0f);
					m_current_state = LETTER_ANIMATION_STATE.PLAYING;
					return false;
				}
				if (force_render)
				{
					m_current_state = ((!m_anim_state_vars.m_active) ? LETTER_ANIMATION_STATE.STOPPED : LETTER_ANIMATION_STATE.PLAYING);
					return false;
				}
				current_action_delay = ((!m_ignore_action_delay && !m_anim_state_vars.m_reverse) ? m_action_delay : 0f);
				altered_delay = false;
				if (m_animation_manager_ref.WhatJustChanged == ANIMATION_DATA_TYPE.ALL || m_animation_manager_ref.WhatJustChanged == ANIMATION_DATA_TYPE.DELAY)
				{
					old_action_delay = m_action_delay;
					m_action_delay = Mathf.Max(m_current_letter_action.m_delay_progression.GetValue(m_progression_variables, m_last_animate_per, m_current_letter_action.m_delay_with_white_space_influence), 0f);
					if (old_action_delay != m_action_delay)
					{
						m_anim_state_vars.m_timer_offset += m_action_delay - old_action_delay;
						altered_delay = true;
					}
				}
				if (!altered_delay && (m_animation_manager_ref.WhatJustChanged == ANIMATION_DATA_TYPE.ALL || m_animation_manager_ref.WhatJustChanged == ANIMATION_DATA_TYPE.DURATION))
				{
					m_action_duration = Mathf.Max(m_current_letter_action.m_duration_progression.GetValue(m_progression_variables, m_last_animate_per), 0f);
					m_anim_state_vars.m_timer_offset = -1f * (((!m_anim_state_vars.m_reverse) ? m_anim_state_vars.m_linear_progress : (1f - m_anim_state_vars.m_linear_progress)) * m_action_duration - timer + ((!m_anim_state_vars.m_reverse) ? current_action_delay : 0f));
					m_anim_state_vars.m_timer_offset -= delta_time;
				}
				m_anim_state_vars.m_action_progress = 0f;
				m_anim_state_vars.m_linear_progress = 0f;
				m_action_timer = timer - m_anim_state_vars.m_timer_offset;
				if (m_anim_state_vars.m_reverse || m_action_timer > current_action_delay)
				{
					m_anim_state_vars.m_linear_progress = (m_action_timer - ((!m_anim_state_vars.m_reverse) ? current_action_delay : 0f)) / m_action_duration;
					if (m_anim_state_vars.m_reverse)
					{
						if (m_action_timer >= m_action_duration)
						{
							m_anim_state_vars.m_linear_progress = 0f;
						}
						else
						{
							m_anim_state_vars.m_linear_progress = 1f - m_anim_state_vars.m_linear_progress;
						}
					}
					if (m_visible_character && !m_anim_state_vars.m_started_action && m_current_letter_action != null)
					{
						if (m_current_letter_action.AudioEffectSetups != null && m_current_letter_action.AudioEffectSetups.Count > 0)
						{
							TriggerEffects(m_current_letter_action.AudioEffectSetups, animate_per, PLAY_ITEM_EVENTS.ON_START, PlayAudioEffect);
						}
						if (m_current_letter_action.ParticleEffectSetups != null && m_current_letter_action.ParticleEffectSetups.Count > 0)
						{
							TriggerEffects(m_current_letter_action.ParticleEffectSetups, animate_per, PLAY_ITEM_EVENTS.ON_START, PlayParticleEffect);
						}
						m_anim_state_vars.m_started_action = true;
					}
					m_anim_state_vars.m_action_progress = ((m_current_letter_action == null) ? EasingManager.GetEaseProgress(EasingEquation.CubicEaseOut, m_anim_state_vars.m_linear_progress) : EasingManager.GetEaseProgress(m_current_letter_action.m_ease_type, m_anim_state_vars.m_linear_progress));
					if (m_current_state != LETTER_ANIMATION_STATE.CONTINUING)
					{
						m_current_state = LETTER_ANIMATION_STATE.PLAYING;
					}
					if ((!m_anim_state_vars.m_reverse && m_anim_state_vars.m_linear_progress >= 1f) || (m_anim_state_vars.m_reverse && m_action_timer >= m_action_duration + current_action_delay))
					{
						m_anim_state_vars.m_action_progress = ((!m_anim_state_vars.m_reverse) ? 1 : 0);
						m_anim_state_vars.m_linear_progress = ((!m_anim_state_vars.m_reverse) ? 1 : 0);
						if (animation.CurrentAnimationState != LETTER_ANIMATION_STATE.CONTINUING && m_visible_character && !m_anim_state_vars.m_reverse && m_anim_state_vars.m_action_index != -1 && m_current_letter_action != null)
						{
							if (m_current_letter_action.AudioEffectSetups != null && m_current_letter_action.AudioEffectSetups.Count > 0)
							{
								TriggerEffects(m_current_letter_action.AudioEffectSetups, animate_per, PLAY_ITEM_EVENTS.ON_FINISH, PlayAudioEffect);
							}
							if (m_current_letter_action.ParticleEffectSetups != null && m_current_letter_action.ParticleEffectSetups.Count > 0)
							{
								TriggerEffects(m_current_letter_action.ParticleEffectSetups, animate_per, PLAY_ITEM_EVENTS.ON_FINISH, PlayParticleEffect);
							}
						}
						if (m_current_state == LETTER_ANIMATION_STATE.CONTINUING)
						{
							m_current_state = LETTER_ANIMATION_STATE.CONTINUING_FINISHED;
						}
						if (m_continueType == ContinueType.EndOfLoop && m_anim_state_vars.m_action_index == m_continueActionIndexTrigger && (ActiveLoopCycles.Count == 0 || ((ActiveLoopCycles[0].m_loop_type == LOOP_TYPE.LOOP || m_anim_state_vars.m_reverse) && ActiveLoopCycles[0].m_number_of_loops <= 1)))
						{
							SetupMesh(m_current_letter_action, m_anim_state_vars.m_action_progress, m_anim_state_vars.m_linear_progress, ref mesh_verts, ref mesh_colours);
							CallContinueCallback(timer);
							return true;
						}
						prev_action_idx = m_anim_state_vars.m_action_index;
						prev_delay = current_action_delay;
						SetNextActionIndex(animation);
						if (m_anim_state_vars.m_active)
						{
							if (!m_anim_state_vars.m_reverse)
							{
								m_anim_state_vars.m_started_action = false;
							}
							m_anim_state_vars.m_timer_offset += prev_delay + m_action_duration;
							if (prev_action_idx != m_anim_state_vars.m_action_index)
							{
								UpdateLoopList(animation);
							}
							else
							{
								SetCurrentLetterAction(animation, m_anim_state_vars.m_action_index, animate_per);
							}
						}
						else
						{
							m_current_state = LETTER_ANIMATION_STATE.STOPPED;
						}
					}
				}
				SetupMesh(m_current_letter_action, m_anim_state_vars.m_action_progress, m_anim_state_vars.m_linear_progress, ref mesh_verts, ref mesh_colours);
			}
			else
			{
				if (m_current_animated_vertices != null)
				{
					m_current_animated_vertices.CopyTo(mesh_verts, 0);
				}
				else
				{
					BaseVertices.CopyTo(mesh_verts, 0);
				}
				if (m_current_animated_colours != null)
				{
					m_current_animated_colours.CopyTo(mesh_colours, 0);
				}
				else
				{
					mesh_colours = new Color[4]
					{
						Color.white,
						Color.white,
						Color.white,
						Color.white
					};
				}
				m_anim_state_vars.m_active = false;
				m_current_state = LETTER_ANIMATION_STATE.STOPPED;
			}
			return true;
		}

		private void PlayAudioEffect(AudioEffectSetup effect_setup, AnimatePerOptions animate_per)
		{
			m_animation_manager_ref.PlayAudioClip(effect_setup, m_progression_variables, animate_per);
		}

		private void PlayParticleEffect(ParticleEffectSetup effect_setup, AnimatePerOptions animate_per)
		{
			m_animation_manager_ref.PlayParticleEffect(this, effect_setup, m_progression_variables, animate_per);
		}

		private void TriggerEffects<T>(List<T> effectSetups, AnimatePerOptions animate_per, PLAY_ITEM_EVENTS play_when, Action<T, AnimatePerOptions> play_effect_callback) where T : EffectItemSetup
		{
			if (effectSetups == null)
			{
				return;
			}
			foreach (T effectSetup in effectSetups)
			{
				if (effectSetup.m_play_when == play_when && (effectSetup.m_effect_assignment == PLAY_ITEM_ASSIGNMENT.PER_LETTER || effectSetup.m_effect_assignment_custom_letters.Contains(m_progression_variables.LetterValue)) && (!effectSetup.m_loop_play_once || m_anim_state_vars.ActiveLoopCycles == null || m_anim_state_vars.ActiveLoopCycles.Count == 0 || m_anim_state_vars.ActiveLoopCycles[0].FirstPass))
				{
					play_effect_callback(effectSetup, animate_per);
				}
			}
		}

		private void CacheCurrentStateToAction(LetterAction end_action, LetterAnimation letterAnimation, bool use_start_state, AnimatePerOptions animate_per)
		{
			if (!m_stub_instance)
			{
				List<LetterAction> letterActions = letterAnimation.LetterActions;
				m_local_scale_from = m_letter_scale;
				m_local_scale_to = ((!use_start_state) ? end_action.m_end_scale.GetValue(letterActions, m_progression_variables, animate_per, true) : end_action.m_start_scale.GetValue(letterActions, m_progression_variables, animate_per, true));
				m_global_scale_from = m_letter_global_scale;
				m_global_scale_to = ((!use_start_state) ? end_action.m_global_end_scale.GetValue(letterActions, m_progression_variables, animate_per, true) : end_action.m_global_start_scale.GetValue(letterActions, m_progression_variables, animate_per, true));
				m_local_rotation_from = m_letter_rotation.eulerAngles;
				m_local_rotation_to = ((!use_start_state) ? end_action.m_end_euler_rotation.GetValue(letterActions, m_progression_variables, animate_per, true) : end_action.m_start_euler_rotation.GetValue(letterActions, m_progression_variables, animate_per, true));
				m_global_rotation_from = m_letter_global_rotation.eulerAngles;
				m_global_rotation_to = ((!use_start_state) ? end_action.m_global_end_euler_rotation.GetValue(letterActions, m_progression_variables, animate_per, true) : end_action.m_global_start_euler_rotation.GetValue(letterActions, m_progression_variables, animate_per, true));
				m_position_from = m_letter_position;
				m_position_to = ((!use_start_state) ? end_action.m_end_pos.GetValue(letterActions, m_progression_variables, animate_per, true) : end_action.m_start_pos.GetValue(letterActions, m_progression_variables, animate_per, true));
				if (m_colour_to == null)
				{
					m_colour_to = new VertexColour();
				}
				m_colour_from = m_letter_colour;
				if (use_start_state)
				{
					end_action.m_start_colour.GetValue(ref m_colour_to, letterActions, m_progression_variables, animate_per, letterAnimation.m_defaultTextColourProgression);
				}
				else
				{
					end_action.m_end_colour.GetValue(ref m_colour_to, letterActions, m_progression_variables, animate_per, letterAnimation.m_defaultTextColourProgression);
				}
				m_anchor_offset_from = m_anchor_offset;
				m_anchor_offset_to = GetAnchorOffset((TextfxTextAnchor)((!use_start_state) ? end_action.m_letter_anchor_end : end_action.m_letter_anchor_start));
			}
		}

		private void CacheLetterActionStartEndStates(LetterAction current_action, LetterAnimation letterAnimation, AnimatePerOptions animate_per)
		{
			if (!m_stub_instance && (current_action.m_action_type != ACTION_TYPE.BREAK || m_colour_from == null))
			{
				m_local_scale_from = current_action.m_start_scale.GetValue(letterAnimation.LetterActions, m_progression_variables, animate_per, true);
				m_local_scale_to = current_action.m_end_scale.GetValue(letterAnimation.LetterActions, m_progression_variables, animate_per, true);
				m_global_scale_from = current_action.m_global_start_scale.GetValue(letterAnimation.LetterActions, m_progression_variables, animate_per, true);
				m_global_scale_to = current_action.m_global_end_scale.GetValue(letterAnimation.LetterActions, m_progression_variables, animate_per, true);
				m_local_rotation_from = current_action.m_start_euler_rotation.GetValue(letterAnimation.LetterActions, m_progression_variables, animate_per, true);
				m_local_rotation_to = current_action.m_end_euler_rotation.GetValue(letterAnimation.LetterActions, m_progression_variables, animate_per, true);
				m_global_rotation_from = current_action.m_global_start_euler_rotation.GetValue(letterAnimation.LetterActions, m_progression_variables, animate_per, true);
				m_global_rotation_to = current_action.m_global_end_euler_rotation.GetValue(letterAnimation.LetterActions, m_progression_variables, animate_per, true);
				m_position_from = current_action.m_start_pos.GetValue(letterAnimation.LetterActions, m_progression_variables, animate_per, true);
				m_position_to = current_action.m_end_pos.GetValue(letterAnimation.LetterActions, m_progression_variables, animate_per, true);
				if (m_colour_from == null)
				{
					m_colour_from = new VertexColour();
				}
				if (m_colour_to == null)
				{
					m_colour_to = new VertexColour();
				}
				current_action.m_start_colour.GetValue(ref m_colour_from, letterAnimation.LetterActions, m_progression_variables, animate_per, letterAnimation.m_defaultTextColourProgression);
				current_action.m_end_colour.GetValue(ref m_colour_to, letterAnimation.LetterActions, m_progression_variables, animate_per, letterAnimation.m_defaultTextColourProgression);
				m_anchor_offset_from = GetAnchorOffset((TextfxTextAnchor)current_action.m_letter_anchor_start);
				m_anchor_offset_to = ((!current_action.m_letter_anchor_2_way) ? m_anchor_offset_from : GetAnchorOffset((TextfxTextAnchor)current_action.m_letter_anchor_end));
			}
		}

		private void SetupMesh(LetterAction letter_action, float action_progress, float linear_progress, ref Vector3[] mesh_verts, ref Color[] mesh_colours)
		{
			if (m_stub_instance)
			{
				return;
			}
			bool flag = true;
			bool flag2 = true;
			if (letter_action != null && letter_action.m_scale_axis_ease_data.m_override_default)
			{
				m_letter_scale = new Vector3(FloatLerp(m_local_scale_from.x, m_local_scale_to.x, EasingManager.GetEaseProgress(letter_action.m_scale_axis_ease_data.m_x_ease, linear_progress)), FloatLerp(m_local_scale_from.y, m_local_scale_to.y, EasingManager.GetEaseProgress(letter_action.m_scale_axis_ease_data.m_y_ease, linear_progress)), FloatLerp(m_local_scale_from.z, m_local_scale_to.z, EasingManager.GetEaseProgress(letter_action.m_scale_axis_ease_data.m_z_ease, linear_progress)));
			}
			else
			{
				m_letter_scale = Vector3Lerp(m_local_scale_from, m_local_scale_to, action_progress);
			}
			if (m_letter_scale == Vector3.one)
			{
				flag = false;
			}
			if (letter_action != null && letter_action.m_global_scale_axis_ease_data.m_override_default)
			{
				m_letter_global_scale = new Vector3(FloatLerp(m_global_scale_from.x, m_global_scale_to.x, EasingManager.GetEaseProgress(letter_action.m_global_scale_axis_ease_data.m_x_ease, linear_progress)), FloatLerp(m_global_scale_from.y, m_global_scale_to.y, EasingManager.GetEaseProgress(letter_action.m_global_scale_axis_ease_data.m_y_ease, linear_progress)), FloatLerp(m_global_scale_from.z, m_global_scale_to.z, EasingManager.GetEaseProgress(letter_action.m_global_scale_axis_ease_data.m_z_ease, linear_progress)));
			}
			else
			{
				m_letter_global_scale = Vector3Lerp(m_global_scale_from, m_global_scale_to, action_progress);
			}
			if (letter_action != null && letter_action.m_rotation_axis_ease_data.m_override_default)
			{
				m_letter_rotation = Quaternion.Euler(FloatLerp(m_local_rotation_from.x, m_local_rotation_to.x, EasingManager.GetEaseProgress(letter_action.m_rotation_axis_ease_data.m_x_ease, linear_progress)), FloatLerp(m_local_rotation_from.y, m_local_rotation_to.y, EasingManager.GetEaseProgress(letter_action.m_rotation_axis_ease_data.m_y_ease, linear_progress)), FloatLerp(m_local_rotation_from.z, m_local_rotation_to.z, EasingManager.GetEaseProgress(letter_action.m_rotation_axis_ease_data.m_z_ease, linear_progress)));
			}
			else
			{
				m_letter_rotation = Quaternion.Euler(Vector3Lerp(m_local_rotation_from, m_local_rotation_to, action_progress));
			}
			if (m_letter_rotation == Quaternion.identity)
			{
				flag2 = false;
			}
			if (letter_action != null && letter_action.m_global_rotation_axis_ease_data.m_override_default)
			{
				m_letter_global_rotation = Quaternion.Euler(FloatLerp(m_global_rotation_from.x, m_global_rotation_to.x, EasingManager.GetEaseProgress(letter_action.m_global_rotation_axis_ease_data.m_x_ease, linear_progress)), FloatLerp(m_global_rotation_from.y, m_global_rotation_to.y, EasingManager.GetEaseProgress(letter_action.m_global_rotation_axis_ease_data.m_y_ease, linear_progress)), FloatLerp(m_global_rotation_from.z, m_global_rotation_to.z, EasingManager.GetEaseProgress(letter_action.m_global_rotation_axis_ease_data.m_z_ease, linear_progress)));
			}
			else
			{
				m_letter_global_rotation = Quaternion.Euler(Vector3Lerp(m_global_rotation_from, m_global_rotation_to, action_progress));
			}
			if (letter_action != null && letter_action.m_position_axis_ease_data.m_override_default)
			{
				m_letter_position = new Vector3(FloatLerp(m_position_from.x, m_position_to.x, EasingManager.GetEaseProgress(letter_action.m_position_axis_ease_data.m_x_ease, linear_progress)), FloatLerp(m_position_from.y, m_position_to.y, EasingManager.GetEaseProgress(letter_action.m_position_axis_ease_data.m_y_ease, linear_progress)), FloatLerp(m_position_from.z, m_position_to.z, EasingManager.GetEaseProgress(letter_action.m_position_axis_ease_data.m_z_ease, linear_progress)));
			}
			else
			{
				m_letter_position = Vector3Lerp(m_position_from, m_position_to, action_progress);
			}
			bool flag3 = m_animation_manager_ref.AnimationInterface != null && m_animation_manager_ref.AnimationInterface.RenderToCurve;
			if (flag3)
			{
				m_letter_position = m_rotationOffsetQuat * m_letter_position;
			}
			if (m_letter_colour == null)
			{
				m_letter_colour = new VertexColour();
			}
			m_anchor_offset = Vector3.Lerp(m_anchor_offset_from, m_anchor_offset_to, action_progress);
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < mesh_verts.Length; i++)
			{
				if (mesh_verts.Length - i > 4)
				{
					num = -1;
					num2 = i;
				}
				else
				{
					num = 4 - (mesh_verts.Length - i);
					num2 = -1;
				}
				mesh_verts[i] = ((num2 < 0) ? BaseVertices[num] : BaseExtraVertices[num2]);
				if (flag2 || flag)
				{
					mesh_verts[i] -= m_anchor_offset;
					if (flag3)
					{
						mesh_verts[i] = m_rotationOffsetQuatInverse * mesh_verts[i];
					}
					if (flag)
					{
						mesh_verts[i] = Vector3.Scale(mesh_verts[i], m_letter_scale);
					}
					if (flag2)
					{
						mesh_verts[i] = m_letter_rotation * mesh_verts[i];
					}
					if (flag3)
					{
						mesh_verts[i] = m_rotationOffsetQuat * mesh_verts[i];
					}
					mesh_verts[i] += m_anchor_offset;
				}
				mesh_verts[i] = Vector3.Scale(mesh_verts[i], m_letter_global_scale);
				mesh_verts[i] = m_letter_global_rotation * mesh_verts[i];
				mesh_verts[i] += m_letter_position * m_animation_manager_ref.MovementScale;
				if (num < 0)
				{
					continue;
				}
				if (m_flippedVerts)
				{
					num += 2;
					if (num > 3)
					{
						num -= 4;
					}
				}
				switch (m_base_indexes[num])
				{
				case 0:
					mesh_colours[i] = Color.Lerp(m_colour_from.top_left, m_colour_to.top_left, action_progress);
					m_letter_colour.top_left = mesh_colours[i];
					break;
				case 1:
					mesh_colours[i] = Color.Lerp(m_colour_from.top_right, m_colour_to.top_right, action_progress);
					m_letter_colour.top_right = mesh_colours[i];
					break;
				case 2:
					mesh_colours[i] = Color.Lerp(m_colour_from.bottom_right, m_colour_to.bottom_right, action_progress);
					m_letter_colour.bottom_right = mesh_colours[i];
					break;
				case 3:
					mesh_colours[i] = Color.Lerp(m_colour_from.bottom_left, m_colour_to.bottom_left, action_progress);
					m_letter_colour.bottom_left = mesh_colours[i];
					break;
				}
			}
			if (mesh_verts.Length > 4)
			{
				for (int j = 0; j < mesh_verts.Length - 4; j++)
				{
					Color color = m_base_extra_colours[j];
					switch (m_base_indexes[j % 4])
					{
					case 0:
						color.a *= m_letter_colour.top_left.a;
						break;
					case 1:
						color.a *= m_letter_colour.top_right.a;
						break;
					case 2:
						color.a *= m_letter_colour.bottom_right.a;
						break;
					case 3:
						color.a *= m_letter_colour.bottom_left.a;
						break;
					}
					mesh_colours[j] = color;
				}
			}
			if (m_current_animated_vertices == null || m_current_animated_vertices.Length != mesh_verts.Length)
			{
				m_current_animated_vertices = new Vector3[mesh_verts.Length];
			}
			mesh_verts.CopyTo(m_current_animated_vertices, 0);
			if (m_current_animated_colours == null || m_current_animated_colours.Length != mesh_verts.Length)
			{
				m_current_animated_colours = new Color[mesh_verts.Length];
			}
			mesh_colours.CopyTo(m_current_animated_colours, 0);
		}

		private static Vector3 Vector3Lerp(Vector3 from_vec, Vector3 to_vec, float progress)
		{
			if (progress <= 1f && progress >= 0f)
			{
				return Vector3.Lerp(from_vec, to_vec, progress);
			}
			return from_vec + Vector3.Scale(to_vec - from_vec, Vector3.one * progress);
		}

		private static float FloatLerp(float from_val, float to_val, float progress)
		{
			if (progress <= 1f && progress >= 0f)
			{
				return Mathf.Lerp(from_val, to_val, progress);
			}
			return from_val + (to_val - from_val) * progress;
		}
	}
}
