using System.Diagnostics;
using System.Text.RegularExpressions;
namespace RecruitmentAssistant
{
    public partial class RecruitmentAssistant : Form
    {
        FileSystemWatcher fw;
        public RecruitmentAssistant()
        {
            InitializeComponent();
            Trace.WriteLine("watch:" + AppDomain.CurrentDomain.BaseDirectory);
            fw = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory);
            fw.Changed += Detected;
            fw.NotifyFilter = NotifyFilters.LastWrite;
            fw.EnableRaisingEvents = true;
            Trace.WriteLine("start;");
        }

        async void Detected(object sender, FileSystemEventArgs e)
        {
            await Task.Delay(200);
            try {
                string FolderPath = Path.GetDirectoryName(e.FullPath);
                string Filename = e.Name;

                Trace.WriteLine("File:"+e.FullPath);
                var t = await Sub.RunOCR(FolderPath, Filename);
                string text = t.Text.Replace(" ", "");
                Trace.WriteLine("OCR:"+text);
                foreach (string[] item in TagData.FixData)
                {
                    text = text.Replace(item[0], item[1]);
                }
                string result = "";
                foreach (string tag in TagData.TagList)
                {
                    if (text.Contains(tag))
                    {
                        result += tag + "\\n";
                        text=text.Replace(tag, "");
                    }
                }
                Trace.WriteLine("tags:" + result);
                if (result.Length == 0)
                {
                    return; //タグがなかった=判定不要
                }
                string data = Regex.Unescape(await Sub.pushTagsAsync(result)).Replace("\"", "");
                string reply = data.Substring(0, data.IndexOf(",title")).Replace("{reply:", "");
                string title = Regex.Match(data, "title:.*").Value.Replace("title:", "").Replace("}", "");

                new Microsoft.Toolkit.Uwp.Notifications.ToastContentBuilder().AddText(title).AddInlineImage(new Uri(Path.Combine(FolderPath, Filename))).AddText(reply).Show();
            }catch(Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
