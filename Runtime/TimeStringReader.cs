using System;

namespace Duration
{
	internal ref struct TimeStringReader
	{
		private readonly ReadOnlySpan<char> time;
		private int index;

		private Token currentToken;

		public readonly Token CurrentToken
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

			var valueAdvance = Seek(this.index, this.time, IsValidCharForValue);
			if (this.index + valueAdvance >= this.time.Length)
			{
				return false;
			}

			var valueSlice = this.time.Slice(this.index, valueAdvance);

			var unitAdvance = Seek(this.index + valueAdvance, this.time, IsValidCharForUnit);
			var unit = this.time.Slice(this.index + valueAdvance, unitAdvance);

			this.currentToken = new(this.index, valueAdvance, unitAdvance, this.time);
			this.index += unitAdvance + valueAdvance;
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

		private static bool IsValidCharForValue(char ch) => char.IsDigit(ch) || ch == '.' || ch == ',';
		private static bool IsValidCharForUnit(char ch) => char.IsLetter(ch);

		public readonly ref struct Token
		{
			private readonly int startIndex;
			private readonly int valueLength;
			private readonly int unitLength;
			private readonly ReadOnlySpan<char> time;

			public double Value => double.Parse(this.time[this.startIndex..(this.startIndex + this.valueLength)]);
			public ReadOnlySpan<char> Unit => this.time[(this.startIndex + this.valueLength)..(this.startIndex + this.valueLength + this.unitLength)];

			public Token(int startIndex, int valueLength, int unitLength, ReadOnlySpan<char> time)
			{
				this.time = time;
				this.startIndex = startIndex;
				this.valueLength = valueLength;
				this.unitLength = unitLength;
			}

			public TimeSpan? ToTimeSpan()
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
