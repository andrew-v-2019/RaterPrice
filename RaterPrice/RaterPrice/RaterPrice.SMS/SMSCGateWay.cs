using System;
using System.Web;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using RaterPrice.SMS.DTO;

namespace RaterPrice.SMS
{

    public class SMSCGateWay : ISmsGateWay
    {

        public SMSCGateWay(string login, string password)
        {
            SMSC_LOGIN = login;
            SMSC_PASSWORD = password;
            SMSC_CHARSET = "utf-8";
        }

        private string SMSC_LOGIN;            // логин клиента 
        private string SMSC_PASSWORD;    // пароль или MD5-хеш пароля в нижнем регистре 
        private string SMSC_CHARSET;        // кодировка сообщения (windows-1251 или koi8-r), по умолчанию используется utf-8 

        private string[][] D2Res;

        private const int SuccessStatusCode = 1;

        public SendSmsResult SendSms(List<string> phones, string message, int messageId, string senderName)
        {
            var result = SendSms(phones.Aggregate((a, b) => a + ", " + b), message, id: messageId);
            var model = GetSendSmsResultFromString(result);
            return model;
        }


        public CheckSmsStatusResult GetStatus(int smsSendId, string phone)
        {
            var result = GetStatus(smsSendId.ToString(), phone, 2);
            var model = GetCheckStatusResultFromString(result);
            return model;
        }

        private SendSmsResult GetSendSmsResultFromString(string[] result)
        {
            var r = DeserializeStrings(result);
            var model = new SendSmsResult()
            {
                RequestResult = new RequestResult()
                {
                    TextView = result.Aggregate((a, b) => a + " " + b),
                    ErrorCode = null,
                    ErrorText = ""
                }
            };

            if (Contains(r, "id"))
            {
                model.SendSmsId = (int?)r["id"];
            }

            if (Contains(r, "error_code"))
            {
                model.RequestResult.ErrorCode = (int)r["error_code"];
                model.RequestResult.ErrorText = r["error"].ToString();
            }
            return model;
        }


        private Dictionary<string, object> DeserializeStrings(string[] source)
        {
            var resultStr = source.Aggregate((a, b) => a + ", " + b);//.Replace("\n", "").Replace("\"", "");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var r = (Dictionary<string, object>)serializer.DeserializeObject(resultStr);
            return r;
        }

        private bool Contains(Dictionary<string, object> source, string key)
        {
            return (source.ContainsKey(key) && !string.IsNullOrEmpty(source[key].ToString()));
        }

        private CheckSmsStatusResult GetCheckStatusResultFromString(string[] result)
        {
            var r = DeserializeStrings(result);
            var model = new CheckSmsStatusResult()
            {
                RequestResult = new RequestResult()
                {
                    TextView = result.Aggregate((a, b) => a + " " + b),
                    ErrorCode = null,
                    ErrorText = ""
                },
            };

            if (Contains(r, "status"))
            {
                model.Status = (int?)r["status"];
            }

            if (Contains(r, "cost"))
            {
                model.Cost = Convert.ToDecimal(r["cost"].ToString().Replace('.',','));
            }

            if (Contains(r, "status_name"))
            {
                model.StatusText = r["status_name"].ToString();
            }

            if (Contains(r, "last_date"))
            {
                model.LastStatusChangeDate = Convert.ToDateTime(r["last_date"].ToString());
            }

            if (Contains(r, "send_date"))
            {
                model.SendDate = Convert.ToDateTime(r["send_date"].ToString());
            }

            if (Contains(r, "error_code"))
            {
                model.RequestResult.ErrorCode = (int)r["error_code"];
                model.RequestResult.ErrorText = r["error"].ToString();
            }
            model.Delivered = model.Status == SuccessStatusCode;
            return model;
        }

        private string[] GetStatus(string id, string phone, int all = 2)
        {
            string[] m = _smsc_send_cmd("status", "phone=" + _urlencode(phone) + "&id=" + _urlencode(id) + "&all=" + all.ToString() + "&fmt=3");

            // (status, time, err, ...) или (0, -error) 

            if (id.IndexOf(',') == -1)
            {
                int idx = all == 1 ? 9 : 12;

                if (all > 0 && m.Length > idx && (m.Length < idx + 5 || m[idx + 5] != "HLR"))
                    m = String.Join(",", m).Split(",".ToCharArray(), idx);
            }
            else
            {
                if (m.Length == 1 && m[0].IndexOf('-') == 2)
                    return m[0].Split(',');

                Array.Resize(ref D2Res, 0);
                Array.Resize(ref D2Res, m.Length);

                for (int i = 0; i < D2Res.Length; i++)
                    D2Res[i] = m[i].Split(',');

                Array.Resize(ref m, 1);
                m[0] = "1";
            }
            return m;
        }


        private string GetBalance()
        {
            string[] m = _smsc_send_cmd("balance", ""); // (balance) или (0, -error) 
            return m.Length == 1 ? m[0] : "";
        }



        private string[] SendSms(string phones, string message, string time = "", int id = 0, string sender = "")
        {
            string[] m = _smsc_send_cmd("send", "cost=3&phones=" + _urlencode(phones)
                            + "&mes=" + _urlencode(message) + "&id=" + id.ToString() + "&translit=0"
                            + (sender != "" ? "&sender=" + _urlencode(sender) : "")
                            + (time != "" ? "&time=" + _urlencode(time) : ""));

            return m;
        }



        private string[] _smsc_send_cmd(string cmd, string arg)
        {
            arg = "login=" + _urlencode(SMSC_LOGIN) + "&psw=" + _urlencode(SMSC_PASSWORD) + "&fmt=3&charset=" + SMSC_CHARSET + "&" + arg;

            string url = ("http") + "://smsc.ru/sys/" + cmd + ".php" + "?" + arg;

            string ret;
            int i = 0;
            HttpWebRequest request;
            StreamReader sr;
            HttpWebResponse response;

            do
            {
                if (i > 0)
                    System.Threading.Thread.Sleep(2000 + 1000 * i);

                if (i == 2)
                    url = url.Replace("://smsc.ru/", "://www2.smsc.ru/");

                request = (HttpWebRequest)WebRequest.Create(url);
                try
                {
                    response = (HttpWebResponse)request.GetResponse();

                    sr = new StreamReader(response.GetResponseStream());
                    ret = sr.ReadToEnd();
                }
                catch (WebException)
                {
                    ret = "";
                }
            }
            while (ret == "" && ++i < 4);

            if (ret == "")
            {
                ret = ","; // фиктивный ответ 
            }

            char delim = ',';

            if (cmd == "status")
            {
                string[] par = arg.Split('&');

                for (i = 0; i < par.Length; i++)
                {
                    string[] lr = par[i].Split("=".ToCharArray(), 2);

                    if (lr[0] == "id" && lr[1].IndexOf("%2c") > 0) // запятая в id - множественный запрос 
                        delim = '\n';
                }
            }

            return ret.Split(delim);
        }

        // кодирование параметра в http-запросе 
        private string _urlencode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        // объединение байтовых массивов 
        private byte[] _concatb(byte[] farr, byte[] sarr)
        {
            int opl = farr.Length;

            Array.Resize(ref farr, farr.Length + sarr.Length);
            Array.Copy(sarr, 0, farr, opl, sarr.Length);

            return farr;
        }


    }
}