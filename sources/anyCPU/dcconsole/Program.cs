using DualConnector;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace dcconsole
{
    internal class Program
	{
        #region Private data
        private const string fnResult = "result.txt";
		private const string fnReceipt = "receipt.txt";
		private const int MAX_COUNT_SAFIELDS = 99;
        #endregion

        #region Main
        private static void Main(string[] args)
		{
			CmdArgs cmdArgs = new CmdArgs();
			if (cmdArgs.Parse(args) != 0)
			{
				Environment.Exit(101);
				return;
			}
			ISAPacket iSAPacket = new SAPacket();
			ISAPacket iSAPacket2 = new SAPacket();
			DCLink dCLink = new DCLink();
			int num = dCLink.InitResources();
			if (num == 0)
			{
				string text = cmdArgs.Get(CmdArgs.CMD_ARGUMENTS.C_PORT);
				string text2 = cmdArgs.Get(CmdArgs.CMD_ARGUMENTS.C_BAUDERATE);
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
				{
					long nCOM = 1L;
					long baudRate = 115200L;
					long.TryParse(text, out nCOM);
					long.TryParse(text2, out baudRate);
					num = dCLink.SetChannelTerminalParam(nCOM, baudRate, 8L, 0L, 0L, 2L);
					if (num != 0)
					{
						num = 102;
						goto IL_332;
					}
				}
				iSAPacket.Amount = cmdArgs.Get(CmdArgs.CMD_ARGUMENTS.C_AMOUNT);
				int num2;
				if (int.TryParse(cmdArgs.Get(CmdArgs.CMD_ARGUMENTS.C_OPERID), out num2))
				{
					iSAPacket.OperationCode = num2;
				}
				iSAPacket.CurrencyCode = cmdArgs.Get(CmdArgs.CMD_ARGUMENTS.C_CURRENCY);
				iSAPacket.TerminalID = cmdArgs.Get(CmdArgs.CMD_ARGUMENTS.C_TERMINALID);
				iSAPacket.ReferenceNumber = cmdArgs.Get(CmdArgs.CMD_ARGUMENTS.C_REFNUM);
				if (int.TryParse(cmdArgs.Get(CmdArgs.CMD_ARGUMENTS.C_TRANSID), out num2))
				{
					iSAPacket.TerminalTrxID = num2;
				}
				iSAPacket.TRACK2 = cmdArgs.Get(CmdArgs.CMD_ARGUMENTS.C_TRACK);
				iSAPacket.AuthorizationCode = cmdArgs.Get(CmdArgs.CMD_ARGUMENTS.C_AUTHCODE);
				if (int.TryParse(cmdArgs.Get(CmdArgs.CMD_ARGUMENTS.C_MODE1), out num2))
				{
					iSAPacket.CommandMode = num2;
				}
				if (int.TryParse(cmdArgs.Get(CmdArgs.CMD_ARGUMENTS.C_MODE), out num2))
				{
					iSAPacket.CommandMode2 = num2;
				}
				string text3 = cmdArgs.Get(CmdArgs.CMD_ARGUMENTS.C_SPAN);
				int maxTimeoutms = 180000;
				if (!string.IsNullOrEmpty(text3))
				{
					int.TryParse(text3, out maxTimeoutms);
				}
				num = dCLink.Exchange(ref iSAPacket, ref iSAPacket2, maxTimeoutms);
				if (num == 0)
				{
					string expr_1A8 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar.ToString();
					string path = expr_1A8 + fnResult;
					string path2 = expr_1A8 + fnReceipt;
					try
					{
						File.Delete(path);
						File.Delete(path2);
					}
					catch (Exception ex)
					{
						Trace.TraceError("Error delete file: {0}", new object[]
						{
							ex.Message
						});
						num = 104;
						goto IL_332;
					}
					FileStream fileStream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read);
					for (int i = 0; i < MAX_COUNT_SAFIELDS; i++)
					{
						if (i == 90)
						{
							if (!string.IsNullOrEmpty(iSAPacket2.ReceiptData))
							{
								int num3 = 0;
								int.TryParse(cmdArgs.Get(CmdArgs.CMD_ARGUMENTS.C_RECEIPTFLAG), out num3);
								FileStream fileStream2 = File.Open(path2, FileMode.Create, FileAccess.Write, FileShare.Read);
								if (num3 != 1)
								{
									byte[] bytes = Helpers.Encoding.GetBytes(iSAPacket2.ReceiptData);
									fileStream2.Write(bytes, 0, bytes.Length);
								}
								else
								{
									string printableTag = Helpers.GetPrintableTag(223, 1, iSAPacket2.ReceiptData);
									if (!string.IsNullOrEmpty(printableTag))
									{
										byte[] bytes2 = Helpers.Encoding.GetBytes(printableTag);
										fileStream2.Write(bytes2, 0, bytes2.Length);
									}
									string printableTag2 = Helpers.GetPrintableTag(218, 1, iSAPacket2.ReceiptData);
									if (!string.IsNullOrEmpty(printableTag2))
									{
										byte[] bytes3 = Helpers.Encoding.GetBytes(printableTag2);
										fileStream2.Write(bytes3, 0, bytes3.Length);
									}
								}
								fileStream2.Close();
							}
						}
						else
						{
							byte[] bufFromSAField = Helpers.GetBufFromSAField(i, iSAPacket2);
							if (bufFromSAField != null)
							{
								fileStream.Write(bufFromSAField, 0, bufFromSAField.Length);
							}
						}
					}
					fileStream.Close();
				}
				dCLink.FreeResources();
			}
			IL_332:
			Environment.Exit(num);
		}
        #endregion
    }
}
