﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Shared.Enums;

namespace KMN_Tontine.Shared.DTOs.Requests
{
    public class CreateTransactionRequest
    {
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int AccountId { get; set; }
        public int TontineId { get; set; }
    }
}
