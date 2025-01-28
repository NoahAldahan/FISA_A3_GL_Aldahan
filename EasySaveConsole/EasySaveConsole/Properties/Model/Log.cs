using System;
using System.IO;


public class Log
{
	private string message { get; set; }

	public void WriteLog(string message)
	{
		this.message = message;
	}
}