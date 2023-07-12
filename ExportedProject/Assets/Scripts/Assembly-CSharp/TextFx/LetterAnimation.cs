using System;
using System.Collections.Generic;
using Boomlagoon.TextFx.JSON;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public class LetterAnimation
	{
		private const char DELIMITER_CHAR = '|';

		[SerializeField]
		private List<LetterAction> m_letter_actions = new List<LetterAction>();

		[SerializeField]
		private List<ActionLoopCycle> m_loop_cycles = new List<ActionLoopCycle>();

		public LETTERS_TO_ANIMATE m_letters_to_animate_option;

		public List<int> m_letters_to_animate;

		public int m_letters_to_animate_custom_idx = 1;

		[SerializeField]
		private int m_num_white_space_chars_to_include;

		[SerializeField]
		public ActionColorProgression m_defaultTextColourProgression = new ActionColorProgression(new VertexColour(Color.white));

		private LETTER_ANIMATION_STATE m_animation_state;

		public int NumActions
		{
			get
			{
				return m_letter_actions.Count;
			}
		}

		public int NumLoops
		{
			get
			{
				return m_loop_cycles.Count;
			}
		}

		public List<LetterAction> LetterActions
		{
			get
			{
				return m_letter_actions;
			}
		}

		public List<ActionLoopCycle> ActionLoopCycles
		{
			get
			{
				return m_loop_cycles;
			}
		}

		public LETTER_ANIMATION_STATE CurrentAnimationState
		{
			get
			{
				return m_animation_state;
			}
			set
			{
				m_animation_state = value;
			}
		}

		public void AddAction(LetterAction letter_action)
		{
			AddAction(letter_action, -1);
		}

		public void AddAction(LetterAction letter_action, int index)
		{
			if (m_letter_actions == null)
			{
				m_letter_actions = new List<LetterAction>();
			}
			if (index < 0)
			{
				m_letter_actions.Add(letter_action);
			}
			else
			{
				m_letter_actions.Insert(index, letter_action);
			}
		}

		public LetterAction AddAction()
		{
			if (m_letter_actions == null)
			{
				m_letter_actions = new List<LetterAction>();
			}
			LetterAction letterAction = new LetterAction();
			m_letter_actions.Add(letterAction);
			return letterAction;
		}

		public void InsertAction(int index, LetterAction action)
		{
			if (m_letter_actions == null)
			{
				m_letter_actions = new List<LetterAction>();
			}
			if (index >= 0 && index <= m_letter_actions.Count)
			{
				m_letter_actions.Insert(index, action);
			}
			UpdateLoopCyclesAfterIndex(index, 1);
		}

		public void RemoveAction(int index, bool deleteAffectedLoops = true)
		{
			RemoveActions(index, 1, deleteAffectedLoops);
		}

		public void RemoveActions(int index, int count, bool deleteAffectedLoops = true)
		{
			if (m_letter_actions != null && index >= 0 && index + count <= m_letter_actions.Count)
			{
				m_letter_actions.RemoveRange(index, count);
			}
			for (int i = 0; i < m_loop_cycles.Count; i++)
			{
				ActionLoopCycle actionLoopCycle = m_loop_cycles[i];
				if (actionLoopCycle.m_start_action_idx >= index + count)
				{
					actionLoopCycle.m_start_action_idx -= count;
					actionLoopCycle.m_end_action_idx -= count;
				}
				else if (deleteAffectedLoops && actionLoopCycle.m_end_action_idx >= index)
				{
					m_loop_cycles.RemoveAt(i);
					i--;
				}
			}
		}

		public LetterAction GetAction(int index)
		{
			if (m_letter_actions != null && index >= 0 && index < m_letter_actions.Count)
			{
				return m_letter_actions[index];
			}
			return null;
		}

		public void AddLoop()
		{
			if (m_loop_cycles == null)
			{
				m_loop_cycles = new List<ActionLoopCycle>();
			}
			m_loop_cycles.Add(new ActionLoopCycle());
		}

		public void InsertLoop(int index, ActionLoopCycle loop)
		{
			InsertLoop(index, loop, false);
		}

		public void InsertLoop(int index, ActionLoopCycle loop, bool force_insert)
		{
			if (m_loop_cycles == null)
			{
				m_loop_cycles = new List<ActionLoopCycle>();
			}
			if (!force_insert)
			{
				foreach (ActionLoopCycle loop_cycle in m_loop_cycles)
				{
					if (loop_cycle.m_start_action_idx == loop.m_start_action_idx && loop_cycle.m_end_action_idx == loop.m_end_action_idx)
					{
						return;
					}
				}
			}
			m_loop_cycles.Insert(index, loop);
		}

		public void RemoveLoop(int index)
		{
			if (m_loop_cycles != null && index >= 0 && index < m_loop_cycles.Count)
			{
				m_loop_cycles.RemoveAt(index);
			}
		}

		public void RemoveLoops(int index, int count)
		{
			if (m_loop_cycles != null && index >= 0 && index + count < m_loop_cycles.Count)
			{
				m_loop_cycles.RemoveRange(index, count);
			}
		}

		public ActionLoopCycle GetLoop(int index)
		{
			if (m_loop_cycles != null && index >= 0 && index < m_loop_cycles.Count)
			{
				return m_loop_cycles[index];
			}
			return null;
		}

		public void AddLoop(int start_idx, int end_idx, bool change_type)
		{
			bool flag = true;
			int index = 0;
			if (end_idx >= start_idx && start_idx >= 0 && start_idx < m_letter_actions.Count && end_idx >= 0 && end_idx < m_letter_actions.Count)
			{
				int num = end_idx - start_idx;
				int num2 = 1;
				foreach (ActionLoopCycle loop_cycle in m_loop_cycles)
				{
					if ((start_idx < loop_cycle.m_start_action_idx && end_idx > loop_cycle.m_start_action_idx && end_idx < loop_cycle.m_end_action_idx) || (end_idx > loop_cycle.m_end_action_idx && start_idx > loop_cycle.m_start_action_idx && start_idx < loop_cycle.m_end_action_idx))
					{
						flag = false;
						Debug.LogWarning("Invalid Loop Added: Loops can not intersect other loops.");
						break;
					}
					if (start_idx == loop_cycle.m_start_action_idx && end_idx == loop_cycle.m_end_action_idx)
					{
						flag = false;
						if (change_type)
						{
							loop_cycle.m_loop_type = ((loop_cycle.m_loop_type == LOOP_TYPE.LOOP) ? LOOP_TYPE.LOOP_REVERSE : LOOP_TYPE.LOOP);
						}
						else
						{
							loop_cycle.m_number_of_loops++;
						}
						break;
					}
					if (num >= loop_cycle.SpanWidth)
					{
						index = num2;
					}
					num2++;
				}
			}
			else
			{
				flag = false;
				Debug.LogWarning("Invalid Loop Added: Check that start/end index are in bounds.");
			}
			if (flag)
			{
				m_loop_cycles.Insert(index, new ActionLoopCycle(start_idx, end_idx));
			}
		}

		private void CalculateLettersToAnimate(LetterSetup[] letters)
		{
			int num = letters.Length;
			if (m_letters_to_animate_option == LETTERS_TO_ANIMATE.ALL_LETTERS)
			{
				int num2 = 0;
				int num3 = 0;
				LetterSetup letterSetup = null;
				m_num_white_space_chars_to_include = 0;
				m_letters_to_animate = new List<int>();
				for (int i = 0; i < num; i++)
				{
					if (letters[i].VisibleCharacter && !letters[i].StubInstance)
					{
						m_letters_to_animate.Add(i);
						letters[i].m_progression_variables.SetLetterValue(num2, num3);
						num2++;
						num3++;
					}
					else
					{
						letters[i].m_progression_variables.SetLetterValue(-1);
						if (letterSetup != null && letterSetup.VisibleCharacter)
						{
							num3++;
							m_num_white_space_chars_to_include++;
						}
					}
					letterSetup = letters[i];
				}
				return;
			}
			if (m_letters_to_animate_option == LETTERS_TO_ANIMATE.FIRST_LETTER || m_letters_to_animate_option == LETTERS_TO_ANIMATE.LAST_LETTER)
			{
				m_letters_to_animate = new List<int>();
				m_letters_to_animate.Add((m_letters_to_animate_option != LETTERS_TO_ANIMATE.FIRST_LETTER) ? (letters.Length - 1) : 0);
				letters[(m_letters_to_animate_option != LETTERS_TO_ANIMATE.FIRST_LETTER) ? (letters.Length - 1) : 0].m_progression_variables.SetLetterValue(0);
				return;
			}
			if (m_letters_to_animate_option != LETTERS_TO_ANIMATE.CUSTOM)
			{
				m_letters_to_animate = new List<int>();
				int num4 = ((m_letters_to_animate_option != LETTERS_TO_ANIMATE.LAST_LETTER_LINES) ? (-1) : 0);
				int num5 = ((m_letters_to_animate_option != LETTERS_TO_ANIMATE.LAST_LETTER_WORDS) ? (-1) : 0);
				int num6 = 0;
				if (m_letters_to_animate_option == LETTERS_TO_ANIMATE.LAST_WORD)
				{
					num6 = letters[letters.Length - 1].m_progression_variables.WordValue;
				}
				else if (m_letters_to_animate_option == LETTERS_TO_ANIMATE.LAST_LINE)
				{
					num6 = letters[letters.Length - 1].m_progression_variables.LineValue;
				}
				else if (m_letters_to_animate_option == LETTERS_TO_ANIMATE.NTH_WORD || m_letters_to_animate_option == LETTERS_TO_ANIMATE.NTH_LINE)
				{
					num6 = m_letters_to_animate_custom_idx - 1;
				}
				int num7 = 0;
				int num8 = 0;
				int num9 = -1;
				foreach (LetterSetup letterSetup2 in letters)
				{
					if (letterSetup2.VisibleCharacter)
					{
						if (m_letters_to_animate_option == LETTERS_TO_ANIMATE.FIRST_LINE || m_letters_to_animate_option == LETTERS_TO_ANIMATE.LAST_LINE || m_letters_to_animate_option == LETTERS_TO_ANIMATE.NTH_LINE)
						{
							if (letterSetup2.m_progression_variables.LineValue == num6)
							{
								letterSetup2.m_progression_variables.SetLetterValue(num8);
								m_letters_to_animate.Add(num7);
								num8++;
							}
						}
						else if (letterSetup2.m_progression_variables.LineValue > num4)
						{
							if (m_letters_to_animate_option == LETTERS_TO_ANIMATE.FIRST_LETTER_LINES)
							{
								letterSetup2.m_progression_variables.SetLetterValue(num8);
								m_letters_to_animate.Add(num7);
								num8++;
							}
							else if (m_letters_to_animate_option == LETTERS_TO_ANIMATE.LAST_LETTER_LINES && num9 >= 0)
							{
								letters[num9].m_progression_variables.SetLetterValue(num8);
								m_letters_to_animate.Add(num9);
								num8++;
							}
							num4 = letterSetup2.m_progression_variables.LineValue;
						}
						if (m_letters_to_animate_option == LETTERS_TO_ANIMATE.FIRST_WORD || m_letters_to_animate_option == LETTERS_TO_ANIMATE.LAST_WORD || m_letters_to_animate_option == LETTERS_TO_ANIMATE.NTH_WORD)
						{
							if (letterSetup2.m_progression_variables.WordValue == num6)
							{
								letterSetup2.m_progression_variables.SetLetterValue(num8);
								m_letters_to_animate.Add(num7);
								num8++;
							}
						}
						else if (letterSetup2.m_progression_variables.WordValue > num5)
						{
							if (m_letters_to_animate_option == LETTERS_TO_ANIMATE.FIRST_LETTER_WORDS)
							{
								letterSetup2.m_progression_variables.SetLetterValue(num8);
								m_letters_to_animate.Add(num7);
								num8++;
							}
							else if (m_letters_to_animate_option == LETTERS_TO_ANIMATE.LAST_LETTER_WORDS && num9 >= 0)
							{
								letters[num9].m_progression_variables.SetLetterValue(num8);
								m_letters_to_animate.Add(num9);
								num8++;
							}
							num5 = letterSetup2.m_progression_variables.WordValue;
						}
					}
					if (letterSetup2.VisibleCharacter)
					{
						num9 = num7;
					}
					num7++;
				}
				if ((m_letters_to_animate_option == LETTERS_TO_ANIMATE.LAST_LETTER_WORDS || m_letters_to_animate_option == LETTERS_TO_ANIMATE.LAST_LETTER_LINES) && (m_letters_to_animate.Count == 0 || m_letters_to_animate[m_letters_to_animate.Count - 1] != num9))
				{
					letters[num9].m_progression_variables.SetLetterValue(num8);
					m_letters_to_animate.Add(num9);
				}
				return;
			}
			int num10 = 0;
			int num11 = 0;
			LetterSetup letterSetup3 = null;
			m_num_white_space_chars_to_include = 0;
			int num12 = 0;
			for (int k = 0; k < num; k++)
			{
				if (letters[k].VisibleCharacter && !letters[k].StubInstance)
				{
					if (m_letters_to_animate.Contains(k))
					{
						letters[k].m_progression_variables.SetLetterValue(num10, num11);
						num10++;
						num11++;
						num12 = 0;
					}
				}
				else
				{
					letters[k].m_progression_variables.SetLetterValue(-1);
					if (letterSetup3 != null && letterSetup3.VisibleCharacter && num10 > 0)
					{
						num11++;
						m_num_white_space_chars_to_include++;
						num12++;
					}
				}
				letterSetup3 = letters[k];
			}
			m_num_white_space_chars_to_include -= num12;
		}

		public void RefreshDefaultTextColour(LetterSetup[] letters)
		{
			CalculateLettersToAnimate(letters);
			VertexColour[] array = new VertexColour[m_letters_to_animate.Count];
			for (int i = 0; i < m_letters_to_animate.Count; i++)
			{
				int num = m_letters_to_animate[i];
				if (num >= letters.Length)
				{
					break;
				}
				array[i] = letters[num].BaseColour;
			}
			m_defaultTextColourProgression.SetValues(array);
			m_defaultTextColourProgression.SetReferenceData(-1, ANIMATION_DATA_TYPE.COLOUR, true);
		}

		public void PrepareData(TextFxAnimationManager anim_manager, LetterSetup[] letters, ANIMATION_DATA_TYPE what_to_update, int num_words, int num_lines, AnimatePerOptions animate_per)
		{
			if (letters == null || letters.Length == 0)
			{
				return;
			}
			if (m_letters_to_animate == null || what_to_update == ANIMATION_DATA_TYPE.ALL || what_to_update == ANIMATION_DATA_TYPE.ANIMATE_ON)
			{
				CalculateLettersToAnimate(letters);
			}
			if (what_to_update == ANIMATION_DATA_TYPE.ALL || what_to_update == ANIMATION_DATA_TYPE.COLOUR || m_defaultTextColourProgression == null)
			{
				VertexColour[] array = new VertexColour[m_letters_to_animate.Count];
				for (int i = 0; i < m_letters_to_animate.Count; i++)
				{
					int num = m_letters_to_animate[i];
					if (num >= letters.Length)
					{
						break;
					}
					array[i] = letters[num].BaseColour;
				}
				m_defaultTextColourProgression.SetValues(array);
				m_defaultTextColourProgression.SetReferenceData(-1, ANIMATION_DATA_TYPE.COLOUR, true);
			}
			LetterAction prev_action = null;
			bool prev_action_end_state = true;
			for (int j = 0; j < m_letter_actions.Count; j++)
			{
				LetterAction letterAction = m_letter_actions[j];
				letterAction.PrepareData(anim_manager, ref letters, this, j, what_to_update, m_letters_to_animate.Count, m_num_white_space_chars_to_include, num_words, num_lines, prev_action, animate_per, m_defaultTextColourProgression, prev_action_end_state);
				if (letterAction.m_action_type == ACTION_TYPE.ANIM_SEQUENCE)
				{
					prev_action_end_state = true;
					prev_action = letterAction;
				}
				foreach (ActionLoopCycle loop_cycle in m_loop_cycles)
				{
					if (loop_cycle.m_end_action_idx == j && loop_cycle.m_loop_type == LOOP_TYPE.LOOP_REVERSE && !loop_cycle.m_finish_at_end)
					{
						prev_action = m_letter_actions[loop_cycle.m_start_action_idx];
						prev_action_end_state = false;
					}
				}
			}
		}

		public tfxJSONValue ExportDataAsPresetSection(bool saveSampleTextData = true)
		{
			tfxJSONObject tfxJSONObject = new tfxJSONObject();
			if (m_loop_cycles.Count > 0)
			{
				tfxJSONArray tfxJSONArray = new tfxJSONArray();
				foreach (ActionLoopCycle loop_cycle in m_loop_cycles)
				{
					tfxJSONArray.Add(loop_cycle.ExportData());
				}
				tfxJSONObject["LOOPS_DATA"] = tfxJSONArray;
			}
			tfxJSONArray tfxJSONArray2 = new tfxJSONArray();
			foreach (LetterAction letter_action in m_letter_actions)
			{
				tfxJSONArray2.Add(letter_action.ExportData());
			}
			tfxJSONObject["ACTIONS_DATA"] = tfxJSONArray2;
			if (saveSampleTextData)
			{
				tfxJSONObject["SAMPLE_NUM_LETTERS_ANIMATED"] = m_letters_to_animate.Count;
			}
			return new tfxJSONValue(tfxJSONObject);
		}

		public void ImportPresetSectionData(tfxJSONObject json_data, LetterSetup[] letters, string assetNameSuffix = "")
		{
			m_letter_actions = new List<LetterAction>();
			m_loop_cycles = new List<ActionLoopCycle>();
			ImportPresetSectionData(json_data, letters, 0, 0, assetNameSuffix);
		}

		public void ImportPresetSectionData(tfxJSONObject json_data, LetterSetup[] letters, int action_insert_index, int loop_insert_index, string assetNameSuffix = "")
		{
			int num_actions_added = 0;
			int num_loops_added = 0;
			ImportPresetSectionData(json_data, letters, action_insert_index, loop_insert_index, ref num_actions_added, ref num_loops_added, assetNameSuffix);
		}

		public void ImportPresetSectionData(tfxJSONObject json_data, LetterSetup[] letters, int action_insert_index, int loop_insert_index, ref int num_actions_added, ref int num_loops_added, string assetNameSuffix = "")
		{
			if (m_letter_actions == null)
			{
				m_letter_actions = new List<LetterAction>();
			}
			float timing_scale = -1f;
			if (m_letters_to_animate == null || m_letters_to_animate.Count == 0)
			{
				CalculateLettersToAnimate(letters);
			}
			if (json_data.ContainsKey("SAMPLE_NUM_LETTERS_ANIMATED") && m_letters_to_animate != null && m_letters_to_animate.Count > 0)
			{
				timing_scale = (float)m_letters_to_animate.Count / (float)json_data["SAMPLE_NUM_LETTERS_ANIMATED"].Number;
			}
			num_actions_added = 0;
			foreach (tfxJSONValue item in json_data["ACTIONS_DATA"].Array)
			{
				LetterAction letterAction = new LetterAction();
				letterAction.ImportData(item.Obj, assetNameSuffix, timing_scale);
				if (num_actions_added == 0 && action_insert_index > 0)
				{
					letterAction.m_offset_from_last = true;
				}
				InsertAction(action_insert_index + num_actions_added, letterAction);
				num_actions_added++;
			}
			if (m_loop_cycles == null)
			{
				m_loop_cycles = new List<ActionLoopCycle>();
			}
			num_loops_added = 0;
			if (!json_data.ContainsKey("LOOPS_DATA"))
			{
				return;
			}
			foreach (tfxJSONValue item2 in json_data["LOOPS_DATA"].Array)
			{
				ActionLoopCycle actionLoopCycle = new ActionLoopCycle();
				actionLoopCycle.ImportData(item2.Obj);
				actionLoopCycle.m_start_action_idx += action_insert_index;
				actionLoopCycle.m_end_action_idx += action_insert_index;
				if (actionLoopCycle.m_start_action_idx < m_letter_actions.Count && actionLoopCycle.m_end_action_idx < m_letter_actions.Count)
				{
					m_loop_cycles.Insert(loop_insert_index + num_loops_added, actionLoopCycle);
					num_loops_added++;
				}
			}
		}

		public tfxJSONValue ExportData()
		{
			tfxJSONObject tfxJSONObject = new tfxJSONObject();
			tfxJSONObject["m_letters_to_animate"] = m_letters_to_animate.ExportData();
			tfxJSONObject["m_letters_to_animate_custom_idx"] = m_letters_to_animate_custom_idx;
			tfxJSONObject["m_letters_to_animate_option"] = (double)m_letters_to_animate_option;
			if (m_loop_cycles.Count > 0)
			{
				tfxJSONArray tfxJSONArray = new tfxJSONArray();
				foreach (ActionLoopCycle loop_cycle in m_loop_cycles)
				{
					tfxJSONArray.Add(loop_cycle.ExportData());
				}
				tfxJSONObject["LOOPS_DATA"] = tfxJSONArray;
			}
			tfxJSONArray tfxJSONArray2 = new tfxJSONArray();
			foreach (LetterAction letter_action in m_letter_actions)
			{
				tfxJSONArray2.Add(letter_action.ExportData());
			}
			tfxJSONObject["ACTIONS_DATA"] = tfxJSONArray2;
			return new tfxJSONValue(tfxJSONObject);
		}

		public void ImportData(tfxJSONObject json_data, string assetNameSuffix = "")
		{
			m_letters_to_animate = json_data["m_letters_to_animate"].Array.JSONtoListInt();
			m_letters_to_animate_custom_idx = (int)json_data["m_letters_to_animate_custom_idx"].Number;
			m_letters_to_animate_option = (LETTERS_TO_ANIMATE)json_data["m_letters_to_animate_option"].Number;
			m_letter_actions = new List<LetterAction>();
			foreach (tfxJSONValue item in json_data["ACTIONS_DATA"].Array)
			{
				LetterAction letterAction = new LetterAction();
				letterAction.ImportData(item.Obj, assetNameSuffix);
				m_letter_actions.Add(letterAction);
			}
			m_loop_cycles = new List<ActionLoopCycle>();
			if (!json_data.ContainsKey("LOOPS_DATA"))
			{
				return;
			}
			foreach (tfxJSONValue item2 in json_data["LOOPS_DATA"].Array)
			{
				ActionLoopCycle actionLoopCycle = new ActionLoopCycle();
				actionLoopCycle.ImportData(item2.Obj);
				if (actionLoopCycle.m_start_action_idx < m_letter_actions.Count && actionLoopCycle.m_end_action_idx < m_letter_actions.Count)
				{
					m_loop_cycles.Add(actionLoopCycle);
				}
			}
		}

		public void UpdateLoopCyclesAfterIndex(int index_of_change, int offset_amount)
		{
			foreach (ActionLoopCycle loop_cycle in m_loop_cycles)
			{
				if (loop_cycle.m_start_action_idx >= index_of_change)
				{
					loop_cycle.m_start_action_idx += offset_amount;
					loop_cycle.m_end_action_idx += offset_amount;
				}
			}
		}
	}
}
