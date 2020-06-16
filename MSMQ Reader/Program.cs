using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MSMQ_Test
{
    class Program
    {
        static MessageQueue q;
        static void Main(string[] args)
        {
            //Get message queue or create it if it doesn't exist
            if (MessageQueue.Exists(@".\Private$\Test"))
                q = MessageQueue.GetPrivateQueuesByMachine("localhost")[0];
            else
                q = MessageQueue.Create(@".\Private$\Test");

            //Binary formatter required for deserialization attack to work
            q.Formatter = new BinaryMessageFormatter();

            Console.WriteLine("Running Press Enter to Stop");
            new Thread(new ThreadStart(ReadJob)).Start();
            Console.ReadLine();
            Environment.Exit(0);
        }

        static void ReadJob()
        {
            //Read any message and print to console
            while (true)
            {
                var msg = q.Receive();
                Console.WriteLine(msg.Body);
            }
        }
    }
}
