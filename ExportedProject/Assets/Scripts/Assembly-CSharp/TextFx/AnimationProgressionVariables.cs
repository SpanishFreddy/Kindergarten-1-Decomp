using System;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public class AnimationProgressionVariables
	{
		[SerializeField]
		private int m_letter_value;

		[SerializeField]
		private int m_letter_value_inc_white_space;

		[SerializeField]
		private int m_word_value;

		[SerializeField]
		private int m_line_value;

		public int LetterValue
		{
			get
			{
				return m_letter_value;
			}
		}

		public int WordValue
		{
			get
			{
				return m_word_value;
			}
			set
			{
				m_word_value = value;
			}
		}

		public int LineValue
		{
			get
			{
				return m_line_value;
			}
			set
			{
				m_line_value = value;
			}
		}

		public AnimationProgressionVariables(int letter_val, int word_val, int line_val)
		{
			m_letter_value = letter_val;
			m_word_value = word_val;
			m_line_value = line_val;
		}

		public void SetLetterValue(int letter_idx, int letter_idx_inc_white_space = -1)
		{
			m_letter_value = letter_idx;
			m_letter_value_inc_white_space = ((letter_idx_inc_white_space >= 0) ? letter_idx_inc_white_space : letter_idx);
		}

		public int GetValue(AnimatePerOptions animate_per, bool consider_white_space = false)
		{
			switch (animate_per)
			{
			case AnimatePerOptions.LETTER:
				return (!consider_white_space) ? m_letter_value : m_letter_value_inc_white_space;
			case AnimatePerOptions.WORD:
				return m_word_value;
			case AnimatePerOptions.LINE:
				return m_line_value;
			default:
				return m_letter_value;
			}
		}
	}
}
