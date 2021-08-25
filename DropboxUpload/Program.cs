using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DropboxUpload
{
    public class Program
    {
        private static readonly string _basePath = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _fileName = "Test.txt";

        // Generate OAuthKey (from https://www.dropbox.com/developers/apps) here
        private const string OAuthKey = "XXXXXXXXXXXX";

        private static async Task Main(string[] args)
        {
            await Helper();
            using (var client = new DropboxClient(OAuthKey))
            {
                var full = await client.Users.GetCurrentAccountAsync();
                await Upload(client, "Test", "Test.txt", string.Concat(_basePath, _fileName));
            }
        }

        private static async Task Upload(DropboxClient dbx, string folder, string file, string fileToUpload)
        {
            using (var mem = new MemoryStream(File.ReadAllBytes(fileToUpload)))
            {
                var updated = await dbx.Files.UploadAsync(
                   "/" folder + "/" + file,
                    WriteMode.Overwrite.Instance,
                    body: mem);
                Console.WriteLine("Saved {0}/{1} rev {2}", folder, file, updated.Rev);
            }
        }

        private static async Task Helper()
        {
            if (!File.Exists(string.Concat(_basePath, _fileName)))
            {
                using (var stream = File.CreateText(string.Concat(_basePath, _fileName)))
                {
                    await stream.WriteLineAsync("Test From Aditya Jaiswal Dev");
                }
            }
        }
    }
}