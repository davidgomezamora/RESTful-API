﻿using Common.DTO;
using Infraestructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Services
{
    public interface IProductModelService
    {
        ProductModelDto GetProductModel<T>(T productModelId);
    }
}
