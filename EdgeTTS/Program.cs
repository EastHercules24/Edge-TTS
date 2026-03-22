using EdgeTTS.DotNet;
using EdgeTTS.DotNet.Models;
using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace EdgeTTS
{
    class Program
    {
        static void Main(string[] args)
        {
            // Starting HttpListener.
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/");
            listener.Start();

            Console.WriteLine("Listening...");

            // Running task.
            Task.Run(async () =>
            {
                // Do task while listener is listening.
                while (listener.IsListening)
                {
                    try 
                    { 
                        // Idk...
                        var ctx = await listener.GetContextAsync();
                        _ = Task.Run(() =>
                        {
                            // Get request and response from HttpListener.
                            HttpListenerRequest request = ctx.Request;
                            HttpListenerResponse response = ctx.Response;

                            // Output received requests.
                            Console.WriteLine($"Received request: {request.HttpMethod} {request.Url}");

                            // Block favicon requests
                            if (request.Url.ToString().Contains("favicon.ico"))
                            {
                                Console.WriteLine("Blocked Url.");
                                response.Close();
                            }
                            else
                            {

                                // Get queries.
                                var re1 = request.QueryString["text"];
                                var re2 = request.QueryString["voice"];
                                var re3 = request.QueryString["filetype"];
                                var re4 = request.QueryString["rate"];
                                var re5 = request.QueryString["volume"];
                                var re6 = request.QueryString["pitch"];

                                // If one or more queries wasn't given, then give a default.
                                if (re1 == null)
                                {
                                    re1 = "Error. No Text Given!";
                                }
                                if (re2 == null)
                                {
                                    re2 = "en-US-EmmaMultilingualNeural";
                                }
                                if (re3 == null)
                                {
                                    re3 = "mp3";
                                }
                                if (re4 == null)
                                {
                                    re4 = "+0%";
                                }
                                if (re5 == null)
                                {
                                    re5 = "+0%";
                                }
                                if (re6 == null)
                                {
                                    re6 = "+0Hz";
                                }
                                
                                // If the queries doesn't start with a + or a -, then add them.
                                if (!re4.ToString().Contains("+"))
                                {
                                    re4 = "+" + re4;
                                } 
                                else if (!re4.ToString().Contains("-"))
                                {
                                    re4 = "-" + re4;
                                }
                                if (!re5.ToString().Contains("+"))
                                {
                                    re5 = "+" + re5;
                                } 
                                else if (!re5.ToString().Contains("-"))
                                {
                                    re5 = "-" + re5;
                                }
                                if (!re6.ToString().Contains("+"))
                                {
                                    re6 = "+" + re6;
                                } 
                                else if (!re6.ToString().Contains("-"))
                                {
                                    re6 = "-" + re6;
                                }

                                // Output of the received queries.
                                Console.WriteLine("Text: " + re1 + "\nVoice: " + re2 + "\nFiletype: " + re3);

                                // Combine the base directory and the file name.
                                string current_file_name = AppDomain.CurrentDomain.BaseDirectory + "/voice_" + re1 + "-" + re2 + ".mp3";

                                // Convert text to speach and save.
                                var request2 = new Communicate(re1.ToString(), voice: re2.ToString(), rate: re4.ToString(), volume: re5.ToString(), pitch: re6.ToString());
                                request2.SaveAsync(current_file_name);

                                // Check if file is being used by the tts.
                                bool fileInUse = IsFileInUse(current_file_name);

                                Console.Write("Wait untill ready.");
                                while (fileInUse)
                                {
                                    fileInUse = IsFileInUse(current_file_name);
                                    Console.Write(".");
                                }

                                // Now check after the loop if the file isn't used anymore.
                                if (fileInUse == false)
                                {
                                    Console.WriteLine("\nDone.");
                                    byte[] buffer = File.ReadAllBytes(current_file_name);

                                    response.ContentLength64 = buffer.Length;
                                    response.ContentType = "audio/" + re3.ToString().ToLower();

                                    System.IO.Stream output = response.OutputStream;
                                    output.Write(buffer, 0, buffer.Length);
                                    output.Close();

                                    File.Delete(current_file_name);
                                }
                                else // If it still in use... Yeah you're fucked.
                                {
                                    Console.WriteLine("\nError: File was still in use!");
                                }
                            }
                        });
                    }
                    catch (Exception ex) // Whenever a error appears.
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            });

            // ReadLine to stop program to close.
            Console.ReadLine();
        }

        // Got that of the internet becaue I couldn't figure out this. 
        public static bool IsFileInUse(string filePath)
        {
            try
            {
                // Try opening the file with read-write access and an exclusive lock
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    // If we can open it, the file isn't in use
                }
            }
            catch (IOException)
            {
                // IOException indicates the file is in use
                return true;
            }

            // If no exception was thrown, the file is not in use
            return false;
        }
    }
}