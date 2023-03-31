global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using EnvDTE80;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualStudio;
using StreamJsonRpc;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;
namespace Aya
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("Aya", Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.AyaString)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideOptionPage(typeof(OptionsProvider.GeneralOptions), "Aya", "General", 0, 0, true, SupportsProfiles = true)]
    public sealed class AyaPackage : ToolkitPackage
    {
        public static int _interval = GetDuration()*30*60*1000;
        private static System.Timers.Timer _timer;
        private static string ayaNum, sorahNum;
        public static Dictionary<int, string> quranSorahs = new Dictionary<int, string>
{
    {1, "الفاتحة"},
    {2, "البقرة"},
    {3, "آل عمران"},
    {4, "النساء"},
    {5, "المائدة"},
    {6, "الأنعام"},
    {7, "الأعراف"},
    {8, "الأنفال"},
    {9, "التوبة"},
    {10, "يونس"},
    {11, "هود"},
    {12, "يوسف"},
    {13, "الرعد"},
    {14, "إبراهيم"},
    {15, "الحجر"},
    {16, "النحل"},
    {17, "الإسراء"},
    {18, "الكهف"},
    {19, "مريم"},
    {20, "طه"},
    {21, "الأنبياء"},
    {22, "الحج"},
    {23, "المؤمنون"},
    {24, "النور"},
    {25, "الفرقان"},
    {26, "الشعراء"},
    {27, "النمل"},
    {28, "القصص"},
    {29, "العنكبوت"},
    {30, "الروم"},
    {31, "لقمان"},
    {32, "السجدة"},
    {33, "الأحزاب"},
    {34, "سبأ"},
    {35, "فاطر"},
    {36, "يس"},
    {37, "الصافات"},
    {38, "ص"},
    {39, "الزمر"},
    {40, "غافر"},
    {41, "فصلت"},
    {42, "الشورى"},
    {43, "الزخرف"},
    {44, "الدخان"},
    {45, "الجاثية"},
    {46, "الأحقاف"},
    {47, "محمد"},
    {48, "الفتح"},
    {49, "الحجرات"},
    {50, "ق"},
    {51, "الذاريات"},
    {52, "الطور"},
    {53, "النجم"},
    {54, "القمر"},
    {55, "الرحمن"},
    {56, "الواقعة"},
    {57, "الحديد"},
    {58, "المجادلة"},
    {59, "الحشر"},
    {60, "الممتحنة"},
    {61, "الصف"},
    {62, "الجمعة"},
    {63, "المنافقون"},
    {64, "التغابن"},
    {65, "الطلاق"},
    {66, "التحريم"},
    {67, "الملك"},
    {68, "القلم"},
    {69, "الحاقة"},
    {70, "المعارج"},
    {71, "نوح"},
    {72, "الجن"},
    {73, "المزمل"},
    {74, "المدثر"},
    {75, "القيامة"},
    {76, "الانسان"},
    {77, "المرسلات"},
    {78, "النبأ"},
    {79, "النازعات"},
    {80, "عبس"},
    {81, "التكوير"},
    {82, "الانفطار"},
    {83, "المطففين"},
    {84, "الانشقاق"},
    {85, "البروج"},
    {86, "الطارق"},
    {87, "الأعلى"},
    {88, "الغاشية"},
    {89, "الفجر"},
    {90, "البلد"},
    {91, "الشمس"},
    {92, "الليل"},
    {93, "الضحى"},
    {94, "الشرح"},
    {95, "التين"},
    {96, "العلق"},
    {97, "القدر"},
    {98, "البينة"},
    {99, "الزلزلة"},
    {100, "العاديات"},
    {101, "القارعة"},
    {102, "التكاثر"},
    {103, "العصر"},
    {104, "الهمزة"},
    {105, "الفيل"},
    {106, "قريش"},
    {107, "الماعون"},
    {108, "الكوثر"},
    {109, "الكافرون"},
    {110, "النصر"},
    {111, "المسد"},
    {112, "الإخلاص"},
    {113, "الفلق"},
    {114, "الناس"}
};

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            General.Saved += OnOptionsSaved;
            SetTimer();
        }
        public static async void ShowNotification(Object o = null, EventArgs e = null)
        {
            var str = "";
            var path1 = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var path = Path.GetFullPath(Path.Combine(path1, @"..\"))+"Resources\\notification-logo.png";
            try
            {
                str = await ReadAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            try
            {
                sorahNum = str.Split('|')[0];
                ayaNum = str.Split('|')[1];
                new ToastContentBuilder()
                .SetToastScenario(ToastScenario.Default)
                .SetToastDuration(ToastDuration.Long)
                .AddArgument("action", "dismiss")
                .AddText(quranSorahs[int.Parse(str.Split('|')[0])]+" : "+ayaNum)
                .AddText(str.Split('|')[2])
                .AddAppLogoOverride(new Uri(path))
                .AddButton(new ToastButton().SetContent("تفسير الآية").AddArgument(key: "action", "tafseer"))
                .AddButton(new ToastButton().SetContent("أغلق").AddArgument("action", "dismiss"))
                .AddAudio(null, null, true)
                .Show(t => {
                    t.ExpirationTime = DateTime.Now.AddSeconds(30);
                    t.Activated += HandleToastActivation;
                });
            }catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            
        }
        public static async Task<string> ReadAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var path1 = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var path = Path.GetFullPath(Path.Combine(path1, @"..\"))+"Resources\\quran.txt";
            StreamReader sr = new StreamReader(path);
            Random rnd = new Random();
            int random = rnd.Next(4749);
            for (int i = 0; i < random; i++)
            {
                await sr.ReadLineAsync();
            }
            return await sr.ReadLineAsync();
        }
        public static async Task<General> GetOptionsAsync()
        {
            return await General.GetLiveInstanceAsync();
        }
        public static int GetDuration()
        {
            return GetOptionsAsync().Result.duration;
        }
        private void OnOptionsSaved(General e)
        {
            var options = GetOptionsAsync().Result;
            var duration = GetDuration();
            if (duration>120)
            {
                options.duration = 120;
                options.Save();
            }
            else if (duration <20)
            {
                options.duration = 20;
                options.Save();
            }
            _timer.Interval = GetDuration()*60*1000;
        }
        public static void SetTimer()
        {
            _timer = new System.Timers.Timer(_interval);
            _timer.Elapsed += ShowNotification;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }
        private static void HandleToastActivation(ToastNotification sender, object args)
        {
            ToastActivatedEventArgs strArgs = args as ToastActivatedEventArgs;
            var argsValues = strArgs.Arguments;
            switch (argsValues.Split('=')[1])
            {
                case "tafseer":
                    Process proc = new Process();
                    proc.StartInfo.UseShellExecute = true;
                    proc.StartInfo.FileName = $"http://quran.ksu.edu.sa/tafseer/katheer/sura{sorahNum}-aya{ayaNum}.html";
                    proc.Start();
                    break;
                case "dismiss":
                    break;
                case "bodyTapped":
                    break;
            }

        }
    }
}