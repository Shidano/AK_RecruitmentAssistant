using System.Text;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using System.Net.Http;
namespace RecruitmentAssistant
{
    internal static class TagData
    {
        public static string Lang = "ja";
        public static string ShowLangText = "ja_JP";
        public static string ShowVerText = "ver.0.3";
        public static string Github_ReadmeLink = "https://github.com/Shidano/AK_RecruitmentAssistant/blob/main/README.md";
        public static string SettingFolderPath = "Settings";
        public static string AutoDeleteFileName = "AutoDelete_true.txt";

        public static List<string> TagList = new List<string>
        {
            // rarity tag
            "上級エリート",
            "エリート",
            "初期",
            "ロボット",
            // job tag
            "前衛タイプ",
            "狙撃タイプ",
            "重装タイプ",
            "医療タイプ",
            "補助タイプ",
            "術師タイプ",
            "特殊タイプ",
            "先鋒タイプ",
            // position tag
            "近距離",
            "遠距離",
            // normal tag
            "牽制",
            "爆発力",
            "治療",
            "支援",
            "COST回復",
            "火力",
            "生存",
            "範囲攻撃",
            "防御",
            "減速",
            "弱化",
            "高速再配置",
            "強制移動",
            "召喚",
            "元素",
            "高空",
        };

        public static List<string[]> FixData = new List<string[]>
        {
            // {"similar","fixed"},
            new string[]{"工","エ"},
            new string[]{"一","ー"},
            new string[]{"夕","タ"},
            new string[]{"卜","ト"},
            new string[]{"り","リ"},
            new string[]{"ブ","プ"},
            new string[]{"カ","力"},
            new string[]{"口","ロ"},
            new string[]{"COST","COST回復"},
            new string[]{"タイ","タイプ"},
        };

    }
}