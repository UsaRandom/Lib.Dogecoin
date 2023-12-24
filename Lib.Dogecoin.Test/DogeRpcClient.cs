
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lib.Dogecoin.Test
{
	public class JsonRpcRequest
	{
		public string Jsonrpc { get; set; } = "2.0";
		public string Method { get; set; }
		public object[] Params { get; set; }
		public int Id { get; set; } = 1;
	}

	public class DogecoinRpcClient
	{
		private readonly HttpClient httpClient;
		private string rpcUrl = "";

		private string Username = "";
		private string Password = "";

		public DogecoinRpcClient(string rpcUrl, string username, string password)
		{
			this.rpcUrl = rpcUrl;
			Username = username;
			Password = password;
			
		}


		public string AddAddressToWatch(string address, string label)
		{
			return Call("importaddress", $"[\"{address}\", \"{label}\", false]");
		}


		public string ListUnspent(string address)
		{
			return Call("listunspent", "[0, 99999999, [\""+address+"\"]]");
		}

		public string BroadcastRawTransaction(string transaction)
		{
			return Call("sendrawtransaction", "[\""+transaction+"\"]");
		}
		private string Call(string method, string parameters)
		{
			var CredentialCache = new CredentialCache();
			CredentialCache.Add(new Uri(rpcUrl), "Basic", new NetworkCredential(Username, Password));

			var httpWebRequest = (HttpWebRequest)WebRequest.Create(rpcUrl);
			httpWebRequest.ContentType = "text/json";
			httpWebRequest.Method = "POST";
			httpWebRequest.Credentials = CredentialCache;

			using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				string json;
				json = "{ \"jsonrpc\": \"2.0\", \"id\":\"" + Guid.NewGuid().ToString() + "\", \"method\": \""+method+"\",\"params\":"+parameters+"}";

				streamWriter.Write(json);
			}
			var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
			{
				var responseText = streamReader.ReadToEnd();
				return responseText;
			}
		}
	}

}
