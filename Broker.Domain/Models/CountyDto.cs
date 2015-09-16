﻿// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System;

namespace Broker.Domain.Models
{
    public class CountyDto
    {
        public int CountyId { get; set; }
        public string CountyName { get; set; }
        public DateTime? UTCDateAdded { get; set; }
    }
}
