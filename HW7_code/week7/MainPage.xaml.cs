using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Data.Xml.Dom;
using Windows.UI.Popups;


//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace week7
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Get_Xml(long phone)
        {
            try
            {
                er.Text = "waiting for a reponse...";
                // 创建一个 httpClient 对象
                HttpClient httpClient = new HttpClient();

                // add a user-agent header
                var header = httpClient.DefaultRequestHeaders;

                // use the PaeseAdd method to add a header value
                header.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.3; WOW64; rv:37.0) Gecko/20100101 Firefox/37.0");
                string getURL = "http://life.tenpay.com/cgi-bin/mobile/MobileQueryAttribution.cgi?chgmobile={0}";
                string url = string.Format(getURL, phone);

                HttpResponseMessage response = await httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();
                er.Text = response.StatusCode + " " + response.ReasonPhrase + Environment.NewLine;

                // 返回的字节流中含有中文，需要进行编码才可正常显示
                Byte[] rescontent = await response.Content.ReadAsByteArrayAsync();

                // 注册编码
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                // 用gb2312进行编码
                Encoding utf = Encoding.GetEncoding("gb2312");
                string Srescontent = utf.GetString(rescontent, 0, rescontent.Length);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(Srescontent);

                XmlElement root = xmlDoc.DocumentElement;
                XmlNodeList pro = root.SelectNodes("/root/province");
                XmlNodeList city = root.SelectNodes("/root/city");
                XmlNodeList supp = root.SelectNodes("/root/supplier");

                prov.Text = pro[0].InnerText;
                citys.Text = city[0].InnerText;
                supps.Text = supp[0].InnerText;

            }
            catch (HttpRequestException hre)
            {
                er.Text = hre.ToString();
            }
            catch (Exception ex)
            {
                er.Text = ex.ToString();
            }
        }
        private void Xml_Click(object sender, RoutedEventArgs e)
        {
            if (box.Text.Length != 11)
            {
                var i = new MessageDialog("invalid phone number").ShowAsync();
            }
            else
            {
                prov.Text = "";
                citys.Text = "";
                er.Text = "";
                supps.Text = "";
                Get_Xml(Convert.ToInt64(box.Text));
            }
        }

        private async void Get_Json(long phone)
        {
            try
            {
                er.Text = "waiting for a reponse...";
                // 创建一个 httpClient 对象
                HttpClient httpClient = new HttpClient();

                // add a user-agent header
                var headers = httpClient.DefaultRequestHeaders;

                // use the PaeseAdd method to add a header value
                string header = "ie Mozilla/5.0 (Windows NT 6.2; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";
                if (!headers.UserAgent.TryParseAdd(header))
                {
                    throw new Exception("Invalid header value: " + header);
                }
                // 添加apikey， 为了使用百度的api
                headers.Add("apikey", "20fd72b2dc9f02f4a248a17891e6ace3");
                string getURL = "http://apis.baidu.com/apistore/mobilephoneservice/mobilephone?tel={0}";
                string url = string.Format(getURL, phone);

                //发送GET请求
                HttpResponseMessage response = await httpClient.GetAsync(url);

                // 确保返回值为成功状态
                response.EnsureSuccessStatusCode();

                // 返回的字节流中含有中文，需要进行编码才可正常显示
                Byte[] getByte = await response.Content.ReadAsByteArrayAsync();

                // 采用UTF-8进行编码
                Encoding code = Encoding.GetEncoding("UTF-8");
                string result = code.GetString(getByte, 0, getByte.Length);

                // 反序列化结果字符串
                JObject res = (JObject)JsonConvert.DeserializeObject(result);
                if (res["errNum"].ToString() != "0")
                    throw (new Exception("手机号码有误"));

                if (res["retData"] != null)
                {
                    prov.Text = res["retData"]["province"].ToString();
                    supps.Text = res["retData"]["carrier"].ToString();
                }

            }
            catch (Exception e)
            {
                er.Text = e.ToString();
            }
        }
        private void Json_Click(object sender, RoutedEventArgs e)
        {
            if (box.Text.Length != 11)
            {
                var i = new MessageDialog("invalid phone number").ShowAsync();
            }
            else
            {
                prov.Text = "";
                citys.Text = "";
                er.Text = "";
                supps.Text = "";
                Get_Json(Convert.ToInt64(box.Text));
            }
        }
    }
}
