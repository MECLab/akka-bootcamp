using System;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for serializing message writes to the console.
    /// (write one message at a time, champ :)
    /// </summary>
    class ConsoleWriterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            // make sure we got a message
	        if (message is Messages.InputError)
	        {
		        var error = (Messages.InputError) message;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Input error: {0}", error.Reason);
	        }
			else if (message is Messages.InputSuccess)
			{
				var input = (Messages.InputSuccess) message;
				Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(input.Reason);
            }
			else
			{
				Console.WriteLine(message.ToString());
			}

            Console.ResetColor();
        }
    }
}
