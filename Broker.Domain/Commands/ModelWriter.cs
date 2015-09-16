// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Broker.Domain.Models;
using Broker.Persistance;
using CarFinder.Api.Contracts;

namespace Broker.Domain.Commands
{
    public interface IModelWriter
    {
        ModelDto AddModel(VehicleMetaData model);
    }

    public class ModelWriter : IModelWriter
    {
        private readonly Entities _context;

        public ModelWriter(Entities context)
        {
            _context = context;
        }

        public ModelDto AddModel(VehicleMetaData model)
        {
            var carModel = new Model
            {

            };

            _context.Models.Add(carModel);
            _context.SaveChanges();

            return Mapper.Map<ModelDto>(carModel);
        }
    }
}
