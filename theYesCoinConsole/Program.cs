using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using theYesCoinConsole.Controls;
using theYesCoinConsole.Models;

namespace theYesCoinConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ConfigureConsole();
            RootRunning();
            Console.ReadLine();
        }
        public static void ConfigureConsole()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Blue;
        }
        // MENU CONTROL ------------------------------------------------------------------------
        static string choose = "";
        public static void RootRunning()
        {
            string choose = "";

            while (true)
            {
                ShowMainMenu();
                choose = Console.ReadLine();

                switch (choose)
                {
                    case "1":
                        HandleRunProgram();
                        break;
                    case "2":
                        ManageAccounts();
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ, vui lòng chọn lại.");
                        break;
                }
            }
        }
        public static void ShowMainMenu()
        {
            Console.Clear();
            WriteMainPanel();
            Console.WriteLine("Chọn chức năng : ");
            Console.WriteLine();
            Console.WriteLine("1> Chạy chương trình");
            Console.WriteLine("2> Quản lý tài khoản");
            Console.WriteLine("0> Thoát chương trình");
            Console.WriteLine();
            Console.Write("Lựa chọn : ");
        }

        public static async Task HandleRunProgram()
        {
            Console.Clear();
            WritePanelRunning();
            Console.WriteLine();
            Console.WriteLine("1> Cấu hình chạy");
            Console.WriteLine("2> Chạy tất cả");
            Console.WriteLine("0> Back");
            Console.WriteLine();
            Console.Write("Lựa chọn : ");
            string run_choose = Console.ReadLine();
            Console.WriteLine(run_choose);

            switch (run_choose)
            {
                case "1":
                    Console.Clear();
                    WritePanelRunning();
                    Config config = Config.GetConfigureRun();
                    Console.WriteLine($"Cấu hình hiện tại : \n");
                    Console.WriteLine($"Nhặt {config.coinCollectAmount} mỗi {config.coinCollectSleep} giây !!\n");
                    Console.WriteLine("Nhập cấp hình mới ?\n");
                    int newAmount = 0;
                    int newSleep = 0;

                    bool validInput = false;
                    while (!validInput)
                    {
                        Console.Write("Nhập số lượng (Khuyến nghị 100) : ");
                        string inputAmount = Console.ReadLine();
                        Console.Write("Nhập thời gian chờ (Khuyến nghị 5s) : ");
                        string inputSleep = Console.ReadLine();

                        if (int.TryParse(inputAmount, out newAmount) && int.TryParse(inputSleep, out newSleep))
                        {
                            validInput = true;
                        }
                        else
                        {
                            Console.WriteLine("Số lượng và thời gian chờ phải là số nguyên. Vui lòng nhập lại.");
                        }
                    }

                    if (Config.ApplyChangeConfigure(newAmount, newSleep))
                    {
                        Console.WriteLine($"Cài đặt thành công !!! Tự động chuyển hướng sau 3 giây");
                        Thread.Sleep(3000);
                        HandleRunProgram();
                    }
                    break;
                case "2":
                    Console.Clear();
                    PrepareClaiming().GetAwaiter().GetResult();
                    break;
                case "0":
                    RootRunning();
                    break;
                default:
                    break;
            }
        }

        public static void ManageAccounts()
        {
            Console.Clear();
            WritePanelAccount();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("1> Hiển thị danh sách");
            Console.WriteLine("2> Thêm tài khoản");
            Console.WriteLine("3> Xoá tài khoản");
            Console.WriteLine("0> Back");
            Console.WriteLine();
            Console.Write("Lựa chọn : ");
            string account_choose = Console.ReadLine();
            switch (account_choose)
            {
                case "1":
                    Console.Clear();
                    WritePanelAccount();
                    Console.WriteLine(Services.getInfomationAllAccount());
                    Console.WriteLine("\nNhấn phím bất kì để quay về !!");
                    Console.ReadLine();
                    ManageAccounts();
                    break;
                case "2":
                    InitializeAccountMenu();
                    break;
                case "3":
                    DeleteAccountByUserId();
                    break;
                case "0":
                    RootRunning();
                    return;
                default:
                    ManageAccounts();
                    break;
            }
        }
        public static void WriteMainPanel()
        {
            Console.WriteLine(@"                                                        ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ 
                  ______                   __  __      │                                                       │
                 |___  /                  |  \/  |     │   - Version : 1.0.1 ( Có cập nhật )                   │
                    / /   _   _   _ __    | \  / |     │   - Phiên bản này được chia sẻ hoàn toàn MIỄN PHÍ     │
                   / /   | | | | | '__|   | |\/| |     │   - Liên hệ ADMIN để báo lỗi , góp ý                  │
                  / /__  | |_| | | |      | |  | |     │   - ZyrM - Chuyên cung cấp công cụ MMO/UG/Airdrop     │
                 /_____|  \__, | |_|      |_|  |_|     │   - Vui lòng liên hệ qua Telegram nếu có nhu cầu      │
                           __/ |                       │     thuê tool , tut....                               │
                          |___/                         ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ 
                ");
            Console.WriteLine("                                                       ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine("                                                       │                                                       │");
            Console.WriteLine("                                                       │  - 𝙏𝙚𝙡𝙚𝙜𝙧𝙖𝙢 : t.me/dckzyr ( ZyrM )                    │");
            Console.WriteLine("                                                       │  - 𝙁𝙖𝙘𝙚𝙗𝙤𝙤𝙠 : fb.com/PhTienDuck                       │");
            Console.WriteLine("                                                       │  - 𝙕𝙖𝙡𝙤 : 0926100949                                  │");
            Console.WriteLine("                                                       │  - KHÔNG XOÁ BẤT CỨ FILE GÌ TRONG QUÁ TRÌNH SỬ DỤNG   │");
            Console.WriteLine("                                                       │  - Update lần cuối : 23/5/2024                        │");
            Console.WriteLine("                                                       │  - Develop from MSquad ^^                             │");
            Console.WriteLine("                                                       ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ ");

        }
        public static void WritePanelRunning()
        {
            Console.WriteLine(@"
                    #  ______                      _                ______                    _ 
                    #  | ___ \                    (_)               | ___ \                  | |
                    #  | |_/ /_   _  _ __   _ __   _  _ __    __ _  | |_/ /__ _  _ __    ___ | |
                    #  |    /| | | || '_ \ | '_ \ | || '_ \  / _` | |  __// _` || '_ \  / _ \| |
                    #  | |\ \| |_| || | | || | | || || | | || (_| | | |  | (_| || | | ||  __/| |
                    #  \_| \_|\__,_||_| |_||_| |_||_||_| |_| \__, | \_|   \__,_||_| |_| \___||_|
                    #                                         __/ |                             
                    #                                        |___/                              
                    ");
        }
        public static void WritePanelAccount()
        {
            Console.WriteLine(@"
                    #    ___                                  _    ______                    _ 
                    #   / _ \                                | |   | ___ \                  | |
                    #  / /_\ \  ___  ___  ___   _   _  _ __  | |_  | |_/ /__ _  _ __    ___ | |
                    #  |  _  | / __|/ __|/ _ \ | | | || '_ \ | __| |  __// _` || '_ \  / _ \| |
                    #  | | | || (__| (__| (_) || |_| || | | || |_  | |  | (_| || | | ||  __/| |
                    #  \_| |_/ \___|\___|\___/  \__,_||_| |_| \__| \_|   \__,_||_| |_| \___||_|
                    #                                                                          
                    #                                                                          
                    ");
        }
        public static void InitializeAccountMenu()
        {
            Console.Clear();
            WritePanelAccount();
            Console.Write("Nhập Token để thêm tài khoản (Nhập 0 nếu muốn dừng tiến trình !! ) : ");
            string token = Console.ReadLine();
            if (token != "0")
            {
                string accountFind = Services.FindLineByToken(token);
                if (accountFind == "0")
                {
                    Services.InitializeToken(token);
                }
                else
                {
                    Console.WriteLine($"Account đã tồn tại trong hệ thống !!!");
                    Console.WriteLine("Chuyển hướng sau 2s");
                    Thread.Sleep(2000);
                    InitializeAccountMenu();
                }
            }
            else if(token == "0")
            {
                ManageAccounts();
            }
        }
        public static void DeleteAccountByUserId()
        {
            Console.Clear();
            WritePanelAccount();
            Console.Write("Nhập mã tài khoản ( userId ) để xoá khỏi hệ thống\nNhập 0 nếu muốn dừng tiến trình\nNhập 1 nếu muốn xoá tất cả danh sách ) : ");
            string userId = Console.ReadLine();

            if (userId != "0" && userId !="1")
            {
                if(Services.DeleteAccount(userId)==true)
                {
                    Console.WriteLine("Xoá thành công !!!");
                    Thread.Sleep(2000);
                    DeleteAccountByUserId();
                }
                else
                {
                    Console.WriteLine("Có vẻ tài khoản này không tồn tại ??");
                    Thread.Sleep(2000);
                    DeleteAccountByUserId();
                }
            }
            else if (userId =="1")
            {
                Console.Clear();
                WritePanelAccount();
                Console.WriteLine("BẠN CÓ THỰC SỰ MUỐN XOÁ TẤT CẢ TÀI KHOẢN ? HÀNH ĐỘNG NÀY CÓ THỂ KHIẾN BẠN MẤT TOÀN BỘ DỮ LIỆU ???\n");
                Console.WriteLine("Gõ 'YES' để xác nhận việc xoá toàn bộ !!");
                string confirm = Console.ReadLine();
                if (confirm != "YES")
                {
                    DeleteAccountByUserId();
                }
                else
                {
                    Services.DeleteAllAccounts();
                    Console.WriteLine("Đã xoá tất cả tài khoản , nhấn phím bất kì để quay trở lại Menu tài khoản");
                    Console.ReadLine();
                    ManageAccounts();
                }
            }
            else
            {
                ManageAccounts();
            }
        }
        public static void StartClaiming()
        {
            Config config = Config.GetConfigureRun();
        }
        private static async Task PrepareClaiming()
        {
            List<string> tokens = Services.ReadTokensFromFile();
            while (true)
            {
                await CollectCoinsForAccounts(tokens);
            }
        }
        private static async Task CollectCoinsForAccounts(List<string> tokens)
        {
            Config config = Config.GetConfigureRun();

            int sleepTime = config.coinCollectSleep;
            int collectAmount = config.coinCollectAmount;

            List<Task> tasks = new List<Task>();
            foreach (var token in tokens)
            {
                Console.WriteLine($"Khởi chạy tác vụ cho token: {token}");
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("Đã hoàn thành 1 vòng !!!");
        }
    }
}
