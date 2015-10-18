using System;
using System.Runtime.CompilerServices;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Shutdown"/>.
    /// </summary>
    class ConsoleReaderActor : UntypedActor
    {
	    public const string StartCommand = "start";
		public const string ExitCommand = "exit";

        private readonly IActorRef _consoleWriterActor;

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
	        if (string.Equals(message.ToString(), StartCommand, StringComparison.OrdinalIgnoreCase))
	        {
		        PrintInstructions();
	        }

	        if (message is Messages.InputError)
	        {
		        _consoleWriterActor.Tell(message);
	        }

			GetAndValidateInput();
        }

		private static void PrintInstructions()
		{
			Console.WriteLine("Write whatever you want into the console!");
			Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
			Console.WriteLine("Type 'exit' to quit this application at any time.\n");
		}

	    private void GetAndValidateInput()
	    {
			var cmd = Console.ReadLine();
		    if (string.IsNullOrEmpty(cmd))
		    {
			    Self.Tell(new Messages.NullInput("No input received."));
				return;
		    }
		    
			if (string.Equals(cmd, ExitCommand, StringComparison.OrdinalIgnoreCase))
			{
				// shut down the system (acquire handle to system via this actors context)
				Context.System.Shutdown();
				return;
			}

		    if (!IsValid(cmd))
		    {
				_consoleWriterActor.Tell(new Messages.InputSuccess("Thank you message validation was successful."));
				Self.Tell(new Messages.ContinueProcessing());
				return;
		    }

			Self.Tell(new Messages.ValidationError("Invalid: input had odd number of characters."));
	    }

	    private static bool IsValid(string cmd)
	    {
		    return cmd.Length%2 == 0;
	    }
    }
}