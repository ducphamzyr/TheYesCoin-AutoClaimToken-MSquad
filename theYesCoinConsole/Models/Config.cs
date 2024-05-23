using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theYesCoinConsole.Models
{
    public class Config
    {
        public Config(int coinCollectAmount, int coinCollectSleep)
        {
            this.coinCollectAmount = coinCollectAmount;
            this.coinCollectSleep = coinCollectSleep;
        }

        public int coinCollectAmount { get; set; }
        public int coinCollectSleep { get; set; }

        // function

        public static Config GetConfigureRun()
        {
            string jsonConfigure = File.ReadAllText("config.json");
            Config config = JsonConvert.DeserializeObject<Config>(jsonConfigure);
            return config;
        }
        public static bool ApplyChangeConfigure(int Amount, int Sleep)
        {
            Config config = new Config(Amount, Sleep);
            if (config != null)
            {
                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText("config.json", json);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
