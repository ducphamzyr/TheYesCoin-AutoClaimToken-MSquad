using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using theYesCoinConsole.Models;
using System.Net.Http;
using System.Security.Policy;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Web.UI;
using System.Threading;

namespace theYesCoinConsole.Controls
{
    public class Services
    {
        public static async Task InitializeToken(string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.yescoin.gold/");
            httpClient.DefaultRequestHeaders.Add("Token", token);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36");

            HttpResponseMessage response = await httpClient.GetAsync("account/getAccountInfo");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                JObject responseJson = JObject.Parse(responseBody);
                JObject data = (JObject)responseJson["data"];
                int totalAmount = (int)data["totalAmount"];
                int currentAmount = (int)data["currentAmount"];
                int rank = (int)data["rank"];
                int userLevel = (int)data["userLevel"];
                string userId = (string)data["userId"];
                Account account = new Account(userId, userLevel, rank, totalAmount, currentAmount, token);
                AddAccountToFile(account);

            }
            else
            {
                Console.WriteLine($"Failed to get data, status code: {response.StatusCode}");
            }
        }
        public static async Task InitializeCollect(string token, int sleep, int amount)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.yescoin.gold/");
                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36");
                while (true)
                {
                    var postData = amount;
                    var json = JsonConvert.SerializeObject(postData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("game/collectCoin", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        if (responseContent.Contains("Success"))
                        {
                            Account account = await GetInfomationNow(token);
                            Console.WriteLine($"+{amount} Coin | Thông Tin : {account.userId} - Đang có : {account.currentAmount} Coin ");
                            UpdateAccount(account);
                        }
                        else if (responseContent.Contains("system error"))
                        {
                            Console.WriteLine("Lỗi system error !!! Đang tiến hành thử lại.... ");
                        }
                        else if (responseContent.Contains("left coin not enough"))
                        {
                            Console.WriteLine("Không đủ coin để nhận , tiến hành bỏ qua... ");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Lỗi: {response.StatusCode} Không xác định , vui lòng liên hệ Admin để xử lý !! ");
                    }
                    client.Dispose();
                    await Task.Delay(sleep * 1000);
                }
            }
        }
        public static async Task<Account> GetInfomationNow(string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.yescoin.gold/");
            httpClient.DefaultRequestHeaders.Add("Token", token);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36");

            HttpResponseMessage response = await httpClient.GetAsync("account/getAccountInfo");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                JObject responseJson = JObject.Parse(responseBody);
                JObject data = (JObject)responseJson["data"];
                int totalAmount = (int)data["totalAmount"];
                int currentAmount = (int)data["currentAmount"];
                int rank = (int)data["rank"];
                int userLevel = (int)data["userLevel"];
                string userId = (string)data["userId"];
                Account account = new Account(userId, userLevel, rank, totalAmount, currentAmount, token);
                return account;

            }
            else
            {
                Console.WriteLine($"Failed to get data, status code: {response.StatusCode}");
                return null;
            }
        }
        static void AddAccountToFile(Account newAccount)
        {
            using (StreamWriter writer = File.AppendText("accounts.txt"))
            {
                writer.WriteLine($"{newAccount.userId}|{newAccount.userLevel}|{newAccount.rankingUser}|{newAccount.currentAmount}|{newAccount.currentAmount}|{newAccount.tokenAccount}");
            }
            if (FindLineByToken(newAccount.tokenAccount) != "0")
            {
                Console.WriteLine($"Thêm thành công tài khoản với thông tin ID = {newAccount.userId}| Level = {newAccount.userLevel}| Coin = {newAccount.rankingUser}");
                Thread.Sleep(1000);
                Program.InitializeAccountMenu();
            }
        }
        public static string FindLineByToken(string token)
        {
            try
            {
                using (StreamReader reader = new StreamReader("accounts.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length >= 6)
                        {
                            string tokenAccount = parts[5];
                            if (tokenAccount == token)
                            {
                                return line;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi đọc tệp văn bản: " + ex.Message);
            }

            return "0";
        }
        public static string getInfomationAllAccount()
        {
            string allAccountInfo = "";

            using (StreamReader reader = new StreamReader("accounts.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length >= 5)
                    {
                        allAccountInfo += $"ID Tài khoản = {parts[0]}|Level hiện tại = {parts[1]}|Rank = {parts[2]}|Coin hiện tại = {parts[3]}|Coin đã thu thập = {parts[4]}\n";
                    }
                }
            }

            return allAccountInfo;
        }
        public static bool DeleteAccount(string userId)
        {
            string filePath = "accounts.txt";
            string[] lines = File.ReadAllLines(filePath);

            bool found = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split('|');
                if (parts.Length >= 6 && parts[0] == userId)
                {
                    found = true;
                    lines[i] = null;
                    break;
                }
            }

            if (found)
            {
                File.WriteAllLines(filePath, lines);
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void DeleteAllAccounts()
        {
            string filePath = "accounts.txt";

            if (File.Exists(filePath))
            {
                File.WriteAllText(filePath, string.Empty);
            }
            else
            {
            }
        }
        public static void UpdateAccount(Account updatedAccount)
        {
            string filePath = "accounts.txt";
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split('|');
                if (parts.Length >= 6 && parts[0] == updatedAccount.userId)
                {
                    lines[i] = $"{updatedAccount.userId}|{updatedAccount.userLevel}|{updatedAccount.rankingUser}|{updatedAccount.totalAmount}|{updatedAccount.currentAmount}|{updatedAccount.tokenAccount}";
                    break;
                }
            }

            File.WriteAllLines(filePath, lines);
        }
        public static List<string> ReadTokensFromFile()
        {
            List<string> tokens = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader("accounts.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length == 6)
                        {
                            tokens.Add(parts[5]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi khi đọc file: {e.Message}");
            }

            return tokens;
        }
    }
}
