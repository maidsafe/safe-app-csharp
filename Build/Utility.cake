#addin nuget:?package=Cake.FileHelpers
using System.Net;
using System.Net.Sockets;

var TCP_LISTEN_TIMEOUT = 300;

Func<IPAddress, int, string, Task> DownloadTcpTextAsync = (IPAddress TCP_LISTEN_HOST, int TCP_LISTEN_PORT, string RESULTS_PATH) => System.Threading.Tasks.Task.Run(() =>
{
    var server = new TcpListener(TCP_LISTEN_HOST, TCP_LISTEN_PORT);
    server.Start();
    var listening = true;

    System.Threading.Tasks.Task.Run(() => {
        // Sleep until timeout elapses or tcp listener stopped after a successful connection
        var elapsed = 0;
        while (elapsed <= TCP_LISTEN_TIMEOUT && listening) {
            System.Threading.Thread.Sleep(1000);
            elapsed++;
        }

        // If still listening, timeout elapsed, stop the listener
        if (listening) {
            server.Stop();
            listening = false;
        }
    });

    try
    {
        TcpClient client = server.AcceptTcpClient();
        NetworkStream stream = client.GetStream();
        StreamReader data_in = new StreamReader(client.GetStream());
        var result = data_in.ReadToEnd();
        System.IO.File.AppendAllText(RESULTS_PATH, result);
        client.Close();
        server.Stop();
        listening = false;
    }
    catch
    {
        Information("Test results listener failed or timed out.");
        throw new Exception();
    }
});

void AnalyseResultFile(string FilePath)
{
    string line;
    var file = new StreamReader(FilePath);
    while ((line = file.ReadLine()) != null)
    {
        foreach (var word in line.Split(' '))
        {
            if (word == @"result=""Failed""")
            {
                throw new Exception("Tests Failed");
            }
        }
    }
    file.Close();
}
