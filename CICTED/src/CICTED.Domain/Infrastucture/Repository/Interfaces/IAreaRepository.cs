﻿using CICTED.Domain.Entities.Trabalho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository.Interfaces
{
    public interface IAreaRepository
    {
        Task<SubAreaConhecimento> GetSubArea(int areaId);
    }
}
