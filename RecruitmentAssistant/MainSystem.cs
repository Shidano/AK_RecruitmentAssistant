using System.Diagnostics;
using System.Text.RegularExpressions;
namespace RecruitmentAssistant
{
    public partial class RecruitmentAssistant : Form
    {
        FileSystemWatcher fw;
        string filename = "";
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
            lock (filename)
            {

                try
                {
                    if (filename == e.Name)
                    {
                        return;
                    }
                    filename = e.Name;
                    string FolderPath = Path.GetDirectoryName(e.FullPath);
                    string Filename = e.Name;

                    Trace.WriteLine("File:" + e.FullPath);
                    var t = Sub.RunOCR(FolderPath, Filename).Result;
                    string text = t.Text.Replace(" ", "");
                    Trace.WriteLine("OCR:" + text);
                    foreach (string[] item in TagData.FixData)
                    {
                        text = text.Replace(item[0], item[1]);
                    }
                    string result = "";
                    int i = 0;
                    foreach (string tag in TagData.TagList)
                    {
                        if (text.Contains(tag))
                        {
                            result += tag + " ";
                            text = text.Replace(tag, " ");
                            i++;
                        }
                    }
                    Trace.WriteLine("tags:" + result);
                    if (i == 0)
                    {
                        return; //タグがなかった=判定不要
                    }
                    else if (i < 5)
                    {
                        result += " " + text;
                    }
                    string res = "";
                    if (!Sub.pushTagsAsync(result, ref res))
                    {
                        new Microsoft.Toolkit.Uwp.Notifications.ToastContentBuilder().AddText(res).AddInlineImage(new Uri(Path.Combine(FolderPath, Filename))).Show();
                        return;
                    }

                    var resDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(res);

                    var toast = new Microsoft.Toolkit.Uwp.Notifications.ToastContentBuilder().AddText(resDict["title"]).AddInlineImage(new Uri(Path.Combine(FolderPath, Filename))).AddText(resDict["reply"]);
                    toast.Show();

                    if (i == 5 && File.Exists(Path.Combine(FolderPath, TagData.SettingFolderPath, TagData.AutoDeleteFileName))){
                        File.Delete(e.FullPath);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void verToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = TagData.Github_ReadmeLink,
                UseShellExecute = true,
            });
        }
    }
}
