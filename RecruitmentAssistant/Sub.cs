using System.Text;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using System.Net.Http;
namespace RecruitmentAssistant
{
    internal static class Sub
    {
        /// <summary>
        ///  改行区切りでタグを投入
        /// </summary>
        internal static async Task<string> pushTagsAsync(string tags)
        {
            string url = "https://discordbot-riseicalculator.herokuapp.com/recruitment/";

            var json = "{ \"text\" : \"" + tags+"\" }";

            var content = new StringContent(json, Encoding.UTF8, "applicartion/json");
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else{
                    return "Internet Error";
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