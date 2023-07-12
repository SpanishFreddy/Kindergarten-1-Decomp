using System;
using System.Collections.Generic;

namespace TextFx
{
	[Serializable]
	public struct AnimationStateVariables
	{
		public bool m_active;

		public bool m_waiting_to_sync;

		public bool m_started_action;

		public float m_break_delay;

		public float m_timer_offset;

		public int m_action_index;

		public bool m_reverse;

		public int m_action_index_progress;

		public int m_prev_action_index;

		public float m_linear_progress;

		public float m_action_progress;

		private List<ActionLoopCycle> m_active_loop_cycles;

		public List<ActionLoopCycle> ActiveLoopCycles
		{
			get
			{
				if (m_active_loop_cycles == null)
				{
					m_active_loop_cycles = new List<ActionLoopCycle>();
				}
				return m_active_loop_cycles;
			}
			set
			{
				m_active_loop_cycles = value;
			}
		}

		public AnimationStateVariables Clone()
		{
			List<ActionLoopCycle> list = null;
			if (m_active_loop_cycles != null)
			{
				list = new List<ActionLoopCycle>();
				if (m_active_loop_cycles.Count > 0)
				{
					for (int i = 0; i < m_active_loop_cycles.Count; i++)
					{
						list.Add(m_active_loop_cycles[i].Clone());
					}
				}
			}
			AnimationStateVariables result = default(AnimationStateVariables);
			result.m_active = m_active;
			result.m_waiting_to_sync = m_waiting_to_sync;
			result.m_started_action = m_started_action;
			result.m_break_delay = m_break_delay;
			result.m_timer_offset = m_timer_offset;
			result.m_action_index = m_action_index;
			result.m_reverse = m_reverse;
			result.m_action_index_progress = m_action_index_progress;
			result.m_prev_action_index = m_prev_action_index;
			result.m_linear_progress = m_linear_progress;
			result.m_action_progress = m_action_progress;
			result.m_active_loop_cycles = list;
			return result;
		}

		public void Reset(int starting_action_index = 0)
		{
			m_active = false;
			m_waiting_to_sync = false;
			m_started_action = false;
			m_break_delay = 0f;
			m_timer_offset = 0f;
			m_action_index = starting_action_index;
			m_reverse = false;
			m_action_index_progress = 0;
			m_prev_action_index = starting_action_index - 1;
			m_linear_progress = 0f;
			m_action_progress = 0f;
			if (m_active_loop_cycles != null)
			{
				m_active_loop_cycles.Clear();
			}
		}
	}
}
