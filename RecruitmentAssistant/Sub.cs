using System.Text;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using System.Net.Http;
using System.Diagnostics;
namespace RecruitmentAssistant
{
    internal static class Sub
    {
        /// <summary>
        ///  改行区切りでタグを投入
        /// </summary>
        internal static bool pushTagsAsync(string tags, ref string result)
        {
            string url = "https://discordbot-riseicalculator.herokuapp.com/recruitment/";

            Dictionary<string, string> json = new Dictionary<string, string>();
            json.Add("text", tags);
            var jsonstring = System.Text.Json.JsonSerializer.Serialize(json);
            var content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
            Trace.WriteLine(jsonstring);
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(url, content).Result; 
                Trace.WriteLine($"Status:{response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                    return true;
                }
                else{
                    result = $"Internet Error:{response.StatusCode}";
                    return false;
                }
            }

        }

        /// <summary>
        ///  OCR実行
        /// </summary>
        internal static async Task<OcrResult> RunOCR(string FolderPath,string FileName)
        {
            StorageFolder appFolder = await StorageFolder.GetFolderFromPathAsync(FolderPath);
            SoftwareBitmap softwareBitmap;
            var bmpFile = await appFolder.GetFileAsync(FileName);
            using (IRandomAccessStream stream = await bmpFile.OpenAsync(FileAccessMode.Read))
            {
                // Create the decoder from the stream
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                // Get the SoftwareBitmap representation of the file
                softwareBitmap = await decoder.GetSoftwareBitmapAsync();
            }
            Windows.Globalization.Language language = new Windows.Globalization.Language(TagData.Lang);
            OcrEngine ocrEngine = OcrEngine.TryCreateFromLanguage(language);

            var ocrResult = await ocrEngine.RecognizeAsync(softwareBitmap);
            return ocrResult;
        }
    }
}