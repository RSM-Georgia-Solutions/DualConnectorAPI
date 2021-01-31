using DualConnector;
using System.Globalization;
using System.Text;

namespace dcconsole
{
    public static class Helpers
    {
        #region Static voids
        public static byte[] GetBufFromSAField(int numField, ISAPacket packet)
        {
            string text = null;
            switch (numField)
            {
                case 0:
                    text = packet.Amount;
                    break;
                case 1:
                    text = packet.AdditionalAmount;
                    break;
                case 4:
                    text = packet.CurrencyCode;
                    break;
                case 6:
                    text = packet.DateTimeHost;
                    break;
                case 8:
                    text = ((packet.CardEntryMode == -1) ? null : packet.CardEntryMode.ToString());
                    break;
                case 9:
                    text = ((packet.PINCodingMode == -1) ? null : packet.PINCodingMode.ToString());
                    break;
                case 10:
                    text = packet.PAN;
                    break;
                case 11:
                    text = packet.CardExpiryDate;
                    break;
                case 12:
                    text = packet.TRACK2;
                    break;
                case 13:
                    text = packet.AuthorizationCode;
                    break;
                case 14:
                    text = packet.ReferenceNumber;
                    break;
                case 15:
                    text = packet.ResponseCodeHost;
                    break;
                case 16:
                    text = packet.PinBlock;
                    break;
                case 17:
                    text = packet.PinKey;
                    break;
                case 18:
                    text = packet.WorkKey;
                    break;
                case 19:
                    text = packet.TextResponse;
                    break;
                case 21:
                    text = packet.TerminalDateTime;
                    break;
                case 23:
                    text = ((packet.TrxID == -1) ? null : packet.TrxID.ToString());
                    break;
                case 25:
                    text = packet.OperationCode.ToString();
                    break;
                case 26:
                    text = ((packet.TerminalTrxID == -1) ? null : packet.TerminalTrxID.ToString());
                    break;
                case 27:
                    text = packet.TerminalID;
                    break;
                case 28:
                    text = packet.MerchantID;
                    break;
                case 29:
                    text = packet.DebitAmount;
                    break;
                case 30:
                    text = packet.DebitCount;
                    break;
                case 31:
                    text = packet.CreditAmount;
                    break;
                case 32:
                    text = packet.CreditCount;
                    break;
                case 34:
                    text = ((packet.OrigOperation == -1) ? null : packet.OrigOperation.ToString());
                    break;
                case 36:
                    text = packet.MAC;
                    break;
                case 39:
                    text = packet.Status.ToString();
                    break;
                case 40:
                    text = packet.AdminTrack2;
                    break;
                case 41:
                    text = packet.AdminPinBlock;
                    break;
                case 42:
                    text = packet.AdminPAN;
                    break;
                case 43:
                    text = packet.AdminCardExpiryDate;
                    break;
                case 46:
                    text = ((packet.AdminCardEntryMode == -1) ? null : packet.AdminCardEntryMode.ToString());
                    break;
                case 49:
                    text = packet.VoidDebitAmount;
                    break;
                case 50:
                    text = packet.VoidDebitCount;
                    break;
                case 51:
                    text = packet.VoidCreditAmount;
                    break;
                case 52:
                    text = packet.VoidCreditCount;
                    break;
                case 53:
                    text = ((packet.ProcessingFlag == -1) ? null : packet.ProcessingFlag.ToString());
                    break;
                case 54:
                    text = ((packet.HostTrxID == -1) ? null : packet.HostTrxID.ToString());
                    break;
                case 56:
                    text = ((packet.RecipientAddress == -1) ? null : packet.RecipientAddress.ToString());
                    break;
                case 57:
                    text = ((packet.CardWaitTimeout == -1) ? null : packet.CardWaitTimeout.ToString());
                    break;
                case 63:
                    text = packet.DeviceSerNumber;
                    break;
                case 64:
                    text = ((packet.CommandMode == -1) ? null : packet.CommandMode.ToString());
                    break;
                case 65:
                    text = ((packet.CommandMode2 == -1) ? null : packet.CommandMode2.ToString());
                    break;
                case 67:
                    text = ((packet.CommandResult == -1) ? null : packet.CommandResult.ToString());
                    break;
                case 70:
                    text = packet.FileData;
                    break;
                case 76:
                    text = packet.CashierRequest;
                    break;
                case 77:
                    text = packet.CashierResponse;
                    break;
                case 79:
                    text = packet.AccountType;
                    break;
                case 80:
                    text = packet.CommodityCode;
                    break;
                case 81:
                    text = packet.PaymentDetails;
                    break;
                case 82:
                    text = packet.ProviderCode;
                    break;
                case 83:
                    text = packet.Acquirer;
                    break;
                case 86:
                    text = packet.AdditionalData;
                    break;
                case 90:
                    text = packet.ReceiptData;
                    break;
            }
            if (!string.IsNullOrEmpty(text))
            {
                string s = string.Format("[{0:00}] = '{1}'\n", numField, text);
                return Encoding.GetEncoding(1251).GetBytes(s);
            }
            return null;
        }

        public static string GetPrintableTag(byte tag, byte position, string data)
        {
            int num = 0;
            if (!string.IsNullOrEmpty(data))
            {
                while (num != -1)
                {
                    int num2 = data.IndexOf('^', num);
                    if (num2 == -1)
                    {
                        break;
                    }
                    string arg_40_0 = data.Substring(num, num2 - num);
                    byte b = 0;
                    if (!byte.TryParse(arg_40_0.TrimStart(new char[]
                    {
                        '0',
                        'X',
                        'x',
                        '~'
                    }), NumberStyles.AllowHexSpecifier, null, out b))
                    {
                        break;
                    }
                    if (b == tag)
                    {
                        int num3 = data.IndexOf('^', num2 + 1);
                        if (num3 == -1)
                        {
                            break;
                        }
                        if (position == 0)
                        {
                            return data.Substring(num2 + 1, num3 - num2 - 1);
                        }
                        if (position == 1)
                        {
                            int num4 = data.IndexOf('~', num3 + 1);
                            if (num4 == -1)
                            {
                                num4 = data.Length;
                            }
                            return data.Substring(num3 + 1, num4 - num3 - 1);
                        }
                        break;
                    }
                    else
                    {
                        num = data.IndexOf('~', num2);
                    }
                }
            }
            return null;
        }

        public static Encoding Encoding
        {
            get
            {
                return Encoding.GetEncoding(1251);
            }
        }
        #endregion

        #region Enums
        public enum Operation
        {
            Sale = 1,
            Cancel = 4,
            Return = 29,
            Reconciliation = 59,   
            Emergency = 56,
            Test = 26,
            Report = 63,
        }
        #endregion
    }
}
