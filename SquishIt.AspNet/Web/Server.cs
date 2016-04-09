namespace SquishIt.AspNet.Web
{
	using System.Web;
	using Framework.Web;

	/// <summary>
	/// 
	/// </summary>
	public class Server : IServer
	{
		private readonly HttpServerUtilityBase _httpServerUtilityBase;

		public Server(HttpServerUtilityBase httpServerUtilityBase)
		{
			_httpServerUtilityBase = httpServerUtilityBase;
		}

		public string MapPath(string path)
		{
			return _httpServerUtilityBase.MapPath(path);
		}
	}
}