using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleSoftpayCloud.model
{

    public class PaymentRequest
    {
        public string AppId { get; set; } = "";
        public int Amount { get; set; }
        public string CurrencyCode { get; set; } = "";
        public int SchemeId { get; set; }
        public string PosReferenceNumber { get; set; } = "";
        public Options Options { get; set; } = new();
        public string EmployeeReference { get; set; } = "";
    }

    public class Options
    {
        public string Receipt { get; set; } = "";
        public string CardHolderLocale { get; set; } = "";
        public bool AmountIsoCodes { get; set; }
        public string BatchType { get; set; } = "";
        public int SwitchBackTimeout { get; set; }
        public string ActivityToResume { get; set; } = "";
        public bool SuppressAppNotification { get; set; }
        public Tipping Tipping { get; set; } = new();
        public bool Surcharge { get; set; }
        public bool Dcc { get; set; }
    }

    public class Tipping
    {
        public string Indicator { get; set; } = "";
        public string Variant { get; set; } = "";
        public int[] Percentages { get; set; } = Array.Empty<int>();
        public int HighTip { get; set; }
        public bool TipPrompt { get; set; }
    }

    public class CreateTransaction
    {
        public string Action { get; set; }
    }
}
