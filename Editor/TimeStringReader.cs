using System;

namespace Huchell.Unity.Editor
{
	internal ref struct TimeStringReader
	{
		private ReadOnlySpan<char> time;
		private int index;

		private Token currentToken;

		public Token CurrentToken
		{
			get
			{
				if (this.index == 0) throw new InvalidOperationException("Tried to access CurrentToken before calling Seek");
				return this.currentToken;
			}
		}

		public Exception SeekException { get; private set; }

		public TimeStringReader(ReadOnlySpan<char> time)
		{
			this.time = time;
			this.index = 0;
			this.currentToken = default;
			this.SeekException = null;
		}

		public bool Seek()
		{
			if (this.index >= this.time.Length)
			{
				return false;
			}

			this.index += Seek(this.index, this.time, char.IsWhiteSpace);
			if (this.index >= this.time.Length)
			{
				return false;
			}

			var advance = Seek(this.index, this.time, static ch => char.IsDigit(ch) || ch == '.');
			if (this.index + advance >= this.time.Length)
			{
				return false;
			}

			var slice = this.time.Slice(this.index, advance);
			var value = double.Parse(slice);
			this.index += advance;

			advance = Seek(this.index, this.time, char.IsLetter);
			var unit = this.time.Slice(this.index, advance);
			this.index += advance;

			this.currentToken = new(value, unit);
			return true;
		}

		private static int Seek(int startIndex, ReadOnlySpan<char> span, Predicate<char> predicate)
		{
			int index = startIndex;
			while (index < span.Length && predicate(span[index]))
			{
				index++;
			}

			return index - startIndex;
		}

		public readonly ref struct Token
		{
			public readonly double Value;
			public readonly ReadOnlySpan<char> Unit;

			public Token(double value, ReadOnlySpan<char> unit)
			{
				this.Value = value;
				this.Unit = unit;
			}

			public TimeSpan ToTimeSpan()
			{
				if (this.Unit.SequenceEqual("ms"))
				{
					return TimeSpan.FromMilliseconds(this.Value);
				}
				if (this.Unit.SequenceEqual("s"))
				{
					return TimeSpan.FromSeconds(this.Value);
				}
				if (this.Unit.SequenceEqual("m"))
				{
					return TimeSpan.FromMinutes(this.Value);
				}
				if (this.Unit.SequenceEqual("h"))
				{
					return TimeSpan.FromHours(this.Value);
				}
				if (this.Unit.SequenceEqual("d"))
				{
					return TimeSpan.FromDays(this.Value);
				}
				return default;
			}
		}
	}
}
