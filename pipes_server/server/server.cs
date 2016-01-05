using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Text;


public class PipeServer
{
    bool running;
    Thread runningThread;
    
    public string PipeName { get; set; }


    

    PipeServer(string pipeName)
    {
        this.PipeName = pipeName;
    }

    void ServerLoop()
    {
        while (running)
        {
            ProcessNextClient();
        }

     
    }

    public void Run()
    {
        running = true;
        runningThread = new Thread(ServerLoop);
        runningThread.Start();

        runningThread.Join();
        
    }

    public void Stop()
    {
        running = false;
    
    }

   

    public void ProcessClientThread(object o)
    {
        try
        {
            NamedPipeServerStream pipeStream = (NamedPipeServerStream)o;

            var streamReader = new StreamReader(pipeStream);
            var streamWriter = new StreamWriter(pipeStream);



            String requestString = streamReader.ReadLine();

            Console.WriteLine(
                "PipeStream Instance#" + pipeStream.GetHashCode() +
                " recieved message: " + requestString);



            Thread.Sleep(3000);
            streamWriter.WriteLine(requestString.Length);
            streamWriter.Flush();
            pipeStream.WaitForPipeDrain();

            pipeStream.Close();
            pipeStream.Dispose();
        }
        catch (System.IO.IOException e)
        {
            Console.WriteLine("error while processing client" + e);
        }
    }

    public void ProcessNextClient()
    {
        try
        {
            NamedPipeServerStream pipeStream = new NamedPipeServerStream(PipeName, PipeDirection.InOut, 10);
            pipeStream.WaitForConnection();

              ThreadPool.QueueUserWorkItem(ProcessClientThread, pipeStream);
            
         
        }
        catch (Exception e)
        {     }
    }

    public static void Main()
    {
        ThreadPool.SetMaxThreads(2, 2);
        PipeServer server = new PipeServer("channell1");
        server.Run();

        
    }
}