using System;
using System.IO;
using System.IO.Pipes;

class NamedPipeClient
{

    private static void client()
    {
        NamedPipeClientStream pipeClient = new NamedPipeClientStream(".",
          "channell1", PipeDirection.InOut, PipeOptions.None);

        if (pipeClient.IsConnected != true) { pipeClient.Connect(); }

        StreamReader streamReader = new StreamReader(pipeClient);
        StreamWriter streamWriter = new StreamWriter(pipeClient);          
        try
            {
                
                streamWriter.WriteLine("здорова");
                streamWriter.Flush();
                
                string len = streamReader.ReadLine();
                Console.WriteLine("hength of string = " + len);
                pipeClient.Close();
            }
            catch (Exception ex) { throw ex; }
        
    }

    public static void Main()
    {
        client();
        Console.Read();
    }
}