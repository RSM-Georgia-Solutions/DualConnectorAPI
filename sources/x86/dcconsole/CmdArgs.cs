using System;
using System.Diagnostics;

namespace dcconsole
{
	internal class CmdArgs
	{
        #region Arguments
        public enum CMD_ARGUMENTS
		{
			NO_ARG,
			C_PORT,
			C_BAUDERATE,
			C_AMOUNT,
			C_OPERID,
			C_CURRENCY,
			C_TERMINALID,
			C_REFNUM,
			C_TRANSID,
			C_TRACK,
			C_AUTHCODE,
			C_MODE1,
			C_MODE,
			C_RECEIPTFLAG,
			C_SPAN,
			C_COUNT
		}
        #endregion

        #region Private voids
        private class ARGS_TAB
		{
			internal static CmdArgs.ARGS_TAB[] tabArgum = new CmdArgs.ARGS_TAB[]
			{
				new CmdArgs.ARGS_TAB(CmdArgs.CMD_ARGUMENTS.C_PORT, "-p"),
				new CmdArgs.ARGS_TAB(CmdArgs.CMD_ARGUMENTS.C_BAUDERATE, "-b"),
				new CmdArgs.ARGS_TAB(CmdArgs.CMD_ARGUMENTS.C_AMOUNT, "-a"),
				new CmdArgs.ARGS_TAB(CmdArgs.CMD_ARGUMENTS.C_OPERID, "-o"),
				new CmdArgs.ARGS_TAB(CmdArgs.CMD_ARGUMENTS.C_CURRENCY, "-c"),
				new CmdArgs.ARGS_TAB(CmdArgs.CMD_ARGUMENTS.C_TERMINALID, "-z"),
				new CmdArgs.ARGS_TAB(CmdArgs.CMD_ARGUMENTS.C_REFNUM, "-r"),
				new CmdArgs.ARGS_TAB(CmdArgs.CMD_ARGUMENTS.C_TRANSID, "-n"),
				new CmdArgs.ARGS_TAB(CmdArgs.CMD_ARGUMENTS.C_TRACK, "-t"),
				new CmdArgs.ARGS_TAB(CmdArgs.CMD_ARGUMENTS.C_AUTHCODE, "-u"),
				new CmdArgs.ARGS_TAB(CmdArgs.CMD_ARGUMENTS.C_MODE1, "-l"),
				new CmdArgs.ARGS_TAB(CmdArgs.CMD_ARGUMENTS.C_MODE, "-m"),
				new CmdArgs.ARGS_TAB(CmdArgs.CMD_ARGUMENTS.C_RECEIPTFLAG, "-f"),
				new CmdArgs.ARGS_TAB(CmdArgs.CMD_ARGUMENTS.C_SPAN, "-s"),
				new CmdArgs.ARGS_TAB()
			};

			public readonly CmdArgs.CMD_ARGUMENTS arg;

			public readonly string txtName;

			public string value;

			private ARGS_TAB()
			{
				this.arg = CmdArgs.CMD_ARGUMENTS.NO_ARG;
				this.txtName = null;
				this.value = null;
			}

			private ARGS_TAB(CmdArgs.CMD_ARGUMENTS _arg, string _txt) : this()
			{
				this.arg = _arg;
				this.txtName = _txt;
			}

			public static void UpdateArg(CmdArgs.CMD_ARGUMENTS arg, string val)
			{
				CmdArgs.ARGS_TAB[] array = CmdArgs.ARGS_TAB.tabArgum;
				for (int i = 0; i < array.Length; i++)
				{
					CmdArgs.ARGS_TAB aRGS_TAB = array[i];
					if (aRGS_TAB.arg == arg)
					{
						aRGS_TAB.value = val;
						return;
					}
				}
			}

			public static string GetArg(CmdArgs.CMD_ARGUMENTS arg)
			{
				CmdArgs.ARGS_TAB[] array = CmdArgs.ARGS_TAB.tabArgum;
				for (int i = 0; i < array.Length; i++)
				{
					CmdArgs.ARGS_TAB aRGS_TAB = array[i];
					if (aRGS_TAB.arg == arg)
					{
						return aRGS_TAB.value;
					}
				}
				return null;
			}
		}

		private const string ccParamPort = "-p";

		private const string ccParamBRate = "-b";

		private const string ccParamAmount = "-a";

		private const string ccParamOperID = "-o";

		private const string ccParamCurrency = "-c";

		private const string ccParamTerminalID = "-z";

		private const string ccParamRefNum = "-r";

		private const string ccParamTransID = "-n";

		private const string ccParamTrack = "-t";

		private const string ccParamAuthCode = "-u";

		private const string ccParamMode1 = "-l";

		private const string ccParamMode = "-m";

		private const string ccParamReceiptFlag = "-f";

		private const string ccParamSpan = "-s";

		public int Parse(string[] args)
		{
            if (args.Length != 0)
            {
                try
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        string text = args[i];
                        CmdArgs.ARGS_TAB[] tabArgum = CmdArgs.ARGS_TAB.tabArgum;
                        for (int j = 0; j < tabArgum.Length; j++)
                        {
                            CmdArgs.ARGS_TAB aRGS_TAB = tabArgum[j];
                            if (text.StartsWith(aRGS_TAB.txtName))
                            {
                                CmdArgs.ARGS_TAB.UpdateArg(aRGS_TAB.arg, text.Substring(aRGS_TAB.txtName.Length));
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Parse cmd line: {0}", new object[]
                    {
                    ex.Message
                    });
                    int i = 1;
                    return i;
                }

                return 0;
            }
			return 1;
		}

		public string Get(CmdArgs.CMD_ARGUMENTS arg)
		{
			return CmdArgs.ARGS_TAB.GetArg(arg);
		}
        #endregion
    }
}
