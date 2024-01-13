using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace WinFormsApp1
{
    public partial class MainForm : Form
    {
        private readonly string duckUrl = "https://api.datamuse.com/words?ml=duck";
        private readonly string lionUrl = "https://api.datamuse.com/words?ml=lion";
        private readonly string elephantUrl = "https://api.datamuse.com/words?ml=elephant";
        private readonly Dictionary<string, int> tagCounter = new Dictionary<string, int>();


        public MainForm()
        {
            InitializeComponent();
        }

        private void handleGETData(string url)
        {
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format(url));
            WebReq.Method = "GET";
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            string jsonString;
            List<responseInJson> wordsFromAPI = new List<responseInJson>();

            using (Stream stream = WebResp.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                jsonString = reader.ReadToEnd();
                wordsFromAPI = JsonConvert.DeserializeObject<List<responseInJson>>(jsonString);
            }

            foreach(responseInJson word in wordsFromAPI)
            {
                foreach(string tag in word.tags)
                {
                    if(!tagCounter.ContainsKey(tag))
                    {
                        tagCounter.Add(tag, 0);
                    }
                    tagCounter[tag]++;
                }
            }
        }

        private void fetch_Click(object sender, EventArgs e)
        {
            handleGETData(duckUrl);
            handleGETData(lionUrl);
            handleGETData(elephantUrl);

            MessageBox.Show("Data Loaded Successfully");
        }

        private void show_Click(object sender, EventArgs e)
        {
            string answer = "";
            foreach(var item in tagCounter)
            {
                answer += "Name: " + item.Key + ". Count: " + item.Value + ". \n";
            }

            MessageBox.Show(answer);
        }

        public class responseInJson
        {
            public string word { get; set; }
            public int score { get; set; }
            public IList<string> tags { get; set; }
        }
        public class wordFromAPI
        {
            public string word { get; set; }
            public int score { get; set; }
        }

    }
}
