// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using AutoMapper;
using Broker.Domain;
using NUnit.Framework;

namespace Broker.Tests
{
    public class AutoMapperTests
    {
        [Test]
        public void Test_AutoMapper_Configuration_Is_Valid()
        {
            AutoMapperDomainConfig.Configure();
            Mapper.AssertConfigurationIsValid();
        }
    }
}
