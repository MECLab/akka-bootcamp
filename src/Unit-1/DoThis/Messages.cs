namespace WinTail
{
	public class Messages
	{
		#region Neutral/system messages
		/// <summary>Marker class to signal a 'continue processing state'</summary>
		public struct ContinueProcessing {}
		#endregion

		#region Success messages
		public struct InputSuccess
		{
			public InputSuccess(string reason) : this()
			{
				Reason = reason;
			}

			public string Reason { get; private set; }
		}
		#endregion

		#region Error messages
		public class InputError
		{
			public InputError(string reason)
			{
				Reason = reason;
			}

			public string Reason { get; private set; }
		}

		public class NullInput : InputError
		{
			public NullInput(string reason) : base(reason)
			{
			}
		}

		public class ValidationError : InputError
		{
			public ValidationError(string reason) : base(reason)
			{
			}
		}
		#endregion
	}
}
