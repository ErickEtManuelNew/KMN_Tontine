﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Shared.Enums;

namespace KMN_Tontine.Shared.DTOs.Responses
{
    public class PaymentPromiseResponse
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PromiseDate { get; set; }
        public DateTime? FulfilledDate { get; set; }
        public bool IsFulfilled { get; set; }
        public string Reference { get; set; }
        public string MemberFullName { get; set; }
        public decimal TotalAmountPromised { get; set; }
        public decimal AmountPaid { get; set; }
        public ICollection<PaymentPromiseAccountResponse> Accounts { get; set; }
    }

    public class PaymentPromiseAccountResponse
    {
        public int AccountId { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountName { get; set; }
        public decimal AmountPromised { get; set; }
    }
}
